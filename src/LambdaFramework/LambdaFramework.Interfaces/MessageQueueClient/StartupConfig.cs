using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
//using MsgQueueClientLibrary.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace MsgQueue.Client;

public class StartupConfig
{
    public void ConfigureServices(IServiceCollection services)
    {
        //// Register dependencies (DbContext, clients, processors, etc.)
        //services.AddDbContext<AppDbContext>(options =>
        //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        //services.AddScoped<IMsgQueueClient, MsgQueueClient>();
        //services.AddScoped<IMsgQueueProcessor, MsgQueueProcessor>();
        //services.AddScoped<IMsgQueueJobManager, MsgQueueJobManager>();

        // Register the background service
        services.AddHostedService<MsgQueueBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Other middleware registrations...

        // Use MsgQueue Client (optional)
        app.UseMsgQueueClient();
    }
}
