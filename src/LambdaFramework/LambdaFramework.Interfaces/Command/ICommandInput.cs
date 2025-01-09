
namespace LambdaFramework.Common
{
    public interface ICommandInput
    {
        Stream BinaryContent { get; set; }
        InputType InputType { get; set; }
        OutputType OutputType { get; set; }
        string StringContent { get; set; }
        string Token { get; set; }
        string UserId { get; set; }
    }
}