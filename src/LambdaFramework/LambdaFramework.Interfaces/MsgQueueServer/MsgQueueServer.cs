using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsgQueue.Server;

public interface IMessageQueueServer
{
//    Task SendMessage(string topicId, string messageId, string messageBody, string user, string system);
    Task SendMessage(IMessageQueueEntry msg);


  //  Task SendMessage(string topicId, string messageId, string messageBody, string cmd, string action, string version, string tenant, string user, string system);


}

public static class MessageQueueServerLib
{
    public static void UseMessageQueueLib(this IApplicationBuilder app, IMessageQueueServer messageQueue)
    {
        // Register the message queue middleware
        app.UseMiddleware<MessageQueueServerMiddleware>(messageQueue);
    }
}


internal class MsgQueueServer
{
}


//### 7. Sample API Call 
//When you call the API endpoint `/api/mq/add/{topicId}/{ messageId}/{ msgbody}/{ user}/{ system}`,
// it will trigger the `SendMessage` method of the `IMessageQueueServer` implementation provided by the client, adding a message to the database.
//ByOverriding this method, we can send it to RabbitMq

public class MessageQueueServerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMessageQueueServer _messageQueue;

    public MessageQueueServerMiddleware(RequestDelegate next, 
        IMessageQueueServer messageQueue)
    {
        _next = next;
        _messageQueue = messageQueue;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/mq/add"))
        {
            var routeValues = context.Request.RouteValues;

            // Extract route parameters
            string topicId = routeValues["topicId"]?.ToString();
            string messageId = routeValues["messageId"]?.ToString();
            string msgbody = routeValues["msgbody"]?.ToString();
            string user = routeValues["user"]?.ToString();
            string system = routeValues["system"]?.ToString();

            if (topicId != null && messageId != null && msgbody != null && user != null && system != null)
            {
                // Call the SendMessage method of the client implementation
                // await _messageQueue.SendMessage(topicId, messageId, msgbody, user, system);
                await _messageQueue.SendMessage(null);
                // Return success response
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("Message added to the queue successfully.");
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid request parameters.");
            }
        }
        else
        {
            // Pass to the next middleware
            await _next(context);
        }
    }
}


public class SqsMsgQueueServer : IMessageQueueServer
{
    //private readonly DbContext _dbContext;

    public SqsMsgQueueServer(
        //DbContext dbContext
        )
    {
        //  _dbContext = dbContext;
    }

    public async Task SendMessage(IMessageQueueEntry msg)
    {
        // Your logic to add to database

        //or we can send to SQS 
    }
}

public class MultiMsgQueueServer : IMessageQueueServer
{
    //private readonly DbContext _dbContext;

    public MultiMsgQueueServer(
        //DbContext dbContext
        )
    {
        
    }

    public async Task SendMessage(IMessageQueueEntry msg)
    {
        // Your logic to add to database
        //  _dbContext = dbContext;
        //outbox pattern
        //sqs is up send to sqs otherwise send to db
        //or we can send to SQS 
    }
}


public class DatabaseMsgQueueServer : IMessageQueueServer
{
    //private readonly DbContext _dbContext;

    public DatabaseMsgQueueServer(
        //DbContext dbContext
        )
    {
      //  _dbContext = dbContext;
    }

    public async Task SendMessage(IMessageQueueEntry msg)
    {
        // Your logic to add to database

        //or we can send to SQS 
    }

    //Task IMessageQueueServer.SendMessage(IMessageQueueEntry msg)
    //{
    //    throw new NotImplementedException();
    //}
}

internal class MessageQueueEntry : IMessageQueueEntry
{

    public int MessageId { get; set; } //Pk of the table 
    public string TopicId { get; set; }
    public string MsgBody { get; set; }
    public string? User { get; set; }

    public int? Retry { get; set; }

    public int? MaxRetry { get; set; }

    public int? MessagePriority { get; set; }
    public int? MessageQueueProcessId { get; set; }
    public long? ThreadId { get; set; }
    public string? MachineName { get; set; }

    public DateTime? PickupTime { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? ProcessTimeInMilliSeconds { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? UtcExecutionTime { get; set; }
    public DateTime? CreatedDate { get; set; }


    public string? System { get; set; }
    public string? CommandName { get; set; }
    public string? ActionName { get; set; }
    public string? TenantName { get; set; }
    public string? CommandVersion { get; set; }
    public DateTime PickedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Status { get; set; }

    [NotMapped]
    public object? Context { get; set; }

    [NotMapped]
    public object? ExtraData { get; set; }
    public DateTime CreatedAt { get; set; }
    public string MessageBody { get; set; }
}