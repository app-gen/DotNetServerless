using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using LambdaFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaFramework.Interfaces.CommandFramework;



public class LambdaEntryPoint
{
    [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
    public async Task<APIGatewayProxyResponse> RomanAddHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var command = new CloudAddCommand();
        return await command.RunAsAwsLambda(request, context);
    }

    [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
    public async Task<APIGatewayProxyResponse> BinaryAddHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var command = new CloudAddCommand();
        return await command.RunAsAwsLambda(request, context);
    }
}