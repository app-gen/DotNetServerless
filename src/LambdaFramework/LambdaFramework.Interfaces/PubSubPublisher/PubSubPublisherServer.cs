using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MsgQueue.Server;
using System.ComponentModel.DataAnnotations.Schema;


namespace PubSub.Publisher;

public interface IPublisherServer
{
//    Task SendMessage(string topicId, string messageId, string messageBody, string user, string system);
    Task SendMessage(IMessageQueueEntry msg);


  //  Task SendMessage(string topicId, string messageId, string messageBody, string cmd, string action, string version, string tenant, string user, string system);


}

public static class PubSubPublisherServerLib
{
    public static void UsePubSubPublisherLib(this IApplicationBuilder app, IPublisherServer messageQueue)
    {
        // Register the message queue middleware
        app.UseMiddleware<PubSubPublisherServerMiddleware>(messageQueue);
    }
}


internal class PubSubPublisherServerServer
{
}


//### 7. Sample API Call 
//When you call the API endpoint `/api/mq/add/{topicId}/{ messageId}/{ msgbody}/{ user}/{ system}`,
// it will trigger the `SendMessage` method of the `IPublisherServer` implementation provided by the client, adding a message to the database.
//ByOverriding this method, we can send it to RabbitMq

public class PubSubPublisherServerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IPublisherServer _messageQueue;

    public PubSubPublisherServerMiddleware(RequestDelegate next, 
        IPublisherServer messageQueue)
    {
        _next = next;
        _messageQueue = messageQueue;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/pubsub/publish"))
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
        else if (context.Request.Path.StartsWithSegments("/api/pubsub/subscribe"))
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


public class KafkaMsgQueueServer : IPublisherServer
{
    //private readonly DbContext _dbContext;

    public KafkaMsgQueueServer(
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

public class MultiMsgQueueServer : IPublisherServer
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


public class DatabasePubSubPublisherServer : IPublisherServer
{
    //private readonly DbContext _dbContext;

    public DatabasePubSubPublisherServer(
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

    //Task IPublisherServer.SendMessage(IMQEntry msg)
    //{
    //    throw new NotImplementedException();
    //}
}
