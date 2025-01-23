using Quartz;
using MsgQueue.Server;

namespace Command.Job;

public interface IMessageProcessor : IBackgroundJob, IJob
{
    Task ExecuteAsync(IMessageQueueEntry messageEntry, CancellationToken cancellationToken, JobTopicConfiguration? config, int agentId = 0, IServiceProvider? sp = null);

}
