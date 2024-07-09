using PluginHost;
using PluginInterfaces;
using System.Reflection;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: PluginHost.exe <mode> <plugin type> [plugin args...]");
    Console.Error.WriteLine("       mode: 'AssemblyLoad' or 'AssemblyLoadContext'");
    Console.Error.WriteLine("       plugin type: Fully qualified type name of the plugin to load");
    return -1;
}

var mode = args[0];
var pluginTypeName = args[1];

Type pluginType;

if (string.Equals(mode, "AssemblyLoad", StringComparison.OrdinalIgnoreCase))
{
    AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
    {
        Console.Error.WriteLine($"AppDomain.CurrentDomain.AssemblyResolve: {e.Name}");
        return null;
    };

    var typeNameParts = pluginTypeName.Split(',', count: 2);
    var typeName = typeNameParts[0].Trim();
    var assemblyNameFromType = typeNameParts[1].Trim();
    var assemblyName = new AssemblyName(assemblyNameFromType);
    var assemblyPath = Path.Combine("plugins", assemblyName.Name + ".dll");

    var pluginAssembly = Assembly.LoadFrom(assemblyPath);
    pluginType = pluginAssembly.GetType(typeName, throwOnError: true, ignoreCase: true)!;
}
else if (string.Equals(mode, "AssemblyLoadContext", StringComparison.OrdinalIgnoreCase))
{
    var context = new AssemblyLoadContextExtensionLoader("plugins");
    pluginType = context.GetExtensionType(pluginTypeName);
}
#if ALLOW_DIRECT_REFERENCES
else if (string.Equals(mode, "Direct", StringComparison.OrdinalIgnoreCase))
{
    pluginType = Type.GetType(pluginTypeName, throwOnError: true, ignoreCase: true)!;
}
#endif
else
{
    Console.Error.WriteLine("Invalid 'mode' argument.");
    return -2;
}

var plugin = (IPlugin)Activator.CreateInstance(pluginType)!;
plugin.DoSomething(args[2..]);



return 0;