/*
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
*/

namespace LambdaFramework.Common;

public interface ICommand
{
    Task<string> Execute(string parameters, ICommandContext? context=null);

    Task<CommandOutput> ExecuteCommand(CommandInput parameters, ICommandContext? context = null);


}



