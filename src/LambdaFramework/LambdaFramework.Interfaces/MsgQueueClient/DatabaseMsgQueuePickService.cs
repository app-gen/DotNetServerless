using Microsoft.Extensions.Configuration;

namespace MsgQueue.Client;

/// <summary>
/// Concrete implementation of IMsgQueueClient for database interactions.
/// Responsible for interacting with the database to pick and process messages.
/// </summary>
public class DatabaseMsgQueuePickService : IMsgQueuePickService
{
    private readonly IConfiguration _processor;

    public DatabaseMsgQueuePickService(IConfiguration processor)
    {
        _processor = processor;
    
    }

    
    public async Task<IMsgQueueEntry?> PickMessageAsync(string topicId)
    {
        // Simulate picking a message from the database
        // You can replace this with actual database logic
        var message = await Task.FromResult(new MsgQueueEntry
        {
            MessageId = 1,
            TopicId = topicId,
            MsgBody = "Test Message from Database",
            User = "UserA",
            System = "SystemA",
            CommandName = "TestCommand",
            ActionName = "Run",
            TenantName = "TenantA",
            CommandVersion = "1.0.0"
        });
        await MarkMessageAsPickedAsync(message);
        return message;
    }

    public async Task MarkMessageAsPickedAsync(IMsgQueueEntry message)
    {
        // Simulate marking the message as picked in the database
        message.PickedAt = DateTime.UtcNow;
        message.Status = "Picked";
        await Task.CompletedTask;
    }

    public async Task MarkAsProcessedAsync(IMsgQueueEntry message)
    {
        // Simulate marking the message as processed in the database
        message.Status = "Processed";
        message.ProcessedAt = DateTime.UtcNow;
        await Task.CompletedTask;
    }

    public async Task MarkAsErrorAsync(IMsgQueueEntry message, string errorMessage)
    {
        // Simulate marking the message as error in the database
        message.Status = "Error";
        message.ErrorMessage = errorMessage;
        await Task.CompletedTask;
    }
}
