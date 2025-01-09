
namespace LambdaFramework.Common
{
    public interface ICommandOutput
    {
        Stream BinaryContent { get; set; }
        OutputType OutputType { get; set; }
        string StringContent { get; set; }
    }
}