namespace LambdaFramework.Common;

[Command("Math", "Add", "1.0.0.0", "")]
public class AddCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null)
    {
        await Task.Delay(1);
        var numbers = parameters.Split(new char[] { ',', ' ', '+' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                           .Select(int.Parse); // Convert each split element to an integer

        // Sum the numbers
        int sum = numbers.Sum();
        return sum.ToString();


    }
}



