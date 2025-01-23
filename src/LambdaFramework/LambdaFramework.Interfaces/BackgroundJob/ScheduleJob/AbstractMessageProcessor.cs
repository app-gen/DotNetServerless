using Microsoft.Extensions.DependencyInjection;
using LambdaFramework.Common;
using Quartz;
using MsgQueue.Server;

namespace Command.Job;

[Command("Abstract", "MQ", "1.0.0.0", "")]
public abstract class AbstractMessageProcessor : AbstractCommand, IMessageProcessor, ICommand, IJob
{

    public string JobId { get; set; }

    public string TaskId { get; set; }
    public ICommandRouter CommmandRouter { get; set; }
    public string AgentId { get; set; }

    public Task Execute(IJobExecutionContext context)
    {
        throw new NotImplementedException();
    }

    public virtual Task ExecuteAsync(CancellationToken cancellationToken, int agentId = 0, IServiceProvider? sp = null)
    {
        //throw new NotImplementedException();

        Console.WriteLine("AbstractScheduleTask.ExecuteAsync");
        return Task.CompletedTask;


    }

    public virtual Task ExecuteAsync(IMessageQueueEntry messageEntry, CancellationToken cancellationToken, JobTopicConfiguration? config,  int agentId = 0, IServiceProvider? sp = null)
    {
        throw new NotImplementedException();
    }

    public virtual Task Initilize(IServiceCollection collection, ICommandRouter router)
    {
        this.CommmandRouter = router;
        //todo load 
        Console.WriteLine("AbstractBackgroundJob.Init");
        return Task.CompletedTask;
    }



}
