using MsgQueue.Server;

namespace MsgQueue.Client.Service;

/// <summary>
/// IMessageQueueProcessor processes messages by executing the associated command and marking them as processed or errored.
/// </summary>
public interface IMessageProcessor
{
    Task ProcessMessageAsync(IMessageQueueEntry messageEntry, ITopicConfig config);
    Task ProcessMessageAsync(IMessageQueueEntry messageEntry);

}

