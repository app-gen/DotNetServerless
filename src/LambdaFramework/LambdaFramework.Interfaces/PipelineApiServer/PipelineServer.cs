using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pipeline.Server;

/*
 *  Pipeline Api will can run a sequenqial workflow/process flow or set of commands one by one.
 *  The commands can run in foreground (realtime/blocking) 
 *  or can be put on a meesage queue for background processing.
 *  it can accept one command or set of commands 
 *  The output of first command will be passed to the next command (as a pipe operator |)
 *  Example - Suppose you have three commands 
 *  1. ToUpperCase()
 *  2. Trim()
 *  3. RemoveSpecalCharacters()
 *  4. Sanitize()
 *  
 *  Now we call it like 
 *   
 *  inputdate > op  ToUpperCase > Trim > RemoveSpecalCharacters >Sanitize
 *  (this is similat to fluent api where the output of first function becomes the input of second
 *  This is based on command patterns and supports command, action, version and tenant, 
 *  All the hooks associated will be also run
 *  
 */

public interface IPipelineServer
{
//    Task SendMessage(string topicId, string messageId, string messageBody, string user, string system);
   // Task SendMessage(IPipelineEntry msg);


  //  Task SendMessage(string topicId, string messageId, string messageBody, string cmd, string action, string version, string tenant, string user, string system);


}

public static class PipelineServerLib
{
    public static void UsePipelineLib(this IApplicationBuilder app, IPipelineServer Pipeline)
    {
        // Register the message queue middleware
        app.UseMiddleware<PipelineServerMiddleware>(Pipeline);
    }
}


internal class MsgQueueServer
{
}



public class PipelineServerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IPipelineServer _Pipeline;

    public PipelineServerMiddleware(RequestDelegate next, 
        IPipelineServer Pipeline)
    {
        _next = next;
        _Pipeline = Pipeline;
    }

    public async Task Invoke(HttpContext context)
    {

        if (context.Request.Path.StartsWithSegments("/api/pipeline/realtime/{tenant}/{command_list_json}"))
        {
            var routeValues = context.Request.RouteValues;

            // Extract route parameters
            string tenant = routeValues["tenant"]?.ToString();
            string command_list_json = routeValues["command_list_json"]?.ToString();


            if (tenant != null && command_list_json != null)
            {
                // real time 
                // background 

                // Call the SendMessage method of the client implementation
                // await _Pipeline.SendMessage(topicId, messageId, msgbody, user, system);
                //await _Pipeline.SendMessage(null);
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
        else if (context.Request.Path.StartsWithSegments("/api/pipeline/background"))
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
                // real time 
                // background 

                // Call the SendMessage method of the client implementation
                // await _Pipeline.SendMessage(topicId, messageId, msgbody, user, system);
                //await _Pipeline.SendMessage(null);
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


public class SqsMsgQueueServer : IPipelineServer
{
    //private readonly DbContext _dbContext;

    public SqsMsgQueueServer(
        //DbContext dbContext
        )
    {
        //  _dbContext = dbContext;
    }

    //public async Task SendMessage(IPipelineEntry msg)
    //{
    //    // Your logic to add to database

    //    //or we can send to SQS 
    //}
}

public class MultiMsgQueueServer : IPipelineServer
{
    //private readonly DbContext _dbContext;

    public MultiMsgQueueServer(
        //DbContext dbContext
        )
    {
        
    }

    //public async Task SendMessage(IPipelineEntry msg)
    //{
    //    // Your logic to add to database
    //    //  _dbContext = dbContext;
    //    //outbox pattern
    //    //sqs is up send to sqs otherwise send to db
    //    //or we can send to SQS 
    //}
}

