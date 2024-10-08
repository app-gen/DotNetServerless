using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MsgQueue.Server;

public interface IMessageQueueServer
{
    Task SendMessage(string topicId, string messageId, string messageBody, string user, string system);
    Task SendMessage(IMessageQueueEntry msg);


    Task SendMessage(string topicId, string messageId, string messageBody, string cmd, string action, string version, string tenant, string user, string system);


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
                await _messageQueue.SendMessage(topicId, messageId, msgbody, user, system);

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



public class SampleDatabaseMsgQueueServer : IMessageQueueServer
{
    //private readonly DbContext _dbContext;

    public SampleDatabaseMsgQueueServer(
        //DbContext dbContext
        )
    {
      //  _dbContext = dbContext;
    }

    public async Task SendMessage(string topicId, string messageId, string messageBody, string cmd, string action, string version, string tenant, string user, string system)
    {
        var messageQueueEntry = new MessageQueueEntry
        {
            TopicId = topicId,
            MessageId = messageId,
            MessageBody = messageBody,
            User = user,
            System = system,
            CreatedAt = DateTime.UtcNow
        };

        //_dbContext.MessageQueue.Add(messageQueueEntry);
        //await _dbContext.SaveChangesAsync();
    }

    public async Task SendMessage(string topicId, string messageId, string messageBody, string user, string system)
    {
        // Example of inserting into a database
        var messageQueueEntry = new MessageQueueEntry
        {
            TopicId = topicId,
            MessageId = messageId,
            MessageBody = messageBody,
            User = user,
            System = system,
            CreatedAt = DateTime.UtcNow
        };

        //_dbContext.MessageQueue.Add(messageQueueEntry);
        //await _dbContext.SaveChangesAsync();
    }

    public async Task SendMessage(IMessageQueueEntry msg)
    {
        var messageQueueEntry = new MessageQueueEntry
        {
        };

    }

    //Task IMessageQueueServer.SendMessage(IMessageQueueEntry msg)
    //{
    //    throw new NotImplementedException();
    //}
}

internal class MessageQueueEntry : IMessageQueueEntry
{
    public string TopicId { get; set; }
    public string MessageId { get; set; }
    public string MessageBody { get; set; }
    public string User { get; set; }
    public string System { get; set; }
    public DateTime CreatedAt { get; set; }
}