using Microsoft.Extensions.Hosting;
//using MsgQueueClientLibrary.Interfaces;

namespace MsgQueue.Client;

//public class MsgQueueRunner :
//{
//    private readonly IMessageQueueProcessor _processor;
//    private readonly IMsgQueuePickService _queService;

//    private readonly IMessageQueueSystem _messageQueue;
//    private readonly List<TopicConfig> _topicConfigs;
//    private readonly SemaphoreSlim _semaphore;

//    public MsgQueueRunner(IMessageQueueProcessor processor,

//        List<TopicConfig> topicConfigs)
//    {
//        _processor = processor;
//        //_messageQueue = messageQueue;
//        _topicConfigs = topicConfigs;
//        _semaphore = new SemaphoreSlim(1, 1);  // Can be adjusted based on thread needs
//    }

//    public async Task StartProcessingAsync()
//    {
//        // Create background tasks for each topic
//        foreach (var config in _topicConfigs)
//        {
//            for (int i = 0; i < config.MaxTasks; i++)
//            {
//                _ = Task.Run(async () =>
//                {
//                    while (true)
//                    {
//                        var message = await _queService.PickMessageAsync(config.TopicId);
//                        if (message != null)
//                        {
//                            await _processor.ProcessMessageAsync(message, config);
//                        }
//                        else
//                        {
//                            // Wait for the configured time if no message is available
//                            await Task.Delay(config.WaitTimeInSecondsIfEmpty * 1000);
//                        }
//                    }
//                });
//            }
//        }
//    }


public class MsgQueueBackgroundService : BackgroundService
    {
        //private readonly IMsgQueueJobManager _msgQueueJobManager;

        //public MsgQueueBackgroundService(IMsgQueueJobManager msgQueueJobManager)
        //{
        //    _msgQueueJobManager = msgQueueJobManager;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //  await _msgQueueJobManager.StartProcessing();
                await Task.Delay(10000, stoppingToken); // Optional delay between processing cycles
            }
        }
    }
