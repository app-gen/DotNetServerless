namespace LambdaFramework.Common;

public class CommandInfo : ICommandInfo
{
    public Type CommandType { get; set; }
    public string TenantName { get; set; }
    public string CommandName { get; set; }
    public string ActionName { get; set; }
    public string Version { get; set; }
}



