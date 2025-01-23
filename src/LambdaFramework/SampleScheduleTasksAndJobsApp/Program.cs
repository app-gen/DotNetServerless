namespace SampleScheduleTasksAndJobsApp;

using Microsoft.AspNetCore.Builder;
using MsgQueue.Server;
using Command.Job;
using System.Windows.Input;
using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static async Task Main(string[] args)
    {

     Console.WriteLine("Start");
        //await Sample.Run(args);

    IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {

        // Configure and register background job library with custom options
        services.AddCommandBackgroundJobLib(options =>
        {
            options.PluginDirectoty = "plugins"; // Example custom value
            options.EnableLogging = true;              // Example custom value
        });

        // Register the JobConfigProviderFromAppSettings
        services.AddSingleton<IJobConfigurationProvider, JobConfigProviderFromAppSettings>();

        // Add other services or hosted services here
    })
    .Build();

     await host.RunAsync();



    }
}

