namespace MsgQueue.Server;

// Enhanced MessageQueueServer
// namespace MsgQueue.Server;

public class CommonMessageQueueServer : IMessageQueueServer
{
   
    private readonly MessageQueueConfig _config;
    private readonly  IQueueProvider _provider;


    public CommonMessageQueueServer(MessageQueueConfig config, IQueueProvider provider)
    {
        _config = config;
     
        _provider = provider;
       

    }

 
    public async Task SendMessage(IMessageQueueEntry message)
    {
        //This is SQS or Database 
        await _provider.SendMessageAsync(message);
    }
}
