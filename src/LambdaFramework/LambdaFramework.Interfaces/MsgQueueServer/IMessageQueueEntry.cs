
using System.ComponentModel.DataAnnotations.Schema;

namespace MsgQueue.Server;

    public  interface IMessageQueueEntry
    {


    public int MessageId { get; set; } //Pk of the table 
    public string TopicId { get; set; }
    public string MsgBody { get; set; }
    public string? User { get; set; }

    public int? Retry { get; set; }

    public int? MaxRetry { get; set; }

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

    [NotMapped]
    public object? ExtraData { get; set; }

    DateTime CreatedAt { get; set; }
        string MessageBody { get; set; }
//        string MessageId { get; set; }
  //      string System { get; set; }
    //    string TopicId { get; set; }
      //  string User { get; set; }
    }
