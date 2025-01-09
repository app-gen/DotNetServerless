using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MsgQueue.Client;
using System.Collections.Concurrent;
using System.Diagnostics;
using MsgQueue.Server;

namespace MsgQueue.Client.Serive;

public partial class MessageProcessingService : BackgroundService
{
    private readonly ILogger<MessageProcessingService> _logger;
    private readonly IMsgQueuePickService _queueService;
    private readonly MessageProcessingConfig _config;
    private readonly TelemetryService _telemetry;
    private readonly ConcurrentDictionary<string, TopicProcessingState> _topicStates;
    private readonly ConcurrentDictionary<string, List<Task>> _runningTasks;
    private readonly CancellationTokenSource _shutdownTokenSource;
    public MessageProcessingService(
        ILogger<MessageProcessingService> logger,
        IMsgQueuePickService queueService,
        IConfiguration configuration,
        TelemetryService telemetry)
    {
        _logger = logger;
        _queueService = queueService;
        _telemetry = telemetry;
        _config = new MessageProcessingConfig();
        configuration.GetSection("MessageProcessing").Bind(_config);

        _topicStates = new ConcurrentDictionary<string, TopicProcessingState>();
        _runningTasks = new ConcurrentDictionary<string, List<Task>>();

        _shutdownTokenSource = new CancellationTokenSource();

        ValidateConfiguration();
        InitializeTopicStates();
    }

    private void ValidateConfiguration()
    {
        if (!_config.Topics.Any())
        {
            throw new InvalidOperationException("At least one topic must be configured");
        }

        foreach (var topic in _config.Topics)
        {
            if (string.IsNullOrEmpty(topic.TopicId))
            {
                throw new InvalidOperationException("TopicId must be configured for all topics");
            }
            if (topic.NumberOfThreads < 1)
            {
                throw new InvalidOperationException($"NumberOfThreads must be at least 1 for topic {topic.TopicId}");
            }
        }
    }


    private void InitializeTopicStates()
    {
        foreach (var topic in _config.Topics)
        {
            _topicStates[topic.TopicId] = new TopicProcessingState
            {
                LastEmptyPollTime = DateTime.UtcNow,
                IsPaused = false,
                ActiveThreads = 0
            };
        }
    }

    private async Task ProcessMessagesAsync1(string topicId, int taskId, int pollInterval, CancellationToken stoppingToken)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["TaskId"] = taskId,
            ["TopicId"] = topicId
        });

        _logger.LogInformation("Starting processing task {TaskId} for topic {TopicId}", taskId, topicId);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAndHandlePauseAsync(topicId, _topicStates[topicId], pollInterval);

                var message = await _queueService.PickMessageAsync(topicId);

                if (message == null)
                {
                    await HandleEmptyQueueAsync(topicId, _topicStates[topicId]);
                    continue;
                }

                using var messageScope = _logger.BeginScope(new Dictionary<string, object>
                {
                    ["MessageId"] = message.MessageId
                });

                using var activity = _telemetry.StartProcessingActivity(topicId, message.MessageId.ToString());
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    await _queueService.MarkMessageAsPickedAsync(message);

                    _logger.LogInformation("Processing message {MessageId} from topic {TopicId}",
                        message.MessageId, topicId);

                    await ProcessMessageAsync(message);
                    await _queueService.MarkAsProcessedAsync(message);

                    stopwatch.Stop();
                    _telemetry.RecordMessageProcessed(topicId);
                    _telemetry.RecordProcessingDuration(topicId, stopwatch.Elapsed.TotalSeconds);

                    _logger.LogInformation("Successfully processed message {MessageId} from topic {TopicId}",
                        message.MessageId, topicId);
                }
                catch (Exception ex)
                {
                    _telemetry.RecordMessageError(topicId, ex.GetType().Name);
                    _logger.LogError(ex, "Failed to process message {MessageId} from topic {TopicId}",
                        message.MessageId, topicId);
                    await _queueService.MarkAsErrorAsync(message, ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(_config.ErrorDelaySeconds), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _telemetry.RecordMessageError(topicId, ex.GetType().Name);
                _logger.LogError(ex, "Error in processing task for topic {TopicId}", topicId);
                await Task.Delay(TimeSpan.FromSeconds(_config.ErrorDelaySeconds), stoppingToken);
            }
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Message Processing Service is starting...");

        try
        {
            // Start processing tasks for each topic
            foreach (var topic in _config.Topics)
            {
                var tasks = new List<Task>();
                for (int i = 0; i < topic.NumberOfThreads; i++)
                {
                    var taskId = i;
                    var task = ProcessMessagesAsync(topic.TopicId, taskId, topic.PollIntervalSeconds, stoppingToken);
                    tasks.Add(task);
                }
                _runningTasks[topic.TopicId] = tasks;
            }

            // Wait for all tasks to complete
            await Task.WhenAll(_runningTasks.SelectMany(x => x.Value));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the message processing service");
            throw;
        }
    }

    private async Task ProcessMessagesAsync(string topicId, int taskId, int pollInterval, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting processing task {TaskId} for topic {TopicId}", taskId, topicId);
        var state = _topicStates[topicId];

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Check if we need to pause processing
                await CheckAndHandlePauseAsync(topicId, state, pollInterval);

                // Pick a message from the queue
                var message = await _queueService.PickMessageAsync(topicId);

                if (message == null)
                {
                    await HandleEmptyQueueAsync(topicId, state);
                    continue;
                }

                // Reset pause state when we get a message
                state.IsPaused = false;
                state.LastEmptyPollTime = DateTime.MinValue;

                _logger.LogInformation("Task {TaskId} picked message {MessageId} from topic {TopicId}",
                    taskId, message.MessageId, topicId);

                // Mark the message as picked
                await _queueService.MarkMessageAsPickedAsync(message);

                try
                {
                    // Process the message
                    await ProcessMessageAsync(message);

                    // Mark the message as processed
                    await _queueService.MarkAsProcessedAsync(message);

                    _logger.LogInformation("Task {TaskId} processed message {MessageId} from topic {TopicId}",
                        taskId, message.MessageId, topicId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Task {TaskId} failed to process message {MessageId} from topic {TopicId}",
                        taskId, message.MessageId, topicId);
                    await _queueService.MarkAsErrorAsync(message, ex.Message);

                    await Task.Delay(TimeSpan.FromSeconds(_config.ErrorDelaySeconds), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Task {TaskId} encountered an error while picking message from topic {TopicId}",
                    taskId, topicId);
                await Task.Delay(TimeSpan.FromSeconds(_config.ErrorDelaySeconds), stoppingToken);
            }
        }

        _logger.LogInformation("Processing task {TaskId} for topic {TopicId} is shutting down", taskId, topicId);
    }

    private async Task CheckAndHandlePauseAsync(string topicId, TopicProcessingState state, int pollInterval)
    {
        if (state.IsPaused)
        {
            await state.PauseLock.WaitAsync();
            try
            {
                var timeSinceLastEmpty = DateTime.UtcNow - state.LastEmptyPollTime;
                if (timeSinceLastEmpty.TotalSeconds < pollInterval)
                {
                    await Task.Delay(TimeSpan.FromSeconds(pollInterval));
                }
                state.IsPaused = false;
            }
            finally
            {
                state.PauseLock.Release();
            }
        }
    }

    private async Task HandleEmptyQueueAsync(string topicId, TopicProcessingState state)
    {
        await state.PauseLock.WaitAsync();
        try
        {
            state.LastEmptyPollTime = DateTime.UtcNow;
            state.IsPaused = true;
        }
        finally
        {
            state.PauseLock.Release();
        }
    }

    private async Task ProcessMessageAsync(IMessageQueueEntry message)
    {
        // TODO: Implement your message processing logic here
        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message Processing Service is stopping...");

       // _shutdownTokenSource.Cancel();

        // Wait for all tasks to complete with timeout
        var allTasks = _runningTasks.SelectMany(x => x.Value);
        var completionTask = Task.WhenAll(allTasks);
        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);

        var completedTask = await Task.WhenAny(completionTask, timeoutTask);
        if (completedTask == timeoutTask)
        {
            _logger.LogWarning("Some tasks did not complete gracefully during shutdown");
        }

        foreach (var state in _topicStates.Values)
        {
            state.PauseLock.Dispose();
        }

        await base.StopAsync(cancellationToken);
    }
}
