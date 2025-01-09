
using MsgQueue.Server;
using System.ComponentModel.DataAnnotations.Schema;
namespace PubSub.Publisher;



public interface ISubscriberEntry

{


    public int SubscriberId { get; set; } //Pk of the table 
    public string TopicId { get; set; }
    public string SubscriberName { get; set; }
	public string Reason { get; set; }
    
	public string SubscriberType { get; set; } //Command or WebHook
    public bool? AllowRetry { get; set; }
	public int? MaxRetry { get; set; }
	
	public int? RetryDelayInMin { get; set; }
	public int? RetryDelayIncrementsInMin { get; set; }
	
	
	public string? WebHookUrl { get; set; }
	public string? WebHookMethod { get; set; }
	
	public string? WebHookSettings { get; set; }
	
	
	public string? CommandName { get; set; }
    public string? ActionName { get; set; }
    public string? TenantName { get; set; }
    public string? CommandVersion { get; set; }
	public string? Status { get; set; }

    [NotMapped]
    public object? Context { get; set; }

    [NotMapped]
    public object? ExtraData { get; set; }

    DateTime CreatedAt { get; set; }
    //string MessageBody { get; set; }

}



public interface IMessageEntry2:IMessageQueueEntry
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

}
