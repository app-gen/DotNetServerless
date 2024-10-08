namespace MsgQueue.Client;

/// <summary>
/// Represents the interface for a single entry in the message queue.
/// </summary>
public interface IMsgQueueEntry
{
    int MessageId { get; set; }
    string TopicId { get; set; }
    

    string MsgBody { get; set; }
    string User { get; set; }
    string System { get; set; }
    string CommandName { get; set; }
    string ActionName { get; set; }
    string TenantName { get; set; }
    string CommandVersion { get; set; }
    DateTime PickedAt { get; set; }
    DateTime? ProcessedAt { get; set; }
    string Status { get; set; }
    string? ErrorMessage { get; set; }  // To store error messages if the message processing fails
}

