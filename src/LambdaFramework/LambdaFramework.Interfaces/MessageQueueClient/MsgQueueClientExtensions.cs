using Microsoft.AspNetCore.Builder;
//using MsgQueueClientLibrary.Interfaces;

namespace MsgQueue.Client;

/// <summary>
/// Extension method to use MsgQueueClient and start background message processing in an ASP.NET Core app.
/// </summary>
public static class MsgQueueClientExtensions
{
    public static void UseMsgQueueClient(this IApplicationBuilder app, IMsgQueuePickService pickService)
    {

        // Start the background message processing
        // client.StartProcessingAsync().GetAwaiter().GetResult();
    }
}
