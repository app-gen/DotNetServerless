namespace MsgQueue.Client;

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
