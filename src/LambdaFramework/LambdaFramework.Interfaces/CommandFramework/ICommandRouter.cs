
namespace LambdaFramework.Common
{
    public interface ICommandRouter
    {
        void ClearCommands();
        Task<string> ExecuteCommand(string tenantName, string commandName, string actionName, string version, string parameters, CommandContext context = null);
        void RegisterCommand(CommandInfo commandInfo);
        void RegisterHook(HookInfo hookInfo);
    }
}