using Ivy.Apps;

namespace Ivy.Chrome;

public enum ChromeNavigation
{
    Tabs,
    Pages
}

public record ChromeSettings
{
    public object? Header { get; init; }
    public object? Footer { get; init; }
    public string? DefaultAppId { get; init; }
    public ChromeNavigation Navigation { get; init; }

    public static ChromeSettings Default() => new()
    {
        Navigation = ChromeNavigation.Tabs
    };
}

public static class ChromeSettingsExtensions
{
    public static ChromeSettings Header(this ChromeSettings settings, object? header) => settings with { Header = header };
    public static ChromeSettings Footer(this ChromeSettings settings, object? footer) => settings with { Footer = footer };
    public static ChromeSettings DefaultAppId(this ChromeSettings settings, string? defaultAppId) => settings with { DefaultAppId = defaultAppId };
    public static ChromeSettings DefaultApp<T>(this ChromeSettings settings)
    {
        var type = typeof(T);
        var descriptor = AppHelpers.GetApp(type);
        return settings with { DefaultAppId = descriptor.Id };
    }
    public static ChromeSettings Navigation(this ChromeSettings settings, ChromeNavigation navigation) => settings with { Navigation = navigation };
    public static ChromeSettings UseTabs(this ChromeSettings settings) => settings with { Navigation = ChromeNavigation.Tabs };
    public static ChromeSettings UsePages(this ChromeSettings settings) => settings with { Navigation = ChromeNavigation.Pages };
}