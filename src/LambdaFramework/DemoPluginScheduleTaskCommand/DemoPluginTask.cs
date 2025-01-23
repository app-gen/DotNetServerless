namespace DemoPluginScheduleTaskCommand;
using Command.Job;
using LambdaFramework.Common;
using Microsoft.Extensions.DependencyInjection;
using MsgQueue.Server;

[Command("Demo", "PluginTask", "1.0.0.0", "")]
public class DemoPluginTask : AbstractScheduleTask, IScheduledTask, IBackgroundJob
{
    public override Task Initilize(IServiceCollection collection, ICommandRouter router)
    {
        this.CommmandRouter = router;
        //todo load 
        Console.WriteLine("DemoPluginTask.Init");

        return Task.CompletedTask;
    }

    public override Task ExecuteAsync(CancellationToken cancellationToken, int agentId = 0, IServiceProvider? sp = null)
    {
        Console.WriteLine("DemoPluginTask.ExecuteAsync");
        return Task.CompletedTask;

    }

}


[Command("Demo", "PluginTask2", "1.0.0.0", "")]
public class DemoPluginTask2 : AbstractScheduleTask, IScheduledTask, IBackgroundJob
{
    public override Task Initilize(IServiceCollection collection, ICommandRouter router)
    {
        this.CommmandRouter = router;
        //todo load 
        Console.WriteLine("DemoPluginTask2.Init");

        return Task.CompletedTask;
    }

    public override Task ExecuteAsync(CancellationToken cancellationToken, int agentId = 0, IServiceProvider? sp = null)
    {
        //throw new NotImplementedException();
        Console.WriteLine("DemoPluginTask2.ExecuteAsync");
        return Task.CompletedTask;

    }

}








[Command("Demo", "PluginTask3", "1.0.0.0", "")]
public class DemoPluginTask3 : AbstractMessageProcessor, IMessageProcessor, IBackgroundJob
{
    public override Task Initilize(IServiceCollection collection, ICommandRouter router)
    {
        this.CommmandRouter = router;
        //todo load 
        Console.WriteLine("DemoPluginTask3.Init");

        return Task.CompletedTask;
    }


    public override Task ExecuteAsync(IMessageQueueEntry messageEntry, 
        CancellationToken cancellationToken, 
        JobTopicConfiguration? config, 
        int agentId = 0, 
        IServiceProvider? sp = null)
    {
        Console.WriteLine("IMessageQueueEntry DemoPluginTask3.ExecuteAsync IMessageQueueEntry");
        return Task.CompletedTask;

    }

}
