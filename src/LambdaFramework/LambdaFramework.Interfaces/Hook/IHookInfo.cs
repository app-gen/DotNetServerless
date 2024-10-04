
namespace LambdaFramework.Common
{
    public interface IHookInfo
    {
        string HookName { get; set; }
        Type HookType { get; set; }
        HookType Type { get; set; }
    }
}