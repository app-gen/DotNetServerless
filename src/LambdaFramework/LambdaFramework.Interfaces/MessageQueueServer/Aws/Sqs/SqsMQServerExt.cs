using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;






namespace MsgQueue.Server;

// QueueProviderType.cs
// namespace MsgQueue.Server.Providers;

public enum QueueProviderType
{
    Database,
    Sqs,
    Sns,
   // RabbitMq,
  //  ZeroMq
}

// MessageQueueConfig.cs
//namespace MsgQueue.Server.Configuration;

public class MessageQueueConfig
{
    public QueueProviderType CurrentQueueProvider { get; set; }
 
    public string ConnectionString { get; set; }
    public Dictionary<string, string> Settings { get; set; } = new();
}

// IQueueProvider.cs
//namespace MsgQueue.Server.Providers;

public interface IQueueProvider
{
    QueueProviderType ProviderType { get; }
    Task SendMessageAsync(IMessageQueueEntry message);
    Task<bool> IsHealthyAsync();
}

// Concrete Provider Implementations
//namespace MsgQueue.Server.Providers;

public class DatabaseQueueProvider : IQueueProvider
{
    private readonly string _connectionString;

    public DatabaseQueueProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public QueueProviderType ProviderType => QueueProviderType.Database;

    public async Task SendMessageAsync(IMessageQueueEntry message)
    {
        // Implementation for database storage
    }

    public async Task<bool> IsHealthyAsync()
    {
        // Check database connectivity
        return true;
    }
}

public class MessageQueueMiddleware
{
    private readonly RequestDelegate _next;

    public MessageQueueMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/mq/add"))
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

public static class MessageQueueServiceExtensions1
{
   

    public static IServiceCollection AddSqsMessageQueue(
        this IServiceCollection services,
        Action<MessageQueueConfig> configAction)
    {
        var config = new MessageQueueConfig();
        configAction(config);

        services.AddSingleton(config);
        //services.AddScoped<IQueueProvider, SqsQueueProvider>();




        // Register SQS provider as scoped
        services.AddScoped<IQueueProvider>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<SnsQueueProvider>>();
            return new SqsQueueProvider(config.Settings, logger);
        });

        // Register message queue server as scoped
        services.AddScoped<IMessageQueueServer>(sp =>
        {
            var queueProvider = sp.GetRequiredService<IQueueProvider>();
            return new CommonMessageQueueServer(config, queueProvider);
        });

        return services;



      
    }

    public static IApplicationBuilder UseMessageQueueServer(
      this IApplicationBuilder app)
    {
        return app.UseMiddleware<MessageQueueMiddleware>();
    }

}
