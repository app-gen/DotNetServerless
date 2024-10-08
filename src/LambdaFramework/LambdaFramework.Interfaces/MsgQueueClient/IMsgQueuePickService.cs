namespace MsgQueue.Client;

/// <summary>
/// IMsgQueueClient is responsible for interacting with the message queue, picking, processing, and marking messages.
/// It will call IMessageQueueProcessor for business logic.
/// </summary>
public interface IMsgQueuePickService
{
    
    // Task StartProcessingAsync();
    Task<IMsgQueueEntry?> PickMessageAsync(string topicId);
    Task MarkMessageAsPickedAsync(IMsgQueueEntry message);
    Task MarkAsProcessedAsync(IMsgQueueEntry message);
    Task MarkAsErrorAsync(IMsgQueueEntry message, string errorMessage);
}

