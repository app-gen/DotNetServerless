namespace LambdaFramework.Common;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class HookAttribute : Attribute
{
    public string HookName { get; }

    public string AssociatedCommand { get; }
    public string TenantId { get; }


    public HookType Type { get; }

    public InputData PipeInput { get; }

    public OutputData PipeOutput { get; }


    public HookAttribute(string hookName, string associatedCommand, string tenant, HookType type, InputData pipeInput, OutputData pipeOutput)
    {
        HookName = hookName;
        Type = type;
        PipeInput = pipeInput;
        PipeOutput = pipeOutput;
        AssociatedCommand = associatedCommand;
        TenantId = tenant;
    }
}



