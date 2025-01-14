using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;






namespace MsgQueue.Server;

public static class SnsMessageQueueServiceExtension
{
   

    public static IServiceCollection AddSnsMessageQueue(
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
