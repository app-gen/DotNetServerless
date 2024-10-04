namespace LambdaFramework.Common;

[Command("AbstractCommand", "Execute", "1.0", "TenantA")]
public class AbstractCommand : ICommand
{
    public async virtual Task<string> Execute(string parameters, ICommandContext? context = null)
    {
        await Task.Delay(1000);
       throw new NotImplementedException();
    }

    public async virtual Task<CommandOutput> ExecuteCommand(CommandInput parameters, ICommandContext? context = null)
    {
        throw new NotImplementedException();
    }
}



