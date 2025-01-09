namespace LambdaFramework.Common;

public class CommandOutput : ICommandOutput
{
    public OutputType OutputType { get; set; }
    public string StringContent { get; set; }
    public Stream BinaryContent { get; set; }
}



