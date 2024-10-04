namespace LambdaFramework.Common;

public class CommandContext : ICommandContext
{
    public ICommandRouter Router { get; }
    public string TenantName { get; }

    public CommandContext(ICommandRouter router, string tenantName)
    {
        Router = router;
        TenantName = tenantName;
    }

    public Task<string> ExecuteCommand(string commandName, string actionName, string version, string parameters)
    {
        return Router.ExecuteCommand(TenantName, commandName, actionName, version, parameters, this);
    }
}

