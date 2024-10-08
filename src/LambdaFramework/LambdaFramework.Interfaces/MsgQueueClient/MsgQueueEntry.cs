namespace MsgQueue.Client;

/// <summary>
/// Represents the concrete class for a single entry in the message queue.
/// </summary>
public class MsgQueueEntry : IMsgQueueEntry
{
    public int MessageId { get; set; }
    public string TopicId { get; set; }
    public string MsgBody { get; set; }
    public string User { get; set; }
    public string System { get; set; }
    public string CommandName { get; set; }
    public string ActionName { get; set; }
    public string TenantName { get; set; }
    public string CommandVersion { get; set; }
    public DateTime PickedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string Status { get; set; }
    public string? ErrorMessage { get; set; }
}

