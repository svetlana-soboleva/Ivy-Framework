using Ivy.Core;

namespace Ivy.Widgets.Internal;

/// <summary>
/// Represents a sidebar news widget that displays RSS or news feed content in a sidebar layout.
/// This widget fetches and renders news articles, updates, or announcements from a specified feed URL,
/// making it ideal for displaying recent news, blog posts, or updates within application sidebars.
/// </summary>
public record SidebarNews : WidgetBase<SidebarNews>
{
    /// <summary>
    /// Initializes a new instance of the SidebarNews class with the specified feed URL.
    /// The widget will attempt to fetch and display content from the provided RSS or news feed.
    /// </summary>
    /// <param name="feedUrl">The URL of the RSS feed or news source to display in the sidebar.</param>
    public SidebarNews(string feedUrl) : base()
    {
        FeedUrl = feedUrl;
    }

    /// <summary>
    /// Gets or sets the URL of the RSS feed or news source to display in the sidebar.
    /// This property controls which news source the widget fetches content from, allowing you
    /// to display different types of news, updates, or announcements based on your application's needs.
    /// 
    /// The feed URL should point to a valid RSS feed, Atom feed, or other supported news format
    /// that the widget can parse and display. When null, no content will be fetched or displayed.
    /// Default is the URL provided in the constructor.
    /// </summary>
    [Prop] public string? FeedUrl { get; set; }
}