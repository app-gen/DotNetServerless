using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;



using Microsoft.AspNetCore.Routing;
using System.Text.Json;


namespace LambdaFramework.Common;


public class CommandApiOptions
    {
        public string PluginDirectoty { get; set; } = "/api/commands";

        public string GetEndpoint { get; set; } = "/api/execute/";
        public string PostEndpoint { get; set; } = "/api/execute/";
        public bool EnableLogging { get; set; } = true; // Example of a configurable option
}


    public static class CommandApiExtensions
    {
        public static IServiceCollection AddCommandApiLib(this IServiceCollection services, Action<CommandApiOptions> configureOptions)
        {
            // Configure and register the options in DI
            services.Configure(configureOptions);

            services.AddSingleton<ICommandRouter, CommandRouter>();

            services.AddSingleton<IExecutionContext, ExecutionContext>();


            return services;
        }

        public static IApplicationBuilder UseCommandApiLib(this IApplicationBuilder app, ICommandRouter router, IExecutionContext context)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<CommandApiOptions>>().Value;

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {

                //// Use the configured endpoint paths
                //endpoints.MapGet(options.GetEndpoint, async context =>
                //{
                //    if (options.EnableLogging)
                //    {
                //        // Log or handle the request accordingly
                //        Console.WriteLine("Handling GET request for commands...");
                //    }

                //    await context.Response.WriteAsync("This is the Command API Library GET response");
                //});

                //endpoints.MapPost(options.PostEndpoint, async context =>
                //{
                //    await context.Response.WriteAsync("Command received and processed.");
                //});



            endpoints.MapPost("/api/execute/{commandName}/{actionName}/{version?}/{tenantId?}/{inputFormat?}/{outputFormat?}", async context =>
            {
                var tenantId = context.Request.RouteValues["tenantId"] as string ?? string.Empty;
                var commandName = context.Request.RouteValues["commandName"] as string ?? string.Empty;
                var actionName = context.Request.RouteValues["actionName"] as string ?? string.Empty;
                var version = context.Request.RouteValues["version"] as string ?? "1.0.0.0";

                if (tenantId?.ToLower() == "any" || tenantId?.ToLower() == "all")
                {
                    tenantId = string.Empty;
                }

                using var reader = new System.IO.StreamReader(context.Request.Body);
                var parameters = await reader.ReadToEndAsync();

                try
                {
                    var result = await router.ExecuteCommand(tenantId, commandName, actionName, version, parameters);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                }
                catch (KeyNotFoundException ex)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "An internal error occurred." }));
                }
            });

             endpoints.MapGet("/api/execute/{commandName}/{actionName}/{tenantId?}/{version?}/{outputFormat?}/{param1?}/{param2?}/{param3?}/{param4?}/{param5?}/{param6?}", async context =>
                {
                    var tenantId = context.Request.RouteValues["tenantId"] as string ?? string.Empty;
                    var commandName = context.Request.RouteValues["commandName"] as string ?? string.Empty;
                    var actionName = context.Request.RouteValues["actionName"] as string ?? string.Empty;
                    var version = context.Request.RouteValues["version"] as string ?? "1.0.0.0";
                    var param1 = context.Request.RouteValues["param1"] as string ?? "";
                    var param2 = context.Request.RouteValues["param2"] as string ?? "";
                    var param3 = context.Request.RouteValues["param3"] as string ?? "";
                    var param4 = context.Request.RouteValues["param4"] as string ?? "";
                    var param5 = context.Request.RouteValues["param5"] as string ?? "";
                    var param6 = context.Request.RouteValues["param5"] as string ?? "";



                    Dictionary<string,string> dict = GetFromRoutes(context.Request.RouteValues);

                    if (tenantId?.ToLower() == "any" || tenantId?.ToLower() == "all")
                    {
                        tenantId = string.Empty;
                    }

                    //using var reader = new System.IO.StreamReader(context.Request.Body);
                    var parameters = param1 ?? "";

                    try
                    {
                        var result = await router.ExecuteCommand(tenantId, commandName, actionName, version, parameters);
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(result);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "An internal error occurred." }));
                    }
                });









            });
            
            return app;
        }

    private static Dictionary<string, string> GetFromRoutes(RouteValueDictionary routeValues)
    {
        throw new NotImplementedException();
    }
}

