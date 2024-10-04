using System.Text.Json;
namespace LambdaFramework.Common;


// Example command implementation
[Command("TestCommand", "Execute", "1.0", "TenantA")]
public class TestCommand : AbstractCommand, ICommand
{
    public async override  Task<string>   Execute(string parameters, ICommandContext? context=null)
    {
        // You can now call other commands from within this command
        var otherCommandResult = await context.ExecuteCommand("OtherCommand", "DoSomething", "1.0", parameters);
        await Task.Delay(1);
        return JsonSerializer.Serialize(new
        {
            Success = true,
            Message = "Command executed successfully",
            Data = parameters,
            OtherCommandResult = otherCommandResult
        });
        
    }
}


[Command("DotNet", "Execute", "1.0", "TenantA")]
public class DotNetScriptCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null)
    {
        return "";
    }
}

[Command("Python", "Execute", "1.0", "TenantA")]
public class PythonScriptCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null)
    {
        return "";
    }
}

[Command("Shell", "Execute", "1.0", "TenantA")]
public class ShellScriptCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null)
    {
        return "";
    }
}

