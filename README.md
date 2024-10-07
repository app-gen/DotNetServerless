
# Project Name Serverless Library using Command Pattern

## Overview 

This project provides a framework for building and executing command-based functionality through an extensible plugin architecture. The project supports the creation of commands, hooks, and plugins that can be dynamically loaded and executed via an API.

## Features 
 
- **Command execution** : Define commands as plugins that can be dynamically loaded at runtime and executed via a common interface.
 
- **Hooks** : Support for `Pre` and `Post` execution hooks.
 
- **Plugin loading** : Load plugins from a directory or assembly dynamically.
 
- **API endpoints** : Expose commands through HTTP endpoints, allowing remote execution of commands via POST and GET requests.
 
- **Multi-tenant support** : Ability to execute commands per tenant.
 
- **Versioning** : Commands are versioned, allowing for multiple versions of the same command to coexist.

## Installation 

### Note: 
You can install the **Serverless.LambdaCommand**  library from NuGet by running the following command in your NuGet Package Manager Console:

```bash
Install-Package Serverless.LambdaCommand
```

This package provides the required components to implement a serverless-like plugin architecture where commands can be dynamically loaded and executed, similar to AWS Lambda.


### Sample Program 


```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Configure the serverless library
        services.AddCommandApiLib(options =>
        {
            options.PluginDirectory = "plugins"; // Directory where plugins are stored
            options.EnableLogging = true;        // Enable logging (can be toggled on/off)
        });

        // Add other necessary services for the application (e.g., database, authentication, etc.)
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICommandRouter router, IExecutionContext context)
    {
        if (env.IsDevelopment())
        {
            // Use developer exception page in development environment
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting(); // Enable routing for the app

        // Add the serverless library middleware
        app.UseCommandApiLib(router, context);

        // Load the static plugins (from the executing assembly)
        PluginLoader.LoadPluginsFromAssembly(Assembly.GetExecutingAssembly(), router);

        // Load dynamically any command plugins from a common location (e.g., the 'plugins' folder)
        PluginLoader.LoadCommonPlugins(router);

        // Setup any additional middleware, such as authentication, authorization, etc.
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Map controller routes if using MVC
        });
    }
}
```


### Prerequisites 
 
- [.NET Core SDK](https://dotnet.microsoft.com/download)
 
- A suitable logging provider, such as [Serilog](https://serilog.net/)

### Setup 
 
1. Clone the repository.
 
2. Navigate to the project directory.
 
3. Restore the project dependencies using the following command:


```bash
dotnet restore
```
 
4. Build the project:


```bash
dotnet build
```
 
5. Run the project:


```bash
dotnet run
```

## Usage 

### Defining Commands 
Commands implement the `ICommand` interface, which provides several overloaded `Execute` methods. You can define your command logic by implementing these methods.

```csharp
public class AddCommand : AbstractCommand, ICommand
{
    public async override Task<string> Execute(string parameters, ICommandContext? context = null, IExecutionContext ec =null)
    {
        await Task.Delay(1);
        var numbers = parameters.Split(new char[] { ',', ' ', '+' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse);
        int sum = numbers.Sum();
        return sum.ToString();
    }
}
```
In this example, the `AddCommand` splits the input `parameters` and calculates the sum of numbers.
### Loading Plugins 
The `PluginLoader` class provides mechanisms to dynamically load commands from assemblies or directories.

```csharp
var pluginLoader = new PluginLoader(logger, router, pluginDirectory);
pluginLoader.LoadPlugins();
```

This loads all commands and hooks available in the specified plugin directory.

### API Endpoints 

Commands can be executed via HTTP endpoints.
 
- **POST** : `/api/execute/{commandName}/{actionName}/{version}/{tenantId}/{inputFormat}/{outputFormat}`
 
- **GET** : `/api/execute/{commandName}/{actionName}/{tenantId}/{version}/{outputFormat}/{param1}/{param2}/...`
Example of a POST request to execute the `AddCommand`:

```bash
curl -X POST "http://localhost:5000/api/execute/Math/Add/1.0.0.0/TenantA" -d "2+3+4"
```

### Configuration 
You can configure various aspects of the framework using the `CommandApiOptions` class:

```csharp
public class CommandApiOptions
{
    public string PluginDirectory { get; set; } = "plugins";
    public bool EnableLogging { get; set; } = true;
}
```
Configure the framework in `Startup.cs`:

```csharp
services.AddCommandApiLib(options =>
{
    options.PluginDirectory = "custom_plugins";
    options.EnableLogging = true;
});
```

## Code Documentation 

### Enums 
 
- `HookType`: Defines hook types as either `Pre` or `Post`.
 
- `InputData` and `OutputData`: Specify types of data passed into and out of the command execution.

### Interfaces 
 
- `ICommand`: Represents a command to be executed. Various `Execute` methods are overloaded to accept different types of input, including strings, objects, and contexts.

### Classes 
 
- `AbstractCommand`: A base class implementing the `ICommand` interface, serving as a foundation for creating commands.
 
- `PluginLoader`: Responsible for loading plugins from a directory or assembly. It registers all discovered commands and hooks in the provided `ICommandRouter`.

## Contributing 

Feel free to open issues or submit pull requests to help improve the framework.


---



Here’s how to initialize the serverless library in a sample program using `Startup.cs`. This setup includes loading command plugins dynamically from a specified directory (`plugins`) and configuring basic routing, logging, and plugin loading.
### Sample Program 


```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Configure the serverless library
        services.AddCommandApiLib(options =>
        {
            options.PluginDirectory = "plugins"; // Directory where plugins are stored
            options.EnableLogging = true;        // Enable logging (can be toggled on/off)
        });

        // Add other necessary services for the application (e.g., database, authentication, etc.)
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICommandRouter router, IExecutionContext context)
    {
        if (env.IsDevelopment())
        {
            // Use developer exception page in development environment
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting(); // Enable routing for the app

        // Add the serverless library middleware
        app.UseCommandApiLib(router, context);

        // Load the static plugins (from the executing assembly)
        PluginLoader.LoadPluginsFromAssembly(Assembly.GetExecutingAssembly(), router);

        // Load dynamically any command plugins from a common location (e.g., the 'plugins' folder)
        PluginLoader.LoadCommonPlugins(router);

        // Setup any additional middleware, such as authentication, authorization, etc.
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Map controller routes if using MVC
        });
    }
}
```

### Explanation of Key Components: 
 
1. **ConfigureServices** : 
  - The `AddCommandApiLib` method initializes the serverless library, specifying the directory where plugins are stored (e.g., "plugins"). The logging can be enabled or disabled as needed.
 
2. **Configure** : 
  - The method sets up the application's request pipeline. It includes error handling, routing, and adding the serverless library middleware (`app.UseCommandApiLib`).
 
  - **Plugin Loading** : Two methods, `LoadPluginsFromAssembly` and `LoadCommonPlugins`, handle loading both static and dynamically loaded plugins for the system. These could represent commands or functions that the system supports.
 
3. **PluginLoader** :
  - This loads command plugins into the router from the current assembly and a shared directory. It enables dynamic extensibility by supporting plugin architecture.

This structure allows you to dynamically deploy and run command plugins like AWS Lambda functions, making your system highly flexible and customizable.

