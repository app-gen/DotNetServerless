using Microsoft.Extensions.Configuration;
using MsgQueue.Server;

namespace PubSub.EventBroker;

//C:\mygitrepo\DotNetServerless\src\LambdaFramework\LambdaFramework.Interfaces\PubSubEventBroker

//Topic  Id 1/MsgTypeId -Notes Add 
//Notes Add 
//(V1,v2) 
//(TenantA/TenantB)




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

    
    public async Task<IMessageQueueEntry?> PickMessageAsync(string topicId)
    {
        // Simulate picking a message from the database
        // or pick from SQS or RabbitMq
        // You can replace this with actual database logic
        var message = await Task.FromResult(new MessageQueueEntry
        {
            MessageId = 1,
            TopicId = topicId,
            //MsgBody = "Test Message from Database",
            User = "UserA",
            System = "SystemA",
            CommandName = "Notes", //DB will get priority fallback to local config
            ActionName = "Add",
            TenantName = "TenantA",
            CommandVersion = "1.0.0.0" 
        });

        await MarkMessageAsPickedAsync(message);
        return message;
    }

    public async Task MarkMessageAsPickedAsync(IMessageQueueEntry message)
    {
        // Simulate marking the message as picked in the database
       // message.PickedAt = DateTime.UtcNow;
        message.Status = "Picked";
        await Task.CompletedTask;
    }

    public async Task MarkAsProcessedAsync(IMessageQueueEntry message)
    {
        // Simulate marking the message as processed in the database
        message.Status = "Processed";
      //  message.ProcessedAt = DateTime.UtcNow;
        await Task.CompletedTask;
    }

    public async Task MarkAsErrorAsync(IMessageQueueEntry message, string errorMessage)
    {
        // Simulate marking the message as error in the database
        message.Status = "Error";
        message.ErrorMessage = errorMessage;
        await Task.CompletedTask;
    }
}
