namespace LambdaFramework.Common;


[Command("Math", "Add", "2.0.0.0", "RomanTenant")]
public class RomanAddCommandV2 : RomanAddCommand, ICommand
{

    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);
        var numbers = parameters.Split(new char[] { ',', ' ', '+' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        // Convert each split element to an integer

        int result = SumRomanNumerals(numbers);
        Console.WriteLine($"The sum of the Roman numerals is: {result}");

        var s = IntToRoman(result);

        // Sum the numbers

        return $"The sum of the Roman numerals is: {result} and in Roman is: {s} ";


    }

}
[Command("Math", "Add", "1.0.0.0", "RomanTenant")]
public class RomanAddCommand : AbstractCommand, ICommand
{

    public string IntToRoman(int num)
    {
        string[] thousands = { "", "M", "MM", "MMM" };
        string[] hundreds = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
        string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
        string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

        return thousands[num / 1000] +
               hundreds[(num % 1000) / 100] +
               tens[(num % 100) / 10] +
               ones[num % 10];
    }
    public int RomanToInt(string roman)
    {
        Dictionary<char, int> romanMap = new Dictionary<char, int>()
    {
        {'I', 1},
        {'V', 5},
        {'X', 10},
        {'L', 50},
        {'C', 100},
        {'D', 500},
        {'M', 1000}
    };

        int total = 0;
        int prevValue = 0;

        foreach (var c in roman)
        {
            int currentValue = romanMap[c];

            // If the current value is greater than the previous value, subtract the double of the previous value (correction for subtraction case).
            if (currentValue > prevValue)
            {
                total += currentValue - 2 * prevValue;
            }
            else
            {
                total += currentValue;
            }

            prevValue = currentValue;
        }

        return total;
    }


    public int SumRomanNumerals(List<string> romanNumerals)
    {
        int sum = 0;

        foreach (var numeral in romanNumerals)
        {
            sum += RomanToInt(numeral);
        }

        return sum;
    }


    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);
        var numbers = parameters.Split(new char[] { ',', ' ', '+' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                           // Convert each split element to an integer

        int result = SumRomanNumerals(numbers);
        Console.WriteLine($"The sum of the Roman numerals is: {result}");


        // Sum the numbers
       
        return $"The sum of the Roman numerals is: {result}";


    }
}


/*
 *  var numbers = input.Split(new char[] { ',', ' ', '+' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(int.Parse); // Convert each split element to an integer

        // Sum the numbers
        int sum = numbers.Sum();

        
*/


