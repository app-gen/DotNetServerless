using LambdaFramework.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MsgQueue.Client;


namespace Command.Job;

public class JobOptions
{
    public string PluginDirectoty { get; set; } = "plugins";
    public bool EnableLogging { get; set; } = true; // Example of a configurable option
}


public static class JobLibExtensions
{
    public static IServiceCollection AddCommandBackgroundJobLib(this IServiceCollection services,
        Action<JobOptions> configureOptions)
    {
        // Configure and register the options in DI
        services.Configure(configureOptions);

        // Register core services
        services.AddSingleton<ICommandRouter, CommandRouter>();
        //this should from the database
        services.AddSingleton<IJobConfigurationProvider, JobConfigProviderFromAppSettings>();


        services.AddSingleton<IMsgQueuePickService, SqsMsgQueuePickService>();

        // Register JobManager as a hosted service
        services.AddHostedService<JobManager>();


        return services;
    }

    public static IApplicationBuilder UseCommandBackgroundJobLib(this IApplicationBuilder app
        //, ICommandRouter router, IExecutionContext context
        //
        )
    {
        var options = app.ApplicationServices.GetRequiredService<IOptions<JobOptions>>().Value;

        var router = app.ApplicationServices.GetRequiredService<ICommandRouter>();



        return app;
    }


    public static IHostBuilder UseCommandBackgroundJobLib(this IHostBuilder app
    //, ICommandRouter router, IExecutionContext context
    //
    )
    {
        //var options = app.UseDefaultServiceProvider.GetRequiredService<IOptions<JobOptions>>().Value;

        //var router = app.ApplicationServices.GetRequiredService<ICommandRouter>();



        return app;
    }


}

