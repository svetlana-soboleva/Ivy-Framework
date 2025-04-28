CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
var server = new IvyServer();
server.UseHotReload();
server.AddAppsFromAssembly();
server.UseChrome();
server.Services.AddSingleton<SampleDbContextFactory>();
await server.RunAsync();