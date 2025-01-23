namespace SampleScheduleTasksAndJobsApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Sample
{
    public static async Task Run(string[] args)
    {

        var hostb = Host.CreateDefaultBuilder(args)
    .UseWindowsService() // Optional for Windows service
    .ConfigureServices(services =>
    {
        // Register multiple instances of MyBackgroundTask
        for (int i = 1; i <= 5; i++) // 5 instances
        {
            int instanceId = i; // Capture the loop variable
            services.AddSingleton<IHostedService>(sp =>
                new MyBackgroundTask(sp.GetRequiredService<ILogger<MyBackgroundTask>>(), instanceId));
        }
    });

        hostb.ConfigureServices(services =>
        {
            // Register multiple instances of MyBackgroundTask
            for (int i = 11; i <= 15; i++) // 5 instances
            {
                int instanceId = i; // Capture the loop variable
                services.AddSingleton<IHostedService>(sp =>
                    new MyBackgroundTask(sp.GetRequiredService<ILogger<MyBackgroundTask>>(), instanceId));
            }
        });

        var host = hostb.Build();



        await host.RunAsync();


    }
}

