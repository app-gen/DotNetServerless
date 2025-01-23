using Command.Job;
using LambdaFramework.Common;
using Microsoft.Extensions.DependencyInjection;

namespace DemoPluginJobCommand;

public class DemoPluginJob :AbstractBackgroundJob
{
    public Task Initilize(IServiceCollection collection, ICommandRouter router)
    {
        // Add any dependencies here 
        Console.WriteLine("AbstractBackgroundJob.Init");
        return Task.CompletedTask;
    }

    public virtual Task ExecuteAsync(int agentId, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}

