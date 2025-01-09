/*
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MsgQueue.Client;
using MsgQueue.Service;
using NLog;
using NLog.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

public class Program
{
    public static async Task Main(string[] args)
    {
        var logger = LogManager.GetCurrentClassLogger();
        try
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped program because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureLogging((hostContext, logging) =>
            {
                logging.ClearProviders();
                logging.AddNLog(new NLogProviderOptions
                {
                    RemoveLoggerFactoryFilter = true
                });
            })
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;

                // Configure OpenTelemetry
                services.AddOpenTelemetry()
                    .ConfigureResource(resource => resource
                        .AddService("MessageProcessingService"))
                    .WithTracing(tracing => tracing
                        .AddSource("MessageProcessingService")
                        .SetSampler(new AlwaysOnSampler())
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri(configuration["Telemetry:OtlpEndpoint"] ?? "http://localhost:4317");
                        }))
                    .WithMetrics(metrics => metrics
                        .AddMeter("MessageProcessingService")
                        .AddRuntimeInstrumentation()
                        .AddProcessInstrumentation()
                        .AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri(configuration["Telemetry:OtlpEndpoint"] ?? "http://localhost:4317");
                        }));

                // Register services
                services.AddHostedService<MessageProcessingService>();
                services.AddSingleton<IMsgQueuePickService, SqsMsgQueuePickService>();
                services.AddSingleton<TelemetryService>();

                // Add health checks
                services.AddHealthChecks()
                    .AddCheck<MessageProcessingHealthCheck>("MessageProcessing");
            });
}
*/