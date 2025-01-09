namespace LambdaFramework.Common;

[Command("AbstractCommand", "Execute", "1.0", "TenantA")]
public class AbstractCommand : ICommand
{
    public async virtual Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1000);
       throw new NotImplementedException();
    }

    public Task<T> Execute<T>(T paramas, ICommandContext? context = null, IExecutionContext? executionContext = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> Execute(IRunContext runData, ICommandContext? context = null, IExecutionContext? executionContext = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> Execute(object parameters, ICommandContext? context = null, IExecutionContext? executionContext = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> Execute(string msgKey, string msgBody, IJobContext? jobContext, ICommandContext? context = null, IExecutionContext? executionContext = null)
    {
        throw new NotImplementedException();
    }

    public async virtual Task<CommandOutput> Execute(CommandInput parameters, ICommandContext? context = null, IExecutionContext ec = null)
    {
        throw new NotImplementedException();
    }
}


