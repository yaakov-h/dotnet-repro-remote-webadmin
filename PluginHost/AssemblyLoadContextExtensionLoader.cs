using System.Reflection;
using System.Runtime.Loader;

namespace PluginHost;

sealed class AssemblyLoadContextExtensionLoader
{
    public AssemblyLoadContextExtensionLoader(string extensionDirectory)
    {
        this.extensionDirectory = extensionDirectory ?? throw new ArgumentNullException(nameof(extensionDirectory));
    }

    readonly string extensionDirectory;

    public Type GetExtensionType(string fullyQualifiedTypeName)
    {
        var parts = fullyQualifiedTypeName.Split(',', count: 2);
        if (parts.Length != 2)
        {
            throw new ArgumentException($"Unexpected type name '{fullyQualifiedTypeName}', expected 'Type,Assembly' format.");
        }

        var typeName = parts[0];
        var assemblyNameText = parts[1];
        var assemblyName = new AssemblyName(assemblyNameText);

        var assumedAssemblyPath = Path.Combine(extensionDirectory, assemblyName.Name + ".dll");
        var context = new PluginLoadContext(assumedAssemblyPath);
        context.Resolving += OnContextResolving;
        var assembly = context.LoadFromAssemblyName(assemblyName);
        var type = assembly.GetType(typeName, throwOnError: true, ignoreCase: true)!;
        return type;
    }

    Assembly? OnContextResolving(AssemblyLoadContext context, AssemblyName assembly)
    {
        Console.Error.WriteLine($"{nameof(AssemblyLoadContextExtensionLoader)}.{nameof(OnContextResolving)}: {context.Name} -> {assembly.Name},Version={assembly.Version}");
        var assumedAssemblyPath = Path.Combine(extensionDirectory, assembly.Name + ".dll");
        if (File.Exists(assumedAssemblyPath))
        {
            Console.Error.WriteLine($"{nameof(AssemblyLoadContextExtensionLoader)}.{nameof(OnContextResolving)}: {context.Name} -> {assembly.Name},Version={assembly.Version} = {assumedAssemblyPath}");
            return context.LoadFromAssemblyPath(assumedAssemblyPath);
        }

        Console.Error.WriteLine($"{nameof(AssemblyLoadContextExtensionLoader)}.{nameof(OnContextResolving)}: {context.Name} -> {assembly.Name},Version={assembly.Version} = null");

        return null;
    }
}
