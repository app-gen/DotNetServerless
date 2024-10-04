using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
/*
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
*/


namespace LambdaFramework.Common;

public interface IHook
{
    Task<bool> Execute(string tenantName, string commandName, string actionName, string version, string parameters);
}



