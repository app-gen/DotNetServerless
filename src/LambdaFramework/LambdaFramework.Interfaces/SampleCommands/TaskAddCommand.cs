namespace LambdaFramework.Common.SampleTask;

[Command("Task", "Add", "1.0.0.0", "")]
public class AddCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);

        return "Task Added";


    }
}


[Command("SampleTask", "Update", "1.0.0.0", "")]
public class UpdateCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);

        return "Task Updated";


    }
}

[Command("SampleTask", "Search", "1.0.0.0", "")]
public class SearchCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);

        return "Task Updated";


    }
}

[Command("SampleTask", "Get", "1.0.0.0", "")]
public class GetCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);

        return "Task Updated";


    }
}

[Command("SampleTask", "Delete", "1.0.0.0", "")]
public class DeleteCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);

        return "Task Updated";


    }
}

[Command("SampleTask", "Approve", "1.0.0.0", "")]
public class ApproveCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);

        return "Task Updated";


    }
}


[Command("SampleTask", "Hold", "1.0.0.0", "")]
public class HoldCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);

        return "Task Updated";


    }
}


[Command("SampleTask", "Hold", "2.0.0.0", "")]
public class HoldCommandV2 : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);

        return "Task Updated";


    }
}
