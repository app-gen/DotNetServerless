
using LambdaFramework.Common;
using Microsoft.AspNetCore.Hosting;

using System.Text.Json;

namespace LambdaFramework;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}


public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ICommandRouter,CommandRouter>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICommandRouter router)
    {
        

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/api/execute/{tenantId}/{commandName}/{actionName}/{version}/{inputFormat?}/{outputFormat?}", async context =>
            {
                var tenantId = context.Request.RouteValues["tenantId"] as string;
                var commandName = context.Request.RouteValues["commandName"] as string;
                var actionName = context.Request.RouteValues["actionName"] as string;
                var version = context.Request.RouteValues["version"] as string;

                if (tenantId?.ToLower() == "any"  || tenantId?.ToLower() == "all")
                {
                    tenantId= string.Empty;
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
        });

        // Load plugins
        PluginLoader.LoadCommonPlugins(router);

        //PluginLoader.LoadPlugins("");
    }
}


//public class Program1
//    {
//        public static void Main1(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }

