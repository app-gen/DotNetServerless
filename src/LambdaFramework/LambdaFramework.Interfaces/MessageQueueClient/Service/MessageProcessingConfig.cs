
namespace MsgQueue.Client.Service;

public class MessageProcessingConfig
{
    public List<TopicConfiguration> Topics { get; set; } = new();
    public int ErrorDelaySeconds { get; set; } = 5;
}

