using LambdaFramework.Common;

namespace PythonScript;


[Command("Python", "Execute", "1.0.0.0", "")]
public class PythonCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);
        return string.Empty;

    }
}

