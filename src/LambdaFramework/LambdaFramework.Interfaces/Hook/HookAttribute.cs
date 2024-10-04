namespace LambdaFramework.Common;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class HookAttribute : Attribute
{
    public string HookName { get; }
    public HookType Type { get; }

    public HookAttribute(string hookName, HookType type)
    {
        HookName = hookName;
        Type = type;
    }
}



