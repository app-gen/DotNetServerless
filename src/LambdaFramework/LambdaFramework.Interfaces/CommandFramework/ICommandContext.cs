
namespace LambdaFramework.Common
{
    public interface ICommandContext
    {
        ICommandRouter Router { get; }
        string TenantName { get; }

        Task<string> ExecuteCommand(string commandName, string actionName, string version, string parameters);
    }
}