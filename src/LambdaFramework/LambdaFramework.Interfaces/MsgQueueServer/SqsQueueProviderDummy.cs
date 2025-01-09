namespace MsgQueue.Server;

public class SqsQueueProviderDummy : IQueueProvider
{
    private readonly string _awsAccessKey;
    private readonly string _awsSecretKey;
    private readonly string _region;

    public SqsQueueProviderDummy(Dictionary<string, string> settings)
    {
        _awsAccessKey = settings["AwsAccessKey"];
        _awsSecretKey = settings["AwsSecretKey"];
        _region = settings["AwsRegion"];
    }

    public QueueProviderType ProviderType => QueueProviderType.Sqs;

    public async Task SendMessageAsync(IMessageQueueEntry message)
    {
        // Implementation for SQS
    }

    public async Task<bool> IsHealthyAsync()
    {
        // Check SQS connectivity
        return true;
    }
}
/*

namespace LambdaFramework.Interfaces.MsgQueueServer
{
    class SampleMQServer
    {
    }
}
*/