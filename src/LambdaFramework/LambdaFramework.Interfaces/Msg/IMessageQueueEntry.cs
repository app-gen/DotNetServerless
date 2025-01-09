
using System.ComponentModel.DataAnnotations.Schema;

namespace MsgQueue.Server;

    public  interface IMessageQueueEntry
    {


    public long MessageId { get; set; } //Pk of the table //We can make it generic like a string

    public string? MessageQueueId { get; set; } //Pk of the table //We can make it generic like a string


    public string TopicId { get; set; }
    public string MessageBody { get; set; }
    public string? User { get; set; }

    public int? Retry { get; set; }

    public int? MaxRetry { get; set; }

    public int? MessagePriority { get; set; }
    public int? MessageQueueProcessId { get; set; }
    public long? ThreadId { get; set; }
    public string? MachineName { get; set; }

    public DateTime? PickupTimeUtc { get; set; }
    public DateTime? StartTimeUtc { get; set; }
    public DateTime? EndTimeUtc { get; set; }
    public int? ProcessTimeInMilliSeconds { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? ExecutionTimeUtc { get; set; }
    public DateTime? CreatedTimeUtc { get; set; }
    //public DateTime PickedAt { get; set; }
    public DateTime? ProcessedTimeUtc { get; set; }
    public string? Status { get; set; }


    // Command 
    public string? System { get; set; }
    public string? CommandName { get; set; }
    public string? ActionName { get; set; }
    public string? TenantName { get; set; }
    public string? CommandVersion { get; set; }
    
    
    [NotMapped]
    public object? Context { get; set; }

    [NotMapped]
    public object? ExtraData { get; set; }

  
    }
