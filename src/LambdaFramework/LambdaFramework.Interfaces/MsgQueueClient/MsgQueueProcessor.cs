using LambdaFramework.Common;

namespace MsgQueue.Client;

/// <summary>
/// Default implementation of IMessageQueueProcessor. It processes the message using the command pattern.
/// </summary>
public class MsgQueueProcessor : IMessageQueueProcessor
{
    private readonly ICommandRouter _commandRouter;

    public MsgQueueProcessor(ICommandRouter commandRouter)
    {
        _commandRouter = commandRouter;
    }

    public async Task ProcessMessageAsync(IMsgQueueEntry messageEntry, TopicConfig config)
    {
        try
        {
            //todo call command router
            // Create a command object using the message details and command API
           // var command = _commandRouter.GetCommand(config.CommandName, config.ActionName, config.TenantName, config.CommandVersion);
            //var result = await command.Execute(messageEntry.MsgBody);

            // Command executed successfully, marking as processed
            messageEntry.Status = "Processed";
            messageEntry.ProcessedAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            // Handle errors and mark as error
            messageEntry.Status = "Error";
            messageEntry.ErrorMessage = ex.Message;
            Console.WriteLine($"Error processing message {messageEntry.MessageId}: {ex.Message}");
        }
    }
}
