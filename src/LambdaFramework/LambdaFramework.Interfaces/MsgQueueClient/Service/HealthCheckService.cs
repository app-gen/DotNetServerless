using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MsgQueue.Client;
using MsgQueue.Service;
using Microsoft.Extensions.Diagnostics.HealthChecks;
//using Serilog;
namespace MsgQueue.Client.Serive;
public class MessageProcessingHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Implement your health check logic here
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));
        }
    }
}
