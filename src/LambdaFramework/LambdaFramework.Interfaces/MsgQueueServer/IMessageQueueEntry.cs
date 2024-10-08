
namespace MsgQueue.Server;

    public  interface IMessageQueueEntry
    {
        DateTime CreatedAt { get; set; }
        string MessageBody { get; set; }
        string MessageId { get; set; }
        string System { get; set; }
        string TopicId { get; set; }
        string User { get; set; }
    }
