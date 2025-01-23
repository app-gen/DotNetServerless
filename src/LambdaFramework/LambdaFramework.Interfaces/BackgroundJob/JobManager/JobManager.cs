using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;
using NCrontab;
using Microsoft.Extensions.Hosting;
using LambdaFramework.Common;
using MsgQueue.Client;
using Microsoft.Extensions.Configuration;
using MsgQueue.Client.Service;
using MsgQueue.Server;

namespace Command.Job;


    public partial  class JobManager : BackgroundService, IDisposable
    {
        private readonly ILogger<JobManager> _logger;
        private readonly IServiceProvider _globalServiceProvider;
        private readonly IJobConfigurationProvider _configProvider;
        private readonly ICommandRouter _commandRouter;
        private readonly IConfiguration _configuration;

    //  private readonly IMsgQueuePickService _queueService;

    private readonly ConcurrentDictionary<string, IServiceCollection> _localServiceCollections;
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _jobCancellationTokens;
        private readonly ConcurrentDictionary<string, Assembly> _loadedAssemblies;
        private readonly Timer _configRefreshTimer;
        private readonly Timer _maintenanceWindowCheckTimer;
        private readonly IMsgQueuePickService? _queueService;

        public JobConfig _jobConfig { get; set; }

        public JobManager(
            ILogger<JobManager> logger,
            IServiceProvider globalServiceProvider,
            IJobConfigurationProvider configProvider,
            IConfiguration configuration,
            IMsgQueuePickService? queueService=null
            )
        {
            _logger = logger;
            _configuration = configuration;
            _globalServiceProvider = globalServiceProvider;
            _configProvider = configProvider;
            if (queueService!=null)
                _queueService = queueService;
            _localServiceCollections = new ConcurrentDictionary<string, IServiceCollection>();
            _jobCancellationTokens = new ConcurrentDictionary<string, CancellationTokenSource>();
            _loadedAssemblies = new ConcurrentDictionary<string, Assembly>();

        

            // Refresh configuration every 15 minutes
           // _configRefreshTimer = new Timer(_ => RefreshConfiguration(), null, TimeSpan.Zero, TimeSpan.FromMinutes(15));

            // Check maintenance windows every minute
           // _maintenanceWindowCheckTimer = new Timer(_ => PauseOnMaintenance(), null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
     
    }

     

        private async Task RefreshJobs(CancellationToken? stoppingToken=null)
        {
            try
            {
                var configs =  _configProvider.GetJobConfigurationsAsync();
                foreach (var config in configs)
                {
                    try
                    {
                        if (!_jobCancellationTokens.ContainsKey(config.Id) && config.IsEnabled)
                        {
                            if (config.Type == JobType.BackgroundJob)
                            {
                                Console.WriteLine("Background");
                                StartBackgroundJob(config);
                            }
                            else if (config.Type == JobType.MsgQueueClient)
                            {
                                Console.WriteLine("Background");
                                StartMQBackgroundJob(config);
                            }
                            else
                            {
                                Console.WriteLine("Schedule");
                                StartScheduledTask(config);
                            }

                        }
                    }
                    catch (Exception ex) { 
                
                
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing configuration");
            }
        }

        private async Task PauseOnMaintenance()
        {
            await Task.Delay(1);
            
            //try
            //{
            //    var configs = _configProvider.GetJobConfigurationsAsync();
            //    var now = DateTime.UtcNow;

            //    foreach (var config in configs)
            //    {
            //        var isInMaintenanceWindow = config.IsInPauseWindow(now);
            //        var tokenSource = _jobCancellationTokens.GetValueOrDefault(config.Id);

            //        if (isInMaintenanceWindow && tokenSource != null && !tokenSource.IsCancellationRequested)
            //        {
            //            _logger.LogInformation("Pausing job {JobId} due to maintenance window", config.Id);
            //            tokenSource.Cancel();
            //        }
            //        else if (!isInMaintenanceWindow && tokenSource?.IsCancellationRequested == true)
            //        {
            //            _logger.LogInformation("Resuming job {JobId} after maintenance window", config.Id);
            //            RestartJob(config);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error checking maintenance windows");
            //}
        }

        private void  RestartJob(JobTopicConfiguration config)
        {
            if (_jobCancellationTokens.TryRemove(config.Id, out var oldToken))
            {

                try {
                    oldToken.Cancel();
                    oldToken.Dispose();
                }

                catch
                {
                    oldToken.Dispose();
                }
            }
                

            if (config.Type == JobType.BackgroundJob)
            {
                 StartBackgroundJob(config);
            }
            else
            {
                StartScheduledTask(config);
            }
           // await Task.Delay(100);
        }


    private void StartMQBackgroundJob(JobTopicConfiguration config)
    {
        var tokenSource = new CancellationTokenSource();
        _jobCancellationTokens.TryAdd(config.Id, tokenSource);

        Type? jobType;
        IMessageProcessor? job = null;
        
        
        try
        {
            jobType = LoadType(config);
            Console.WriteLine("creating tasks " + jobType);
            job = (IMessageProcessor)ActivatorUtilities.CreateInstance(CreateServiceProvider(config.AssemblyName), jobType);
            Console.WriteLine("creating tasks" + jobType + job);
        }
        catch
        {

            return;
        }

        Task.Run(async () =>
        {
            try
            {
                
               

                
                //todo call init of job
                int max = config.MaxThreads;
                if (max <= 0)
                {
                    max = 1;
                }
                var threads = new Task[max];
                for (int i = 0; i < config.MaxThreads; i++)
                {
                    int thread = i; //take local to avoid thead local issue
                    Console.WriteLine("creating tasks");
                    threads[i] = ExecuteMqJob(thread, job, config, tokenSource.Token);
                }

                await Task.WhenAll(threads);
            }
            catch (Exception ex)
            {
                Console.WriteLine("creating tasks", ex);

                _logger.LogError(ex, "Error running background job {JobId}", config.Id);
            }
        }, tokenSource.Token);
    }


    private void  StartBackgroundJob(JobTopicConfiguration config)
        {
            var tokenSource = new CancellationTokenSource();
            _jobCancellationTokens.TryAdd(config.Id, tokenSource);

            Task.Run(async () =>
            {
                try
                {
                    var jobType = LoadType(config);
                    Console.WriteLine("creating tasks " +  jobType);


                    var job = (IBackgroundJob)ActivatorUtilities.CreateInstance(CreateServiceProvider(config.AssemblyName), jobType);
                    Console.WriteLine("creating tasks" + jobType+ job);
                    //todo call init of job
                    int max = config.MaxThreads;
                    if ( max<=0)
                    {
                        max = 1;
                    }
                    var threads = new Task[max];
                    for (int i = 0; i < config.MaxThreads; i++)
                    {
                        int thread = i; //take local to avoid thead local issue
                        Console.WriteLine("creating tasks");
                        threads[i] = ExecuteBackgroundJob(thread, job, config, tokenSource.Token);
                    }

                    await Task.WhenAll(threads);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("creating tasks", ex);

                    _logger.LogError(ex, "Error running background job {JobId}", config.Id);
                }
            }, tokenSource.Token);
        }

        private void StartScheduledTask(JobTopicConfiguration config)
        {
            var tokenSource = new CancellationTokenSource();
            _jobCancellationTokens.TryAdd(config.Id, tokenSource);

            int pollTimeInMilliSecond = 1000 * 60;
            if (config.PollTimeInMilliSecond >= 1000)
            {
                pollTimeInMilliSecond = config.PollTimeInMilliSecond;
            }

            var taskType = LoadType(config);
            if (taskType == null)
            {
                throw new Exception("IScheduledTask Type not found");
            }

            IScheduledTask task = ActivatorUtilities.CreateInstance(CreateServiceProvider(config.AssemblyName), taskType) as IScheduledTask;

            if (task == null)
            {
                throw new Exception("IScheduledTask not found");
            }


        Task.Run(async () =>
            {
                try
                {
                    
                    
                    var schedule = CrontabSchedule.Parse(config.CronExpression);
                    var nextRun = schedule.GetNextOccurrence(DateTime.UtcNow);
                    Console.WriteLine(nextRun);

                    await task.Initilize(null, null);

                    while (!tokenSource.Token.IsCancellationRequested)
                    {

                        try
                        {
                            var now = DateTime.UtcNow;
                            if (now >= nextRun && !config.IsInPauseWindow(now))
                            {
                                await task.ExecuteAsync(tokenSource.Token,0,null);

                                nextRun = schedule.GetNextOccurrence(DateTime.UtcNow);
                            }
                            Console.WriteLine("waiting for nextRun");

                            Console.WriteLine(nextRun);

                            await Task.Delay(pollTimeInMilliSecond, tokenSource.Token); //wait for a min
                        }
                        catch (Exception ex) {

                            await Task.Delay(pollTimeInMilliSecond, tokenSource.Token);

                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error running scheduled task {TaskId}", config.Id);
                }
            }, tokenSource.Token);
        }

        private async Task ExecuteBackgroundJob(int instance, IBackgroundJob job, JobTopicConfiguration config, CancellationToken cancellationToken)
        {
            Console.WriteLine("IBackgroundJob " +  job);

            int counter = 0;

            while (!cancellationToken.IsCancellationRequested)
            {


                if (counter > 10000) counter = 10000;

                try
                {
                    var now = DateTime.UtcNow;

                    Console.WriteLine("Running " +  instance + "   " +   now);
                    await job.ExecuteAsync(cancellationToken);

                    await Task.Delay(10);

                    if (config.DelayInMilliSecondForEveryRun>0)
                    {
                        await Task.Delay(config.DelayInMilliSecondForEveryRun);
                    }
                    counter = 0;

                }
                catch (Exception e)
                {
                    counter = counter *2 ;
                    Console.WriteLine($"Failed to run job: {e} : Counter {counter}");

                    await Task.Delay(counter * 1000);

                }
            }

        
            //return Task.CompletedTask;

            /*
                while (!cancellationToken.IsCancellationRequested)
                {
                    string message =  _queueService.DequeueMessageAsync(config.QueueName, cancellationToken);
                    if (message != null)
                    {
                        try
                        {
                           /// await job.ProcessMessageAsync(message, CreateServiceProvider(config.AssemblyName), cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing message for job {JobId}", config.Id);
                        }
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(config.EmptyQueueWaitTimeSeconds), cancellationToken);
                    }
                }
            */
        }




    private async Task ExecuteMqJob(int instance, IMessageProcessor job, 
        JobTopicConfiguration config, CancellationToken cancellationToken)
    {
        Console.WriteLine("IMessageTask " + job);
        int counter = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            if (counter > 10000) counter = 10000;

            try
            {
                var now = DateTime.UtcNow;
                await PickAndProcessMessage(instance, job, config, cancellationToken);

                await Task.Delay(1000);
                counter = 0;


            }
            catch (Exception e)
            {
                counter = counter * 2;
                Console.WriteLine($"Failed to run job: {e} : Counter {counter}");

                await Task.Delay(counter * 1000);

            }
        }


        //return Task.CompletedTask;

        /*
            while (!cancellationToken.IsCancellationRequested)
            {
                string message =  _queueService.DequeueMessageAsync(config.QueueName, cancellationToken);
                if (message != null)
                {
                    try
                    {
                       /// await job.ProcessMessageAsync(message, CreateServiceProvider(config.AssemblyName), cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message for job {JobId}", config.Id);
                    }
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(config.EmptyQueueWaitTimeSeconds), cancellationToken);
                }
            }
        */
    }


    private  async Task PickAndProcessMessage(int instance, IMessageProcessor job,
        JobTopicConfiguration config, CancellationToken cancellationToken)
    {
        if (this._queueService == null)
        {
            Console.WriteLine("this._queueService == null");
        }

        IMessageQueueEntry?  message = await  _queueService.PickMessageAsync(config.QueueName);
        if (message == null)
        {
            await Task.Delay(10000);
        }

        try
        {
            await job.ExecuteAsync(message,cancellationToken,config,instance,null);
            //mark as complete;

        }
        catch {

            //mark as complete;
            await Task.Delay(10000);

        }


        /*
            while (!cancellationToken.IsCancellationRequested)
            {
                string message =  _queueService.DequeueMessageAsync(config.QueueName, cancellationToken);
                if (message != null)
                {
                    try
                    {
                       /// await job.ProcessMessageAsync(message, CreateServiceProvider(config.AssemblyName), cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message for job {JobId}", config.Id);
                    }
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(config.EmptyQueueWaitTimeSeconds), cancellationToken);
                }
            }
        */
    }






    private Type LoadType(JobTopicConfiguration config)
        {
            var assembly = _loadedAssemblies.GetOrAdd(config.AssemblyName, LoadAssembly);
            return assembly.GetType(config.TypeName) ?? throw new TypeLoadException($"Could not load type {config.TypeName}");
        }

        private Assembly LoadAssembly(string assemblyName)
        {
            try
            {
                var assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", $"{assemblyName}.dll");
                return Assembly.LoadFrom(assemblyPath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load assembly {assemblyName}", ex);
            }
        }

        private IServiceProvider CreateServiceProvider(string assemblyName)
        {
            IServiceCollection localServices = _localServiceCollections.GetOrAdd(assemblyName, _ => new ServiceCollection());
            ServiceProvider serviceProvider = localServices.BuildServiceProvider();
            return new GloabalLocalServiceProvider(serviceProvider, _globalServiceProvider);
        }

        public void Dispose()
        {
            _configRefreshTimer?.Dispose();
            _maintenanceWindowCheckTimer?.Dispose();
            foreach (var tokenSource in _jobCancellationTokens.Values)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
        }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await  RefreshJobs(stoppingToken);
        
       // return base.ExecuteAsync(stoppingToken);

    }
}

