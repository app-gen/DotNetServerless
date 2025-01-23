using Microsoft.Extensions.DependencyInjection;
using LambdaFramework.Common;
using Quartz;

namespace Command.Job;

public interface IScheduledTask: IBackgroundJob, IJob
    {
        
      //  Task ExecuteAsync(IServiceProvider serviceProvider, 
        //    ICommandRouter router , CancellationToken cancellationToken);
    }

public interface IMessageTask : IBackgroundJob, IJob
{

    //  Task ExecuteAsync(IServiceProvider serviceProvider, 
    //    ICommandRouter router , CancellationToken cancellationToken);
}


[Command("Abstract", "ScheduleTask", "1.0.0.0", "")]
public abstract class AbstractScheduleTask : AbstractCommand, IScheduledTask, ICommand, IJob
{

    public string JobId { get; set; }

    public string TaskId { get; set; }
    public ICommandRouter CommmandRouter { get;  set; }
    public string AgentId { get; set; }

    public Task Execute(IJobExecutionContext context)
    {
        throw new NotImplementedException();
    }

    public virtual Task ExecuteAsync(CancellationToken cancellationToken, int agentId=0, IServiceProvider? sp=null)
    {
        //throw new NotImplementedException();

        Console.WriteLine("AbstractScheduleTask.ExecuteAsync");
        return Task.CompletedTask;


    }

    public virtual Task Initilize(IServiceCollection collection, ICommandRouter router)
    {
        this.CommmandRouter = router;
        //todo load 
        Console.WriteLine("AbstractBackgroundJob.Init");
        return Task.CompletedTask;
    }
    
    

}
