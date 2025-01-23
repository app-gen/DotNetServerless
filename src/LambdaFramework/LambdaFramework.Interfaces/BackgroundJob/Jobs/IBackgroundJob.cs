using Microsoft.Extensions.DependencyInjection;
using LambdaFramework.Common;
using Quartz;
using MsgQueue.Server;
namespace Command.Job;

public interface IBackgroundJob:  IJob
{
    string JobId { get; set; }
    string AgentId { get; set; }

    Task Initilize(IServiceCollection collection, ICommandRouter router); 

    Task ExecuteAsync(CancellationToken cancellationToken, int agentId=0, IServiceProvider? sp=null);



}
