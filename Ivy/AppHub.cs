using System.Collections.Concurrent;
using System.Text.Json.Nodes;
using Ivy.Apps;
using Ivy.Auth;
using Ivy.Core;
using Ivy.Helpers;
using Ivy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ivy;

public class AppSessionStore
{
    public readonly ConcurrentDictionary<string, AppSession> Sessions = new();
}

public class AppHub(
    IvyServer server, 
    IClientNotifier clientNotifier, 
    IContentBuilder contentBuilder,
    AppSessionStore sessionStore,
    ILogger<AppHub> logger) : Hub
{
    public static string GetAppId(IvyServer ivyServer, HttpContext httpContext)
    {
        string? appId = ivyServer.DefaultAppId;
            
        if(httpContext!.Request.Query.ContainsKey("appId"))
        {
            appId = httpContext!.Request.Query["appId"].ToString();
        }
        
        if (string.IsNullOrEmpty(appId))
        {
            appId = ivyServer.DefaultAppId ?? ivyServer.AppRepository.GetAppOrDefault(null).Id;
        }

        return appId;
    }

    public AppArgs GetAppArgs(string appId, HttpContext httpContext)
    {
        string? appArgs = null;
        if(httpContext!.Request.Query.ContainsKey("appArgs"))
        {
            appArgs = httpContext!.Request.Query["appArgs"].ToString();
        }
        return new AppArgs(appId, appArgs ?? server.Args?.Args);
    }
    
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext()!;
        var appId = GetAppId(server, httpContext);
        var appArgs = GetAppArgs(appId, httpContext);
        var appDescriptor = server.GetApp(appId);

        logger.LogInformation($"Connected: {Context.ConnectionId} [{appId}]");
        
        var clientProvider = new ClientProvider(new ClientSender(clientNotifier, Context.ConnectionId));
        
        var appServices = new ServiceCollection();
        appServices.AddSingleton(typeof(IContentBuilder), contentBuilder);
        appServices.AddSingleton(typeof(IAppRepository), server.AppRepository);
        appServices.AddSingleton(typeof(IDownloadService), new DownloadService(Context.ConnectionId));
        appServices.AddSingleton(typeof(IClientProvider), clientProvider);
        appServices.AddSingleton(appDescriptor);
        appServices.AddSingleton(appArgs);
        
        var isAuthProtected = server.AuthProviderType != null;
        if (isAuthProtected)
        {
            var authProvider = server.Services.BuildServiceProvider().GetService<IAuthProvider>() ?? throw new Exception("IAuthProvider not found");
            var jwt = httpContext.Request.Cookies["jwt"];
            if(!string.IsNullOrEmpty(jwt))
            {
                appId = AppIds.Auth;
            }
            else
            {
                var validJwt = await authProvider.ValidateJwtAsync(jwt!);
                if (!validJwt)
                {
                    appId = AppIds.Auth;
                }
            }
            appServices.AddSingleton<IAuthService>(s => new AuthService(authProvider, jwt));
        }
        
        var serviceProvider = new CompositeServiceProvider(appServices, server.Services); 
        
        var app = appDescriptor.CreateApp();
        
        var widgetTree = new WidgetTree(app, contentBuilder, serviceProvider);
        
        var appState = new AppSession
        {
            AppId = appId,
            AppDescriptor = appDescriptor,
            App = app,
            ConnectionId = Context.ConnectionId,
            WidgetTree = widgetTree,
            ContentBuilder = contentBuilder,
            AppServices = serviceProvider
        };

        async void OnWidgetTreeChanged(WidgetTreeChanged[] changes)
        {
            logger.LogDebug($"> Update");
            await clientNotifier.NotifyClientAsync(appState.ConnectionId, "Update", changes);
        }

        appState.TrackDisposable(widgetTree.Subscribe(OnWidgetTreeChanged));
        
        sessionStore.Sessions[Context.ConnectionId] = appState;
        
        await base.OnConnectedAsync();

        try
        {
            await widgetTree.BuildAsync();
            logger.LogInformation($"Refresh: {Context.ConnectionId} [{appId}]");
            await Clients.Caller.SendAsync("Refresh", new
            {
                Widgets = widgetTree.GetWidgets().Serialize(), 
                appDescriptor.RemoveIvyBranding
            });
        }
        catch (Exception e)
        {
            var tree = new WidgetTree(new ErrorView(e), contentBuilder, serviceProvider);
            await tree.BuildAsync();
            await Clients.Caller.SendAsync("Refresh", new
            {
                Widgets = tree.GetWidgets().Serialize(), 
                appDescriptor.RemoveIvyBranding
            });
        }
    }

    public override Task OnDisconnectedAsync(System.Exception? exception)
    {
        if (sessionStore.Sessions.TryRemove(Context.ConnectionId, out var appState))
        {
            appState.Dispose();
        }
        
        return base.OnDisconnectedAsync(exception);
    }
    
    public void HotReload()
    {
        if (sessionStore.Sessions.TryGetValue(Context.ConnectionId, out var appState))
        {
            logger.LogInformation($"HotReload: {Context.ConnectionId} [{appState.AppId}]");
            try
            {
                appState.WidgetTree.HotReload();
            }
            catch (Exception e)
            {
                logger.LogError(e, "HotReload failed.");
            }
        }
        else
        {
            logger.LogWarning($"HotReload: {Context.ConnectionId} [Not Found]");
        }
    }
    
    public void Event(string eventName, string widgetId, JsonArray? args)
    {
        try
        {
            logger.LogInformation($"Event: {eventName} {widgetId} {args}");
            var appState = sessionStore.Sessions[Context.ConnectionId];

            if (!appState.WidgetTree.TriggerEvent(widgetId, eventName, args ?? new JsonArray()))
            {
                logger.LogWarning($"Event '{eventName}' for Widget '{widgetId}' not found.");
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Event trigger failed.");
        }
    }
}

public class ClientSender(IClientNotifier clientNotifier, string connectionId) : IClientSender
{
    public void Send(string method, object data)
    {
        _ = clientNotifier.NotifyClientAsync(connectionId, method, data);
    }
}

public class ClientProvider(IClientSender sender) : IClientProvider
{
    public IClientSender Sender { get; set; } = sender;
}
