using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
//using MsgQueueClientLibrary.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MsgQueue.Client;

public static class MsgQueueClientLibExtensions
    {
        /// <summary>
        /// Registers and starts the MsgQueue background job in an ASP.NET Core application.
        /// </summary>
        public static IApplicationBuilder UseMsgQueueClient(this IApplicationBuilder app)
        {
            // Ensures that the hosted service (background service) is initialized
            var hostApplicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
           // var msgQueueClient = app.ApplicationServices.GetRequiredService<IMsgQueueClient>();

            // You can optionally add more configuration or logging
            return app;
        }

        /// <summary>
        /// Registers the necessary services to set up MsgQueueClient processing.
        /// </summary>
        public static IServiceCollection AddMsgQueueClient(this IServiceCollection services)
        {
            // Register the background service and its dependencies
           // services.AddScoped<IMsgQueueClient, MsgQueueClient>();
            //services.AddScoped<IMsgQueueProcessor, MsgQueueProcessor>();
            //services.AddScoped<IMsgQueueJobManager, MsgQueueJobManager>();

            // Register background processing service
            services.AddHostedService<MsgQueueBackgroundService>();

            return services;
        }
    }
