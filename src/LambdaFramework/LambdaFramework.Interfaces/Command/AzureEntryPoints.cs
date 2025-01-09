using LambdaFramework.Common;
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
namespace LambdaFramework.Interfaces.CommandFramework;


public class RomanAddFunction
{
    [FunctionName("MathRomanAddV1")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "math/add/v1/roman")] HttpRequest req,
        ILogger log)
    {
        var command = new CloudAddCommand();
        return await command.RunAsAzureFunction(req, log);
    }
}

// Functions/BinaryAddFunction.cs
public class BinaryAddFunction
{
    [FunctionName("MathBinaryAddV1")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "math/add/v1/binary")] HttpRequest req,
        ILogger log)
    {
        var command = new CloudAddCommand();
        return await command.RunAsAzureFunction(req, log);
    }
}
