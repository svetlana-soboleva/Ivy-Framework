using Microsoft.AspNetCore.SignalR;

namespace Ivy;

public interface IClientNotifier
{
    Task NotifyClientAsync(string connectionId, string method, object message);
}

public class ClientNotifier(IHubContext<AppHub> hubContext) : IClientNotifier
{
    public Task NotifyClientAsync(string connectionId, string method, object message)
    {
        return hubContext.Clients.Client(connectionId).SendAsync(method, message);
    }
}