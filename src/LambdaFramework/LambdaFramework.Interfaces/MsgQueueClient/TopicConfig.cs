namespace MsgQueue.Client;

/// <summary>
/// Configures topics and the number of threads/tasks per topic for processing messages.
/// </summary>
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

