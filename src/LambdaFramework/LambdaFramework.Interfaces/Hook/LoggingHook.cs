using Microsoft.Extensions.Logging;


namespace LambdaFramework.Common;

// Example hook implementation
[Hook("LoggingHook", "Math:Add:1.0.0.0", "", HookType.Pre ,InputData.Modify, OutputData.None)]
public class LoggingHook : IHook
{
    private readonly ILogger<LoggingHook> _logger;

    public LoggingHook(ILogger<LoggingHook> logger)
    {
        _logger = logger;
    }


    public async Task<Tuple<string, bool>> Execute(string tenantName, string commandName, string actionName, string version, string parameters, 
        ICommandContext? context = null, IExecutionContext? executionContext = null, HookAttribute? attribute = null)
    {
        

        _logger.LogInformation("Executing command: {TenantName}:{CommandName}:{ActionName}:{Version}",
            tenantName, commandName, actionName, version);

        var data = Tuple.Create("a", true);
        await Task.Delay(1);
        return (data);

    }
}

[Hook("DebugHook", "Math:Add:1.0.0.0", "", HookType.Pre ,InputData.Modify, OutputData.None)]
public class DebugHook : IHook
{
    private readonly ILogger<DebugHook> _logger;

    public DebugHook(ILogger<DebugHook> logger)
    {
        _logger = logger;
    }


    public async Task<Tuple<string, bool>> Execute(string tenantName, string commandName, string actionName, string version, string parameters, 
        ICommandContext? context = null, IExecutionContext? executionContext = null, HookAttribute? attribute = null)
    {
        

        _logger.LogInformation("Debug command: {TenantName}:{CommandName}:{ActionName}:{Version}",
            tenantName, commandName, actionName, version);

        var data = Tuple.Create("a", true);
        await Task.Delay(1);
        return (data);

    }
}




