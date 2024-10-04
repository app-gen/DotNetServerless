using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace LambdaFramework.Common;

public class PluginRefreshService : BackgroundService
{
    private readonly PluginLoader _pluginLoader;
    private readonly ILogger<PluginRefreshService> _logger;
    private readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(30);

    public PluginRefreshService(PluginLoader pluginLoader, ILogger<PluginRefreshService> logger)
    {
        _pluginLoader = pluginLoader;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Refreshing plugins");
            _pluginLoader.LoadPlugins();
            await Task.Delay(_refreshInterval, stoppingToken);
        }
    }
}



