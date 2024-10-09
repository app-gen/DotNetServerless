using System.ComponentModel.DataAnnotations.Schema;

namespace MsgQueue.Client;

/// <summary>
/// Represents the concrete class for a single entry in the message queue.
/// </summary>
public class MsgQueueEntry : IMsgQueueEntry
{

    public int MessageId { get; set; } //Pk of the table 
    public string TopicId { get; set; }
    public string MsgBody { get; set; }
    public string? User { get; set; }

    public int? Retry { get; set; } = 0;

    public int? MaxRetry  { get; set; } = 0;

    public int? MessagePriority { get; set; }
    public int? MessageQueueProcessId { get; set; }
    public long? ThreadId { get; set; }
    public string? MachineName { get; set; }

    public DateTime? PickupTime { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? ProcessTimeInMilliSeconds { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? UtcExecutionTime { get; set; }
    public DateTime? CreatedDate { get; set; }


    public string? System { get; set; }
    public string? CommandName { get; set; }
    public string? ActionName { get; set; }
    public string? TenantName { get; set; }
    public string? CommandVersion { get; set; }
    public DateTime PickedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Status { get; set; }

    [NotMapped]
    public object? Context { get; set; }
    public object? ExtraData { get; set; }

    //public string? ErrorMessage { get; set; }
}

