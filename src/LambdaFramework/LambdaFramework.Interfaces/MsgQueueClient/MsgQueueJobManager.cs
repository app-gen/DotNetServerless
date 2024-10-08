using Microsoft.Extensions.Configuration;

namespace MsgQueue.Client;

public class MsgQueueJobManager : IMsgQueueJobManager
{
    private readonly IMsgQueuePickService _msgQueuePickService;
    private readonly IConfiguration _configuration;

    private readonly List<TopicConfig> _topicConfigs;


    public MsgQueueJobManager(IMsgQueuePickService msgQueueClient, IConfiguration configuration)
    {
        _msgQueuePickService = msgQueueClient;
        _configuration = configuration;

        // Deserialize the "MsgQueueConfig:Topics" section from configuration into a list of TopicConfig objects
        _topicConfigs = configuration.GetSection("MsgQueueConfig:Topics").Get<List<TopicConfig>>() ?? new List<TopicConfig>();

        if (_topicConfigs.Count == 0) {

            Console.WriteLine("No Topics");
        }
    }



    public void StartProcessing()
    {
        foreach (var topicConfig in _topicConfigs.Where(x=> (!x.Active.HasValue) || x.Active.Value  ))
        {
            ProcessMessagesForTopic(topicConfig);
        }
    }

    private void ProcessMessagesForTopic( TopicConfig config)
    {
        int noOfTasks = config.MaxTasks>0? config.MaxTasks:1;


        var semaphore = new SemaphoreSlim(noOfTasks);

        for (int i = 0; i < noOfTasks; i++)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await semaphore.WaitAsync();

                    try
                    {
                        var message = await _msgQueuePickService.PickMessageAsync(config.TopicId);
                        if (message != null)
                        {
                            MergeMessage(message,config);

                            //await _msgQueuePickService.ProcessMessageAsync(message, config);
                        }
                        else
                        {
                            await Task.Delay(config.WaitTimeInSecondsIfEmpty * 1000);
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }
            });
        }
    }

    private void MergeMessage(IMsgQueueEntry message, TopicConfig config)
    {
        //throw new NotImplementedException();

    }
}