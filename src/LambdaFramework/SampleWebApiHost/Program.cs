
using LambdaFramework.Common;
using System.Reflection;
using System.Text.Json;

namespace SampleWebApiHost;

//Sample Program

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

//Sample Program

public class Startup
{

    public void ConfigureServices(IServiceCollection services)
    {
        //Configure Serverless Lib
        services.AddCommandApiLib(options =>
        {
            options.PluginDirectoty = "plugins";
            options.EnableLogging = true;  // Disable logging for example
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICommandRouter router, IExecutionContext context)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        
        //Add Serverless Lib
        app.UseCommandApiLib(router,context);

        //load current static plugins
        PluginLoader.LoadPluginsFromAssembly(Assembly.GetExecutingAssembly() ,router);

        // Load command plugins
        PluginLoader.LoadCommonPlugins(router);

    }
}

