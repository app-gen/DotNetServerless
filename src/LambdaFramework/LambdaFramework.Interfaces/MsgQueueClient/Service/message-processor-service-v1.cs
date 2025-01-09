using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MsgQueue.Client;
using System.Collections.Concurrent;
using MsgQueue.Server;

namespace MsgQueue.Service;
/*
public class MessageProcessingConfig1
{
    public int NumberOfThreads { get; set; } = 1;
    public string TopicId { get; set; } = string.Empty;
    public int ErrorDelaySeconds { get; set; } = 5;
}

public class MessageProcessingService1 : BackgroundService
{
    private readonly ILogger<MessageProcessingService> _logger;
    private readonly IMsgQueuePickService _queueService;
    private readonly MessageProcessingConfig1     _config;
    private readonly ConcurrentDictionary<int, Task> _runningTasks;
    private readonly CancellationTokenSource _shutdownTokenSource;

    public MessageProcessingService1(
        ILogger<MessageProcessingService> logger,
        IMsgQueuePickService queueService,
        IConfiguration configuration)
    {
        _logger = logger;
        _queueService = queueService;
        _config = new MessageProcessingConfig1();
        configuration.GetSection("MessageProcessing").Bind(_config);
        
        _runningTasks = new ConcurrentDictionary<int, Task>();
        _shutdownTokenSource = new CancellationTokenSource();

        ValidateConfiguration();
    }

    private void ValidateConfiguration()
    {
        if (_config.NumberOfThreads < 1)
        {
            throw new InvalidOperationException("NumberOfThreads must be at least 1");
        }

        if (string.IsNullOrEmpty(_config.TopicId))
        {
            throw new InvalidOperationException("TopicId must be configured");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Message Processing Service is starting...");

        try
        {
            // Start the processing tasks
            for (int i = 0; i < _config.NumberOfThreads; i++)
            {
                var taskId = i;
                var task = ProcessMessagesAsync(taskId, stoppingToken);
                _runningTasks.TryAdd(taskId, task);
            }

            // Wait for all tasks to complete
            await Task.WhenAll(_runningTasks.Values);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the message processing service");
            throw;
        }
    }

    private async Task ProcessMessagesAsync(int taskId, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting processing task {TaskId}", taskId);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Pick a message from the queue
                var message = await _queueService.PickMessageAsync(_config.TopicId);

                if (message == null)
                {
                    continue;
                }

                _logger.LogInformation("Task {TaskId} picked message {MessageId}", taskId, message.MessageId);

                // Mark the message as picked
                await _queueService.MarkMessageAsPickedAsync(message);

                try
                {
                    // Process the message
                    await ProcessMessageAsync(message);

                    // Mark the message as processed
                    await _queueService.MarkAsProcessedAsync(message);
                    
                    _logger.LogInformation("Task {TaskId} processed message {MessageId}", taskId, message.MessageId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Task {TaskId} failed to process message {MessageId}", taskId, message.MessageId);
                    await _queueService.MarkAsErrorAsync(message, ex.Message);
                    
                    // Wait before processing next message after error
                    await Task.Delay(TimeSpan.FromSeconds(_config.ErrorDelaySeconds), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Task {TaskId} encountered an error while picking message", taskId);
                await Task.Delay(TimeSpan.FromSeconds(_config.ErrorDelaySeconds), stoppingToken);
            }
        }

        _logger.LogInformation("Processing task {TaskId} is shutting down", taskId);
    }

    private async Task ProcessMessageAsync(IMessageQueueEntry message)
    {
        // TODO: Implement your message processing logic here
        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message Processing Service is stopping...");

        // Trigger cancellation
        _shutdownTokenSource.Cancel();

        // Wait for all tasks to complete with timeout
        var completionTask = Task.WhenAll(_runningTasks.Values);
        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);

        var completedTask = await Task.WhenAny(completionTask, timeoutTask);
        if (completedTask == timeoutTask)
        {
            _logger.LogWarning("Some tasks did not complete gracefully during shutdown");
        }

        await base.StopAsync(cancellationToken);
    }
}
*/