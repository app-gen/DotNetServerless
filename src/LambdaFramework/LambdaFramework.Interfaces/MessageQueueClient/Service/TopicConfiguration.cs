
namespace MsgQueue.Client.Service;

public interface ITopicConfig
{

    public string TopicId { get; set; }
    public string? TopicName { get; set; }

    public int MaxTasks { get; set; }
    public int WaitTimeInSecondsIfEmpty { get; set; }

    public string? Comments { get; set; }

    public string? CommandName { get; set; }
    public string? ActionName { get; set; }
    public string? TenantName { get; set; }
    public string? CommandVersion { get; set; }

    public bool? Active { get; set; }

    public DateTime? StartTimeUtc { get; set; }

    public DateTime? EndTimeUtc { get; set; }

}


public class TopicConfiguration
{
    public string TopicId { get; set; } = string.Empty;
    public int NumberOfThreads { get; set; } = 1;
    public int PollIntervalSeconds { get; set; } = 60; // Time to wait when queue is empty
}


public class TopicConfig : ITopicConfig
{
    public string TopicId { get; set; } = "1";
    public string? TopicName { get; set; } = "TestTopic";

    public int MaxTasks { get; set; } = 10;
    public int WaitTimeInSecondsIfEmpty { get; set; } = 15;

    public string? Comments { get; set; }

    public string? CommandName { get; set; }
    public string? ActionName { get; set; }
    public string? TenantName { get; set; }
    public string? CommandVersion { get; set; }

    public bool? Active { get; set; }

    public DateTime? StartTimeUtc { get; set; }

    public DateTime? EndTimeUtc { get; set; }

}

public class TopicConfigList
{
    public List<TopicConfig> Topics { get; set; } = new();
}
