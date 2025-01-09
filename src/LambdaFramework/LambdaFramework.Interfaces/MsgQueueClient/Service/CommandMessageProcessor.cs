using LambdaFramework.Common;
using MsgQueue.Server;


namespace MsgQueue.Client.Service;

/// <summary>
/// Default implementation of IMessageQueueProcessor. It processes the message using the command pattern.
/// </summary>
public class CommandMessageProcessor : IMessageProcessor
{
    private readonly ICommandRouter _commandRouter;
    private readonly ITopicService _ts;

    public CommandMessageProcessor(ICommandRouter commandRouter, ITopicService ts)
    {
        this._commandRouter = commandRouter;
        this._ts = ts;
        //_commandRouter = commandRouter;
    }

    public async Task ProcessMessageAsync(IMessageQueueEntry messageEntry)
    {
        ITopicConfig c = _ts.GetTopicConfig(messageEntry.TopicId);
        await ProcessMessageAsync(messageEntry, c);
    }
    public async Task ProcessMessageAsync(IMessageQueueEntry messageEntry, ITopicConfig config)
    {
        try
        {
            //todo call command router
            // Create a command object using the message details and command API
            // var command = _commandRouter.GetCommand(config.CommandName, config.ActionName, config.TenantName, config.CommandVersion);
            //var result = await command.Execute(messageEntry.MsgBody);

            // Command executed successfully, marking as processed
            messageEntry.Status = "Processed";
            // messageEntry.ProcessedAt = DateTime.UtcNow;
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
