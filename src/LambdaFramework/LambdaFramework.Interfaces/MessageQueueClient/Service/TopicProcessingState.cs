
namespace MsgQueue.Client.Serive;

public class TopicProcessingState
{
    public DateTime LastEmptyPollTime { get; set; }
    public bool IsPaused { get; set; }
    public int ActiveThreads { get; set; }
    public SemaphoreSlim PauseLock { get; set; } = new SemaphoreSlim(1, 1);
}
