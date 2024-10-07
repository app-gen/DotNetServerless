namespace LambdaFramework.Common;

[Command("Math", "Add", "1.0.0.0", "BinaryTenant")]
public class BinaryAddCommand : AbstractCommand, ICommand
{

    public string SumBinaryNumbers(List<string> binaryNumbers)
    {
        int sum = 0;

        // Convert each binary number to integer and sum them
        foreach (var binary in binaryNumbers)
        {
            sum += Convert.ToInt32(binary, 2); // Convert binary string to integer
        }

        // Convert the sum back to binary string
        return Convert.ToString(sum, 2);
    }

    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {

        await Task.Delay(1);
        var numbers = parameters.Split(new char[] { ',', ' ', '+' }, StringSplitOptions.RemoveEmptyEntries).ToList();


        // Sum the numbers
        var sum = SumBinaryNumbers(numbers);// numbers.Sum();
        return $"Sorry I am not sure but U can try, Is it {sum}.";


    }

        //await Task.Delay(1);
        //var numbers = parameters.Split(new char[] { ',', ' ', '+' }, StringSplitOptions.RemoveEmptyEntries)
        //                   .Select(int.Parse); // Convert each split element to an integer

        //// Sum the numbers
        //int sum = numbers.Sum();
        //return sum.ToString();


    
}


/*
 *  var numbers = input.Split(new char[] { ',', ' ', '+' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(int.Parse); // Convert each split element to an integer

        // Sum the numbers
        int sum = numbers.Sum();

        
*/


