using MsgQueue.Server;

namespace PubSub.EventBroker;

/// <summary>
/// IMessageQueueProcessor processes messages by executing the associated command and marking them as processed or errored.
/// </summary>
public interface IMessageQueueProcessor
{
    Task ProcessMessageAsync(IMessageQueueEntry messageEntry, TopicConfig config);
}

