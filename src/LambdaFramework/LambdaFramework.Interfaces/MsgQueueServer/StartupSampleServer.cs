using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MsgQueue.Server;

//public class StartupSampleServer
//{
//    public void ConfigureServices(IServiceCollection services)
//    {
//        // Add required services including IMessageQueue implementation
//      //  services.AddScoped<IMessageQueue, DatabaseMessageQueue>();
//        //services.AddDbContext<YourDbContext>(); // Add your database context here
//    }

    //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
    //    IMessageQueue messageQueue)
    //{
    //    if (env.IsDevelopment())
    //    {
    //        app.UseDeveloperExceptionPage();
    //    }

    //    app.UseRouting();

    //    // Add the Message Queue middleware
    //    app.UseMessageQueueLib(messageQueue);

    //    app.UseEndpoints(endpoints =>
    //    {
    //        endpoints.MapControllers();
    //    });
    //}

