using System.Reflection;
using System.Runtime.Loader;

namespace PluginHost;

sealed class PluginLoadContext : AssemblyLoadContext
{
    public PluginLoadContext(string extensionDirectory)
    {
        resolver = new AssemblyDependencyResolver(extensionDirectory);
    }

    readonly AssemblyDependencyResolver resolver;

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // Types from the plugin interfaces assembly have to be common across multiple AssemblyLoadContexts so that they
        // match type signatures (e.g. interface implementations).
        // Ideally, plugins should not ship their own copy of this assembly, but they need it in order to unit-test themselves.
        if (string.Equals(assemblyName.Name, "PluginInterfaces", StringComparison.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine($"{nameof(PluginLoadContext)}.{nameof(Load)}: {assemblyName.Name} loading from default context.");
            return Default.LoadFromAssemblyName(assemblyName);
        }

        var path = resolver.ResolveAssemblyToPath(assemblyName);
        if (path is null)
        {
            Console.Error.WriteLine($"{nameof(PluginLoadContext)}.{nameof(Load)}: {assemblyName.Name} failed to resolve to path");
            return null;
        }

        Console.Error.WriteLine($"{nameof(PluginLoadContext)}.{nameof(Load)}: Loading {assemblyName.Name} from {path}");
        return LoadFromAssemblyPath(path);
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var path = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (path is null)
        {
            Console.Error.WriteLine($"{nameof(PluginLoadContext)}.{nameof(LoadUnmanagedDll)}: {unmanagedDllName} failed to resolve to path");
            return IntPtr.Zero;
        }

        Console.Error.WriteLine($"{nameof(PluginLoadContext)}.{nameof(LoadUnmanagedDll)}: Loading {unmanagedDllName} from {path}");
        return LoadUnmanagedDllFromPath(path);
    }
}
