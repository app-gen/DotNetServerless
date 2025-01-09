using MsgQueue.Server;

namespace SampleMessageQueueServerApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        //SQS MQ

        // Configure Message Queue with SQS
        // Configure Message Queue with SQS
        builder.Services.AddSqsMessageQueue(config =>
        {
            config.CurrentQueueProvider = QueueProviderType.Sqs;
            config.Settings = new Dictionary<string, string>
         {
            { "AwsAccessKey", builder.Configuration["AWS:AccessKey"] ?? "" },
            { "AwsSecretKey", builder.Configuration["AWS:SecretKey"] ?? "" },
            { "AwsRegion", builder.Configuration["AWS:Region"] ?? "us-east-1" },
            { "RetryAttempts", "3" },
            { "RetryDelayMs", "1000" }
         };
        });


        // Register the SQS provider
        // builder.Services.AddScoped<IQueueProvider, SqsQueueProvider>();

        // Register the message queue server
        // builder.Services.AddScoped<IMessageQueueServer, EnhancedMessageQueueServer>();
        ///////////////////////////////////////







        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        //MQ

        ////////////////////////////
        ///

        // Add message queue middleware
        app.UseMessageQueueServer();


        // optional for test Add a sample endpoint to test the queue
        app.MapPost("/api/queue/message", async (IMessageQueueServer messageQueue) =>
        {
            var message = new MessageQueueEntry
            {
                TopicId = "high-priority",
                MessageBody = "Test message",
                System = "TestSystem",
                User = "TestUser",
                CreatedTimeUtc = DateTime.UtcNow
            };

            await messageQueue.SendMessage(message);
            return Results.Ok("Message queued successfully");
        });

        // optional for test Add a sample endpoint to test the queue
        app.MapGet("/api/queue/message", async (IMessageQueueServer messageQueue) =>
        {
            var message = new MessageQueueEntry
            {
                TopicId = "high-priority",
                MessageBody = "Test message",
                System = "TestSystem",
                User = "TestUser",
                CreatedTimeUtc = DateTime.UtcNow
            };

           // await messageQueue.SendMessage(message);
            return Results.Ok("Message queued successfully");
        });





        ///////////////////////////




        app.Run();
    }
}

