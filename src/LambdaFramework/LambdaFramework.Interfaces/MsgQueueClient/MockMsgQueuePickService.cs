using Microsoft.Extensions.Configuration;
using MsgQueue.Client.Service;
using MsgQueue.Server;

namespace MsgQueue.Client;

/// <summary>
/// Mock implementation of IMsgQueueClient for testing or development purposes.
/// </summary>
public class MockMsgQueuePickService : IMsgQueuePickService
{
    private readonly IConfiguration _processor;
    private readonly ITopicConfig _topicConfig;

    public MockMsgQueuePickService(IConfiguration processor, ITopicConfig topicConfig)
    {
        _processor = processor;
        _topicConfig = topicConfig;
    }

    public async Task<IMessageQueueEntry?> PickMessageAsync(string topicId)
    {
        // Simulate picking a message (for testing purposes)
        var message = await Task.FromResult(new MessageQueueEntry
        {
            MessageId = 2,
            TopicId = topicId,
            MessageBody = "Test Message from Mock",
            User = "TestUser",
            System = "TestSystem",
            CommandName = "MockCommand",
            ActionName = "Execute",
            TenantName = "TestTenant",
            CommandVersion = "1.0.0"
        });
        await MarkMessageAsPickedAsync(message);
        return message;
    }

    public async Task MarkMessageAsPickedAsync(IMessageQueueEntry message)
    {
        // Simulate marking the message as picked in mock mode
        message.PickupTimeUtc = DateTime.UtcNow;
        message.Status = "Picked";
        await Task.CompletedTask;
    }

    public async Task MarkAsProcessedAsync(IMessageQueueEntry message)
    {
        // Simulate marking the message as processed in mock mode
        message.Status = "Processed";
        message.ProcessedTimeUtc = DateTime.UtcNow;
        await Task.CompletedTask;
    }

    public async Task MarkAsErrorAsync(IMessageQueueEntry message, string errorMessage)
    {
        // Simulate marking the message as error in mock mode
        message.Status = "Error";
        message.ErrorMessage = errorMessage;
        await Task.CompletedTask;
    }
}
