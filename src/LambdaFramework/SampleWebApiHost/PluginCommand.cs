
using LambdaFramework.Common;

namespace SampleWebApiHost;



//base command for all tentants
[Command("Task", "Add", "1.0.0.0", "")]
public class TaskAddCommand_Common : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec = null)
    {
        await Task.Delay(1);
        //todo add a task to table
        return parameters;

    }
}


[Command("Notes", "Add", "1.0.0.0", "")]
public class NotesAddCommand_Common : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec = null)
    {
        await Task.Delay(1);
        //todo add a task to table
        return parameters;


    }
}


//override the base command
[Command("Task", "Add", "1.0.0.0", "TenanatA")]
public class TenanatA_TaskAddCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec = null)
    {
        await Task.Delay(1);
        //todo add a task to table
        return parameters;


    }
}



[Command("Math", "Minus", "1.0.0.0", "")]
public class MinusCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec = null)
    {
        await Task.Delay(1);
        var numbers = parameters.Split(new char[] { ',', ' ', '+', '-' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(int.Parse).ToList(); // Convert each split element to an integer

        int n1 = numbers[0];
        int n2 = numbers[1];


        // Sum the numbers
        int r = n1 - n2;

        return r.ToString();


    }
}


[Command("Echo", "Minus", "1.0.0.0", "")]
public class EchoCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec = null)
    {
        await Task.Delay(1);
        Console.WriteLine(parameters);
        return parameters.ToString();

    }
}

[Command("Log", "Write", "1.0.0.0", "")]
public class LogCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec = null)
    {
        await Task.Delay(1);
        Console.WriteLine(parameters);
        return parameters.ToString();

    }
}

[Hook("DebugHook1", "Math:Add:1.0.0.0", "", HookType.Pre, InputData.Modify, OutputData.None)]
public class Debug2Hook : IHook
{
    private readonly ILogger<Debug2Hook> _logger;

    public Debug2Hook(ILogger<Debug2Hook> logger)
    {
        _logger = logger;
    }


    public async Task<Tuple<string, bool>> Execute(string tenantName, string commandName, string actionName, string version, string parameters,
        ICommandContext? context = null, IExecutionContext? executionContext = null, HookAttribute? attribute = null)
    {


        _logger.LogInformation("Debug command: {TenantName}:{CommandName}:{ActionName}:{Version}",
            tenantName, commandName, actionName, version);

        var data = Tuple.Create("a", true);
        await Task.Delay(1);
        return (data);

    }
}

//Common Command for all 
[Hook("Discount", "Product:Price:1.0.0.0", "", HookType.Pre, InputData.Modify, OutputData.None)]
public class Discount : IHook
{

    public async Task<Tuple<string, bool>> Execute(string tenantName, string commandName, string actionName, string version, string parameters,
        ICommandContext? context = null, IExecutionContext? executionContext = null, HookAttribute? attribute = null)
    {

        var rate = decimal.Parse(parameters);
        var finalRate = rate * 0.85M;

        var data = Tuple.Create(finalRate.ToString(), true);
        await Task.Delay(1);
        return (data);

    }
}



[Hook("Discount", "Product:Price:1.0.0.0", "TenantA", HookType.Pre, InputData.Modify, OutputData.None)]
public class TenantADiscount_Pre : IHook
{

    public async Task<Tuple<string, bool>> Execute(string tenantName, string commandName, string actionName, string version, string parameters,
        ICommandContext? context = null, IExecutionContext? executionContext = null, HookAttribute? attribute = null)
    {

        var rate = decimal.Parse(parameters);
        var finalRate = rate * 0.85M;

        var data = Tuple.Create(finalRate.ToString(), true);
        await Task.Delay(1);
        return (data);

    }
}


[Hook("Discount", "Product:Price:1.0.0.0", "TenantA", HookType.Post, InputData.Modify, OutputData.None)]
public class TenantADiscount_Post : IHook
{

    public async Task<Tuple<string, bool>> Execute(string tenantName, string commandName, string actionName, string version, string parameters,
        ICommandContext? context = null, IExecutionContext? executionContext = null, HookAttribute? attribute = null)
    {

        var rate = decimal.Parse(parameters);
        var finalRate = rate * 0.85M;

        var data = Tuple.Create(finalRate.ToString(), true);
        await Task.Delay(1);
        return (data);

    }
}


[Hook("Tax", "Product:Price:1.0.0.0", "", HookType.Pre, InputData.Modify, OutputData.None)]
public class Tax : IHook
{

    public async Task<Tuple<string, bool>> Execute(string tenantName, string commandName, string actionName, string version, string parameters,
        ICommandContext? context = null, IExecutionContext? executionContext = null, HookAttribute? attribute = null)
    {

        var rate = decimal.Parse(parameters);
        var finalRate = rate * 1.10M;

        var data = Tuple.Create(finalRate.ToString(), true);
        await Task.Delay(1);
        return (data);

    }
}

[Command("Product", "Price", "1.0.0.0", "")]
public class ProductPriceCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec = null)
    {
        var qty = decimal.Parse(parameters);
        var cost = 105;
        var total = qty * cost;
        await Task.Delay(1);
        Console.WriteLine(parameters);
        Console.WriteLine(total);
        return total.ToString();

    }
}



[Hook("OtherTax", "OtherProduct:OtherPrice:1.0.0.0", "", HookType.Pre, InputData.Modify, OutputData.None)]
public class OtherTax : IHook
{

    public async Task<Tuple<string, bool>> Execute(string tenantName, string commandName, string actionName, string version, string parameters,
        ICommandContext? context = null, IExecutionContext? executionContext = null, HookAttribute? attribute = null)
    {

        var rate = decimal.Parse(parameters);
        var finalRate = rate * 1.10M;

        var data = Tuple.Create(finalRate.ToString(), true);
        await Task.Delay(1);
        return (data);

    }
}

