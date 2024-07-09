cd bin

Write-Host
Write-Host Building project...
dotnet build ..\repro.sln

Write-Host
Write-Host Running with Assembly.Load...
.\PluginHost.exe AssemblyLoad "RemoteWebAdministrationPlugin.ListRemoteIisSitesPlugin, RemoteWebAdministrationPlugin" $env:ComputerName

Write-Host
Write-Host Running with AssemblyLoadContext...
.\PluginHost.exe AssemblyLoadContext "RemoteWebAdministrationPlugin.ListRemoteIisSitesPlugin, RemoteWebAdministrationPlugin" $env:ComputerName

Write-Host
Write-Host Rebuilding project with ProjectReference...
dotnet build ..\repro.sln /p:AllowDirectReferences=true

Write-Host
Write-Host Running with Assembly.Load...
.\PluginHost.exe AssemblyLoad "RemoteWebAdministrationPlugin.ListRemoteIisSitesPlugin, RemoteWebAdministrationPlugin" $env:ComputerName

Write-Host
Write-Host Running with AssemblyLoadContext...
.\PluginHost.exe AssemblyLoadContext "RemoteWebAdministrationPlugin.ListRemoteIisSitesPlugin, RemoteWebAdministrationPlugin" $env:ComputerName

Write-Host
Write-Host Running with direct Type.GetType...
.\PluginHost.exe Direct "RemoteWebAdministrationPlugin.ListRemoteIisSitesPlugin, RemoteWebAdministrationPlugin" $env:ComputerName