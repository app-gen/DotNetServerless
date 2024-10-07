
namespace LambdaFramework.Common
{
    public interface ICommandRouter
    {
        void ClearCommands();
        Task<string> ExecuteCommand(string tenantName, string commandName, string actionName, string version, string parameters, 
            ICommandContext? context = null, IExecutionContext? econtext = null);

        Task<string> ExecuteCommand(string tenantName, string commandName, string actionName, string version, Dictionary<string,string> parameters,
            ICommandContext? context = null, IExecutionContext? econtext = null);

        void RegisterCommand(CommandInfo commandInfo);
        void RegisterHook(HookInfo hookInfo);
    }

    public interface IExecutionContext
    {
        void ClearCommands();
    }

    public class ExecutionContext: IExecutionContext
    {
        
        void IExecutionContext.ClearCommands()
        {
            throw new NotImplementedException();
        }
    }
}