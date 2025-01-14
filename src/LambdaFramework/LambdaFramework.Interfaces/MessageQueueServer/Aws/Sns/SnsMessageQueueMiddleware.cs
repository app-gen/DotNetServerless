using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MsgQueue.Server;

public class SnsMessageQueueMiddleware
{
    private readonly RequestDelegate _next;

    public SnsMessageQueueMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/sns/add"))
        {
            // Resolve the service from the scoped container
            var messageQueue = context.RequestServices.GetRequiredService<IMessageQueueServer>();
            var config = context.RequestServices.GetRequiredService<MessageQueueConfig>();

            var message = await CreateMessageFromRequest(context);
            if (message != null)
            {
                await messageQueue.SendMessage(message);
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
            await _next(context);
        }
    }

    private async Task<IMessageQueueEntry> CreateMessageFromRequest(HttpContext context)
    {
        using var reader = new StreamReader(context.Request.Body);
        var jsonString = await reader.ReadToEndAsync();

        if (string.IsNullOrEmpty(jsonString))
        {
            return null;
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var message = JsonSerializer.Deserialize<MessageQueueEntry>(jsonString, jsonOptions);

        if (string.IsNullOrEmpty(message?.TopicId))
        {
            return null;
        }

        // Set default values for required fields if not provided
      //  message.CreatedAt = DateTime.UtcNow;
        message.CreatedTimeUtc = DateTime.UtcNow;

        if (string.IsNullOrEmpty(message.MessageBody))
        {
           // message.MessageBody = message.MsgBody; // Support both property names
        }

        return message;
    }

    
}
