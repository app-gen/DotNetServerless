namespace LambdaFramework.Common;

public interface ICommand
{
    Task<string> Execute(string parameters, ICommandContext? context=null, IExecutionContext? executionContext= null);

    Task<T> Execute<T>(T paramas, ICommandContext? context = null, IExecutionContext? executionContext = null);


    Task<string> Execute(IRunContext runData, ICommandContext? context = null, IExecutionContext? executionContext = null);

    Task<string> Execute(object parameters, ICommandContext? context = null, IExecutionContext? executionContext = null);


    Task<string> Execute(string msgKey, string msgBody, IJobContext? jobContext,  ICommandContext? context = null, IExecutionContext? executionContext = null);


    Task<CommandOutput> Execute(CommandInput parameters, ICommandContext? context = null, IExecutionContext? executionContext = null);


}



