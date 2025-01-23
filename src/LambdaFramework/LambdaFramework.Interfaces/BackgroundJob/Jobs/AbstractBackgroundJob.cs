using Microsoft.Extensions.DependencyInjection;
using LambdaFramework.Common;
using Quartz;
namespace Command.Job;


public abstract class AbstractBackgroundJob : AbstractCommand, IBackgroundJob, ICommand, IJob
{
    public string JobId { get; set; }

    public string AgentId { get; set; }

    public Task Initilize(IServiceCollection collection, ICommandRouter router)
    {
        // Add any dependencies here 
        Console.WriteLine("AbstractBackgroundJob.Init");
        return Task.CompletedTask;
    }


    public virtual Task Execute(IJobExecutionContext context)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAsync(CancellationToken cancellationToken, int agentId, IServiceProvider sp)
    {
        Console.WriteLine("AbstractBackgroundJob.AbstractBackgroundJob");
        return Task.CompletedTask;
        
    }
} 