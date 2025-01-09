namespace MsgQueue.Client.Service
{
    public interface ITopicService
    {
        ITopicConfig? GetTopicConfig(string id);
    }
}