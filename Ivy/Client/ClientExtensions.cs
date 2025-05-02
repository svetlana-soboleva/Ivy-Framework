using Ivy.Core;
using Ivy.Shared;

namespace Ivy.Client;

public static class ClientExtensions
{
    public static void CopyToClipboard(this IClientProvider client, string content)
    {
        client.Sender.Send("CopyToClipboard", content);
    }

    public static void OpenUrl(this IClientProvider client, string url)
    {
        client.Sender.Send("OpenUrl", url);
    }
    
    public static void Redirect(this IClientProvider client, string url)
    {
        client.Sender.Send("Redirect", url);
    }
    
    public static void SetJwt(this IClientProvider client, string? jwt)
    {
        client.Sender.Send("SetJwt", jwt);
    }
    
    public static void SetTheme(this IClientProvider client, Theme theme)
    {
        client.Sender.Send("SetTheme", theme.ToString());
    }
    
    internal static void SetChatPanelUrl(this IClientProvider client, string url)
    {
        client.Sender.Send("$SetChatPanelUrl", url);
    }
}