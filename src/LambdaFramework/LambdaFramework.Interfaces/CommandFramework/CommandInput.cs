namespace LambdaFramework.Common;

public class CommandInput : ICommandInput
{
    public InputType InputType { get; set; }
    public OutputType OutputType { get; set; }
    public string StringContent { get; set; }
    public Stream BinaryContent { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
}



