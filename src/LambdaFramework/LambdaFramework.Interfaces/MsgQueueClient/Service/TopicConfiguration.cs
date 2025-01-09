
namespace MsgQueue.Client.Serive;

public class TopicConfiguration
{
    public string TopicId { get; set; } = string.Empty;
    public int NumberOfThreads { get; set; } = 1;
    public int PollIntervalSeconds { get; set; } = 60; // Time to wait when queue is empty
}
