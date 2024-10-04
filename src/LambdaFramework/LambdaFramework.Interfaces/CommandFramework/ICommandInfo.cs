
namespace LambdaFramework.Common
{
    public interface ICommandInfo
    {
        string ActionName { get; set; }
        string CommandName { get; set; }
        Type CommandType { get; set; }
        string TenantName { get; set; }
        string Version { get; set; }
    }
}