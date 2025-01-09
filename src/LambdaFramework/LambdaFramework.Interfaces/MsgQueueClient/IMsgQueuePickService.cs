using MsgQueue.Server;

namespace MsgQueue.Client;

/// <summary>
/// IMsgQueueClient is responsible for interacting with the message queue, picking, processing, and marking messages.
/// It will call IMessageQueueProcessor for business logic.
/// </summary>
public interface IMsgQueuePickService
{
    
    // Task StartProcessingAsync();
    Task<IMessageQueueEntry?> PickMessageAsync(string topicId);
    Task MarkMessageAsPickedAsync(IMessageQueueEntry message);
    Task MarkAsProcessedAsync(IMessageQueueEntry message);
    Task MarkAsErrorAsync(IMessageQueueEntry message, string errorMessage);
}

