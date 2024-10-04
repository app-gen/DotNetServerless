namespace LambdaFramework.Common;

//public class AbstractCommand: ICommand
//{

//}





//namespace EnhancedDynamicCommandFramework

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandAttribute : Attribute
{
    public string TenantName { get; }
    public string CommandName { get; }
    public string ActionName { get; }
    public string Version { get; }

    public string InputType { get; }
    public string OutputType { get; }


    public CommandAttribute(string commandName, 
        string actionName="Execute", 
        string version="1.0.0.0", string tenantName = "", 
        string input="string" , string output="string")
    {
        CommandName = commandName;
        ActionName = actionName;
        Version = version;
        TenantName = tenantName;
        InputType = input;
        OutputType = output;
    }
}



