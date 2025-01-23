using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class MyBackgroundTask : IHostedService
{
    private readonly ILogger<MyBackgroundTask> _logger;
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly int _instanceId;

    public MyBackgroundTask(ILogger<MyBackgroundTask> logger, int instanceId = 0)
    {
        _logger = logger;
        _instanceId = instanceId;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background task {InstanceId} is starting.", _instanceId);
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        // Start the background task in a separate thread
        _ = Task.Run(() => RunBackgroundTaskLoopAsync(_cancellationTokenSource.Token), cancellationToken);

        return Task.CompletedTask;
    }

    private async Task RunBackgroundTaskLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // Run the actual task logic
                await RunTaskLogicAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Background task {InstanceId} was canceled.", _instanceId);
                break;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred in background task {InstanceId}. Restarting in 5 seconds...", _instanceId);

                // Wait before restarting the task
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Cancellation requested while waiting to restart background task {InstanceId}.", _instanceId);
                    break;
                }
            }
        }

        _logger.LogInformation("Background task loop {InstanceId} is stopping.", _instanceId);
    }

    private async Task RunTaskLogicAsync(CancellationToken cancellationToken)
    {
        // Simulate long-running background work
        _logger.LogInformation("Background task {InstanceId} is running at: {Time}", _instanceId, DateTimeOffset.Now);

        // Simulate some work (e.g., fetch data, process, etc.)
        await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

        // Simulate an exception for testing purposes
        throw new InvalidOperationException($"Simulated exception in task logic for instance {_instanceId}.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background task {InstanceId} is stopping.", _instanceId);
        _cancellationTokenSource?.Cancel();

        // Optionally, wait for any cleanup or ongoing tasks to complete
        await Task.Delay(500, cancellationToken); // Short delay for graceful shutdown
    }
}
