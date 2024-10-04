using Microsoft.Extensions.Logging;


namespace LambdaFramework.Common;

// Example hook implementation
[Hook("LoggingHook", HookType.Pre)]
public class LoggingHook : IHook
{
    private readonly ILogger<LoggingHook> _logger;

    public LoggingHook(ILogger<LoggingHook> logger)
    {
        _logger = logger;
    }

    public Task<bool> Execute(string tenantName, string commandName, string actionName, string version, string parameters)
    {
        _logger.LogInformation("Executing command: {TenantName}:{CommandName}:{ActionName}:{Version}",
            tenantName, commandName, actionName, version);
        return Task.FromResult(true);
    }
}



