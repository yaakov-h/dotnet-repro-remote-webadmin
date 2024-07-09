cd bin

dotnet build ..\repro.sln
.\PluginHost.exe AssemblyLoad "RemoteWebAdministrationPlugin.ListRemoteIisSitesPlugin, RemoteWebAdministrationPlugin" $env:ComputerName
.\PluginHost.exe AssemblyLoadContext "RemoteWebAdministrationPlugin.ListRemoteIisSitesPlugin, RemoteWebAdministrationPlugin" $env:ComputerName

dotnet build ..\repro.sln /p:AllowDirectReferences=true
.\PluginHost.exe AssemblyLoad "RemoteWebAdministrationPlugin.ListRemoteIisSitesPlugin, RemoteWebAdministrationPlugin" $env:ComputerName
.\PluginHost.exe AssemblyLoadContext "RemoteWebAdministrationPlugin.ListRemoteIisSitesPlugin, RemoteWebAdministrationPlugin" $env:ComputerName
.\PluginHost.exe Direct "RemoteWebAdministrationPlugin.ListRemoteIisSitesPlugin, RemoteWebAdministrationPlugin" $env:ComputerName