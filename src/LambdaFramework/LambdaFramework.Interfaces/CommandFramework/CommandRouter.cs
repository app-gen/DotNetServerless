using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;


namespace LambdaFramework.Common;

public class CommandRouter : ICommandRouter
{
    private Dictionary<string, CommandInfo> _commands = new Dictionary<string, CommandInfo>();
    private Dictionary<string, HookInfo> _preHooks = new Dictionary<string, HookInfo>();
    private Dictionary<string, HookInfo> _postHooks = new Dictionary<string, HookInfo>();
    private readonly ILogger<ICommandRouter> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CommandRouter(ILogger<ICommandRouter> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public void RegisterCommand(CommandInfo commandInfo)
    {
        string key = $"{commandInfo.TenantName ?? ""}:{commandInfo.CommandName}:{commandInfo.ActionName}:{commandInfo.Version}";
        _commands[key] = commandInfo;
    }

    public void RegisterHook(HookInfo hookInfo)
    {
        if (hookInfo.Type == HookType.Pre)
        {
            _preHooks[hookInfo.HookName] = hookInfo;
        }
        else
        {
            _postHooks[hookInfo.HookName] = hookInfo;
        }
    }

    public void ClearCommands()
    {
        _commands.Clear();
        _preHooks.Clear();
        _postHooks.Clear();
    }

    public async Task<string> ExecuteCommand(string tenantName, string commandName, string actionName, string version, string parameters, 
        ICommandContext context = null, IExecutionContext ec = null
        
        )
    {
        CommandInfo commandInfo = FindCommand(tenantName, commandName, actionName, version);

        if (commandInfo == null)
        {
            throw new KeyNotFoundException($"Command not found: {tenantName}:{commandName}:{actionName}:{version}");
        }

        context = context ?? new CommandContext(this, tenantName);

        // Execute pre-hooks
        foreach (var preHook in _preHooks.Values)
        {
            var hook = (IHook)ActivatorUtilities.CreateInstance(_serviceProvider, preHook.HookType);
            //if (!await hook.Execute(tenantName, commandName, actionName, version, parameters))
            //{
            //    _logger.LogWarning("Pre-hook {HookName} failed for command {TenantName}:{CommandName}:{ActionName}:{Version}",
            //        preHook.HookName, tenantName, commandName, actionName, version);
            //    return JsonSerializer.Serialize(new { Success = false, Message = "Pre-hook execution failed" });
            //}
        }

        ICommand command = (ICommand)ActivatorUtilities.CreateInstance(_serviceProvider, commandInfo.CommandType);

        var stopwatch = Stopwatch.StartNew();
        var result = await command.Execute(parameters, context);
        stopwatch.Stop();

        _logger.LogInformation(
            "Executed command {TenantName}:{CommandName}:{ActionName}:{Version} in {ElapsedMilliseconds}ms",
            tenantName, commandName, actionName, version, stopwatch.ElapsedMilliseconds);

        // Execute post-hooks
        foreach (var postHook in _postHooks.Values)
        {
            var hook = (IHook)ActivatorUtilities.CreateInstance(_serviceProvider, postHook.HookType);
            await hook.Execute(tenantName, commandName, actionName, version, result);
        }

        return result;
    }

    private CommandInfo FindCommand(string tenantName, string commandName, string actionName, string version)
    {
        // Try to find tenant-specific command
        string key = $"{tenantName}:{commandName}:{actionName}:{version}";
        if (_commands.TryGetValue(key, out CommandInfo commandInfo))
        {
            return commandInfo;
        }

        // If not found, try to find base command
        key = $":{commandName}:{actionName}:{version}";
        if (_commands.TryGetValue(key, out commandInfo))
        {
            return commandInfo;
        }

        return null;
    }

    public Task<string> ExecuteCommand(string tenantName, string commandName, string actionName, string version, 
        Dictionary<string, string> parameters, ICommandContext context = null, IExecutionContext econtext = null)
    {

        //CommandInfo commandInfo = FindCommand(tenantName, commandName, actionName, version);

        //if (commandInfo == null)
        //{
        //    throw new KeyNotFoundException($"Command not found: {tenantName}:{commandName}:{actionName}:{version}");
        //}

        //context = context ?? new CommandContext(this, tenantName);

        //// Execute pre-hooks
        //foreach (var preHook in _preHooks.Values)
        //{
        //    var hook = (IHook)ActivatorUtilities.CreateInstance(_serviceProvider, preHook.HookType);
        //    if (!await hook.Execute(tenantName, commandName, actionName, version, parameters))
        //    {
        //        _logger.LogWarning("Pre-hook {HookName} failed for command {TenantName}:{CommandName}:{ActionName}:{Version}",
        //            preHook.HookName, tenantName, commandName, actionName, version);
        //        return JsonSerializer.Serialize(new { Success = false, Message = "Pre-hook execution failed" });
        //    }
        //}

        //ICommand command = (ICommand)ActivatorUtilities.CreateInstance(_serviceProvider, commandInfo.CommandType);

        //var stopwatch = Stopwatch.StartNew();
        //var result = await command.Execute(parameters, context);
        //stopwatch.Stop();

        //_logger.LogInformation(
        //    "Executed command {TenantName}:{CommandName}:{ActionName}:{Version} in {ElapsedMilliseconds}ms",
        //    tenantName, commandName, actionName, version, stopwatch.ElapsedMilliseconds);

        //// Execute post-hooks
        //foreach (var postHook in _postHooks.Values)
        //{
        //    var hook = (IHook)ActivatorUtilities.CreateInstance(_serviceProvider, postHook.HookType);
        //  //  await hook.Execute(tenantName, commandName, actionName, version, result);
        //}

        //return result;
        throw new NotImplementedException();
    }
}

