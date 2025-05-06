CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
var server = new IvyServer();
#if !DEBUG
server.UseHttpRedirection();
#endif
#if DEBUG
server.UseHotReload();
#endif
server.AddAppsFromAssembly();
server.UseHotReload();
//server.UseChrome(); We only have one app for new so no chrome is needed
await server.RunAsync();