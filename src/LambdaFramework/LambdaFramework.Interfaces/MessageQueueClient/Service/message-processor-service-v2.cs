//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Configuration;
//using MsgQueue.Client;
////using MsgQueue.Service;

//using System.Collections.Concurrent;
//using MsgQueue.Server;


//namespace MsgQueue.Client.Serive;

//public partial class MessageProcessingService2 : BackgroundService
//{
//    private readonly ILogger<MessageProcessingService2> _logger;
//    private readonly IMsgQueuePickService _queueService;
//    private readonly MessageProcessingConfig _config;
//    private readonly ConcurrentDictionary<string, TopicProcessingState> _topicStates;
//    private readonly ConcurrentDictionary<string, List<Task>> _runningTasks;
//    private readonly CancellationTokenSource _shutdownTokenSource;

//    public MessageProcessingService2(
//        ILogger<MessageProcessingService2> logger,
//        IMsgQueuePickService queueService,
//        IConfiguration configuration)
//    {
//        _logger = logger;
//        _queueService = queueService;
//        _config = new MessageProcessingConfig();
//        configuration.GetSection("MessageProcessing").Bind(_config);
        
//        _topicStates = new ConcurrentDictionary<string, TopicProcessingState>();
//        _runningTasks = new ConcurrentDictionary<string, List<Task>>();
//        _shutdownTokenSource = new CancellationTokenSource();

//     //   ValidateConfiguration();
//       // InitializeTopicStates();
//    }

  

   

    
//}
