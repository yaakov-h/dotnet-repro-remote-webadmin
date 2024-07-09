using Microsoft.Web.Administration;
using PluginInterfaces;

namespace RemoteWebAdministrationPlugin;

public class ListRemoteIisSitesPlugin : IPlugin
{
    public void DoSomething(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("This plugin requires a <server name> argument.");
            return;
        }

        var serverName = args[0];

        using var sm = ServerManager.OpenRemote(serverName);
        Console.WriteLine($"Server '{serverName}' has {sm.Sites.Count} sites hosted in IIS.");
    }
}
