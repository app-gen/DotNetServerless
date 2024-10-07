using Microsoft.Extensions.Logging;
using System.Reflection;
namespace LambdaFramework.Common;

public class PluginLoader
{
    private readonly ILogger<PluginLoader> _logger;
    private readonly ICommandRouter _router;
    private readonly string _pluginPath;


    public void LoadAllCommands(string pluginFolderPath, ICommandRouter router)
    {
        var commands = new List<ICommand>();

        // Get the current assembly
        var currentAssembly = Assembly.GetExecutingAssembly();
        var assemblies = new List<Assembly> { currentAssembly };
        if (!string.IsNullOrEmpty(pluginFolderPath))
        {
            // Load assemblies from the plugin folder
            if (Directory.Exists(pluginFolderPath))
            {
                var pluginFiles = Directory.GetFiles(pluginFolderPath, "*.dll");
                foreach (var file in pluginFiles)
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(file);
                        assemblies.Add(assembly);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading assembly {file}: {ex.Message}");
                    }
                }
            }
        }
        // Find and instantiate ICommand implementations
        foreach (Assembly assembly in assemblies)
        {
            var commandTypes = assembly.GetTypes()
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in commandTypes)
            {
                try
                {
                    var command = (ICommand)Activator.CreateInstance(type);
                    commands.Add(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating instance of {type.FullName}: {ex.Message}");
                }
            }
        }

        //return commands;
    }


    public static void LoadPluginsFromAssembly(Assembly pluginAssembly, ICommandRouter router)
    {
        //var pluginAssembly = Assembly.LoadFrom(pluginPath);

        var commandTypes = pluginAssembly.GetTypes()
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var commandType in commandTypes)
        {
            var commandAttribute = commandType.GetCustomAttribute<CommandAttribute>();
            if (commandAttribute != null)
            {
                var commandInfo = new CommandInfo
                {
                    CommandType = commandType,
                    TenantName = commandAttribute.TenantName,
                    CommandName = commandAttribute.CommandName,
                    ActionName = commandAttribute.ActionName,
                    Version = commandAttribute.Version
                };
                router.RegisterCommand(commandInfo);
            }
        }
    }


    public static void LoadPluginsFromPath(string pluginPath, ICommandRouter router)
    {
        var pluginAssembly = Assembly.LoadFrom(pluginPath);
        if (pluginAssembly != null)
        {
            LoadPluginsFromAssembly(pluginAssembly, router);
        }
    }

    public static void LoadCommonPlugins( ICommandRouter router)
    {
        var pluginAssembly = Assembly.GetExecutingAssembly();
        if (pluginAssembly != null)
        {
            LoadPluginsFromAssembly(pluginAssembly, router);
        }
    }



    public PluginLoader(ILogger<PluginLoader> logger, ICommandRouter router, string pluginPath)
    {
        _logger = logger;
        _router = router;
        _pluginPath = pluginPath;
    }

    public void LoadPlugins()
    {
        _logger.LogInformation("Starting to load plugins from {PluginPath}", _pluginPath);
        _router.ClearCommands();

        var pluginAssembly = Assembly.LoadFrom(_pluginPath);

        // Load commands
        var commandTypes = pluginAssembly.GetTypes()
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var commandType in commandTypes)
        {
            var commandAttribute = commandType.GetCustomAttribute<CommandAttribute>();
            if (commandAttribute != null)
            {
                var commandInfo = new CommandInfo
                {
                    CommandType = commandType,
                    TenantName = commandAttribute.TenantName,
                    CommandName = commandAttribute.CommandName,
                    ActionName = commandAttribute.ActionName,
                    Version = commandAttribute.Version
                };
                _router.RegisterCommand(commandInfo);
                _logger.LogInformation("Registered command: {TenantName}:{CommandName}:{ActionName}:{Version}",
                    commandInfo.TenantName, commandInfo.CommandName, commandInfo.ActionName, commandInfo.Version);
            }
        }

        // Load hooks
        var hookTypes = pluginAssembly.GetTypes()
            .Where(t => typeof(IHook).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var hookType in hookTypes)
        {
            var hookAttribute = hookType.GetCustomAttribute<HookAttribute>();
            if (hookAttribute != null)
            {
                var hookInfo = new HookInfo
                {
                    HookType = hookType,
                    HookName = hookAttribute.HookName,
                    Type = hookAttribute.Type
                };
                _router.RegisterHook(hookInfo);
                _logger.LogInformation("Registered {HookType} hook: {HookName}",
                    hookAttribute.Type, hookAttribute.HookName);
            }
        }

        _logger.LogInformation("Finished loading plugins");
    }
}



