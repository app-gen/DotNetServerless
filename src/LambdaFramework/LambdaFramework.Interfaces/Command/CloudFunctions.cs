using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LambdaFramework.Common;


// AzureFunctionCommand.cs


public abstract class AzureAwsCommand : AbstractCommand
{
    public async Task<IActionResult> RunAsAzureFunction(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        ILogger log)
    {
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadLineAsync()??"";
           // var input = JsonSerializer.Deserialize<CommandInput>(requestBody);

           var result = await Execute(requestBody);

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error executing Azure function command");
            return new BadRequestObjectResult(ex.Message);
        }
    }


    public async Task<APIGatewayProxyResponse> RunAsAwsLambda(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            // var input = JsonSerializer.Deserialize<CommandInput>(request.Body);
            var result = await Execute(request.Body);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = result,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                }
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error: {ex.Message}");
            return new APIGatewayProxyResponse
            {
                StatusCode = 400,
                Body = ex.Message
            };
        }
    }
}

// AwsLambdaCommand.cs
//namespace LambdaFramework.Cloud;

public abstract class CloudLambdaCommandFunction : AzureAwsCommand
{
}

// Example usage of a cloud-ready command
[Command("Math", "Add", "1.0.0.0", "CloudTenant")]
public class CloudAddCommand : CloudLambdaCommandFunction
{
    public override async Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec = null)
    {
        var numbers = parameters.Split(new char[] { ',', ' ', '+' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(int.Parse);
        return numbers.Sum().ToString();
    }
}

// Required models
public class CommandInput1
{
    public string TenantName { get; set; }
    public string Parameters { get; set; }
}