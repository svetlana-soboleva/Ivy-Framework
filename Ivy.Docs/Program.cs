using Ivy.Chrome;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
var server = new IvyServer();
server.AddAppsFromAssembly();
server.UseHotReload();

var chromeSettings = new ChromeSettings()
    .Header(
        Layout.Vertical().Padding(2) | new IvyLogo()
    )
    .DefaultApp<Ivy.Docs.Apps.Onboarding.GettingStarted.IntroductionApp>()
    .UsePages();
server.UseChrome(() => new DefaultSidebarChrome(chromeSettings));

await server.RunAsync();