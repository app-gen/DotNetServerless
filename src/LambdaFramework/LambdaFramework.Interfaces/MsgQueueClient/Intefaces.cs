namespace MsgQueue.Client;

/// <summary>
/// IMessageQueue represents the interface for sending messages to the queue.
/// Clients will provide their own implementation for this interface.
/// </summary>
public interface IMessageQueueSystem
{
    Task SendMessageAsync(IMsgQueueEntry messageEntry);
}

