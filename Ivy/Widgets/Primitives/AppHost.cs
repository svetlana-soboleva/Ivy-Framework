using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

public record AppHost : WidgetBase<AppHost>
{
    public AppHost(string appId, string? appArgs, string? parentId, long? refreshToken) 
    {
        ParentId = parentId ?? string.Empty;
        AppId = appId;
        AppArgs = appArgs;
        RefreshToken = refreshToken;
    }
    [Prop] public string AppId { get; set; }
    [Prop] public string? AppArgs { get; set; }
    [Prop] public string? ParentId { get; set; }
    [Prop] public long? RefreshToken { get; set; }
}