using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Defines the visual style variant for tabbed interfaces, controlling how tabs are displayed
/// and how the content area is emphasized within the overall layout.
/// </summary>
public enum TabsVariant
{
    /// <summary>Traditional tab interface with clickable tab buttons and an underline indicator for the active tab.</summary>
    Tabs,
    /// <summary>Content-focused variant with subtle tab indicators, emphasizing the displayed content over the tab controls.</summary>
    Content
}

/// <summary>
/// Represents a tabbed layout widget that creates a tabbed interface allowing users to switch
/// between different content sections. This widget supports both traditional tabs and content-based
/// variants, with features such as closable tabs, badges, icons, and drag-and-drop reordering.
/// 
/// The TabsLayout widget provides comprehensive tab management capabilities including event handling
/// for tab selection, closing, refreshing, and reordering. It automatically sizes itself to fill
/// available space and removes parent padding for optimal tabbed interface presentation.
/// </summary>
public record TabsLayout : WidgetBase<TabsLayout>
{
    /// <summary>
    /// Initializes a new instance of the TabsLayout class with the specified event handlers,
    /// selected tab index, and tab content. The layout will automatically configure itself
    /// for optimal tabbed interface presentation with full dimensions and parent padding removal.
    /// </summary>
    /// <param name="onSelect">Optional event handler for tab selection events. Called when a user
    /// clicks on a tab, receiving the index of the selected tab for navigation or state updates.</param>
    /// <param name="onClose">Optional event handler for tab close events. Called when a user
    /// closes a tab, receiving the index of the closed tab for cleanup or state management.</param>
    /// <param name="onRefresh">Optional event handler for tab refresh events. Called when a user
    /// clicks a refresh button on a tab, receiving the index of the tab to refresh.</param>
    /// <param name="onReorder">Optional event handler for tab reordering events. Called when a user
    /// drags and drops tabs to reorder them, receiving an array of the new tab order indices.</param>
    /// <param name="selectedIndex">The index of the initially selected tab. When null, no tab
    /// is pre-selected and the user must make an initial selection.</param>
    /// <param name="tabs">Variable number of Tab objects defining the tab structure, content,
    /// and visual properties for the tabbed interface.</param>
    public TabsLayout(Action<Event<TabsLayout, int>>? onSelect, Action<Event<TabsLayout, int>>? onClose, Action<Event<TabsLayout, int>>? onRefresh, Action<Event<TabsLayout, int[]>>? onReorder, int? selectedIndex, params Tab[] tabs) : base(tabs.Cast<object>().ToArray())
    {
        OnSelect = onSelect;
        OnClose = onClose;
        OnRefresh = onRefresh;
        OnReorder = onReorder;
        SelectedIndex = selectedIndex;
        Width = Size.Full();
        Height = Size.Full();
        RemoveParentPadding = true;
    }

    /// <summary>
    /// Gets or sets the index of the currently selected tab.
    /// This property controls which tab is currently active and displaying its content.
    /// When null, no tab is selected and the interface may show a default state or
    /// require user selection before displaying content.
    /// Default is the value provided in the constructor.
    /// </summary>
    [Prop] public int? SelectedIndex { get; set; }

    /// <summary>
    /// Gets or sets the visual style variant for the tabbed interface.
    /// This property controls how tabs are displayed and how the content area is emphasized
    /// within the overall layout, affecting the user experience and visual hierarchy.
    /// Default is <see cref="TabsVariant.Content"/>.
    /// </summary>
    [Prop] public TabsVariant Variant { get; set; } = TabsVariant.Content;

    /// <summary>
    /// Gets or sets whether to remove any padding inherited from parent containers.
    /// When true, the tabs layout will ignore parent padding and extend to the full
    /// available space, providing optimal tabbed interface presentation.
    /// 
    /// This property is automatically set to true during construction to ensure
    /// tabs can utilize the full available space without parent padding constraints.
    /// Default is true.
    /// </summary>
    [Prop] public bool RemoveParentPadding { get; set; }

    /// <summary>
    /// Gets or sets the padding around the tabs layout container.
    /// This property controls the internal spacing between the tabs content and the
    /// layout boundaries, providing visual separation and breathing room.
    /// 
    /// When null, no padding is applied. When specified using a <see cref="Thickness"/>
    /// object, you can control padding on different sides independently for precise layout control.
    /// Default is 4 units of padding on all sides.
    /// </summary>
    [Prop] public Thickness? Padding { get; set; } = new Thickness(4);

    /// <summary>
    /// Gets or sets the event handler for tab selection events.
    /// This event is triggered when a user clicks on a tab, providing the index
    /// of the selected tab for navigation, state updates, or other custom actions.
    /// Default is the value provided in the constructor.
    /// </summary>
    [Event] public Action<Event<TabsLayout, int>>? OnSelect { get; set; }

    /// <summary>
    /// Gets or sets the event handler for tab close events.
    /// This event is triggered when a user closes a tab, providing the index
    /// of the closed tab for cleanup, state management, or other custom actions.
    /// Default is the value provided in the constructor.
    /// </summary>
    [Event] public Action<Event<TabsLayout, int>>? OnClose { get; set; }

    /// <summary>
    /// Gets or sets the event handler for tab refresh events.
    /// This event is triggered when a user clicks a refresh button on a tab,
    /// providing the index of the tab to refresh for data reloading or other actions.
    /// Default is the value provided in the constructor.
    /// </summary>
    [Event] public Action<Event<TabsLayout, int>>? OnRefresh { get; set; }

    /// <summary>
    /// Gets or sets the event handler for tab reordering events.
    /// This event is triggered when a user drags and drops tabs to reorder them,
    /// providing an array of the new tab order indices for state updates or persistence.
    /// Default is the value provided in the constructor.
    /// </summary>
    [Event] public Action<Event<TabsLayout, int[]>>? OnReorder { get; set; }
}

/// <summary>
/// Provides extension methods for the TabsLayout widget that enable a fluent API for
/// configuring tabbed interface appearance and behavior. These methods allow you to easily
/// set visual variants, control padding, and manage parent padding removal for optimal
/// tabbed interface presentation.
/// </summary>
public static class TabsLayoutExtensions
{
    /// <summary>
    /// Sets the visual style variant for the tabbed interface.
    /// This method allows you to change the tab display style after creation, switching
    /// between traditional tabs and content-focused variants.
    /// </summary>
    /// <param name="tabsLayout">The TabsLayout to configure.</param>
    /// <param name="variant">The visual style variant to apply to the tabs.</param>
    /// <returns>A new TabsLayout instance with the updated variant setting.</returns>
    public static TabsLayout Variant(this TabsLayout tabsLayout, TabsVariant variant)
    {
        return tabsLayout with { Variant = variant };
    }

    /// <summary>
    /// Sets whether to remove any padding inherited from parent containers.
    /// This method allows you to control whether the tabs layout extends to the full
    /// available space or respects parent padding constraints.
    /// </summary>
    /// <param name="tabsLayout">The TabsLayout to configure.</param>
    /// <param name="removeParentPadding">Whether to remove parent padding. Default is true.</param>
    /// <returns>A new TabsLayout instance with the updated parent padding removal setting.</returns>
    public static TabsLayout RemoveParentPadding(this TabsLayout tabsLayout, bool removeParentPadding = true)
    {
        return tabsLayout with { RemoveParentPadding = removeParentPadding };
    }

    /// <summary>
    /// Sets the padding around the tabs layout container using a Thickness object.
    /// This method allows fine-grained control over padding on different sides
    /// for precise layout control and visual spacing.
    /// </summary>
    /// <param name="tabsLayout">The TabsLayout to configure.</param>
    /// <param name="padding">The Thickness object containing left, top, right, and bottom padding values.</param>
    /// <returns>A new TabsLayout instance with the updated padding setting.</returns>
    public static TabsLayout Padding(this TabsLayout tabsLayout, Thickness? padding)
    {
        return tabsLayout with { Padding = padding };
    }

    /// <summary>
    /// Sets uniform padding around the tabs layout container on all sides.
    /// This convenience method creates equal padding on all sides for consistent
    /// spacing around the tabbed interface.
    /// </summary>
    /// <param name="tabsLayout">The TabsLayout to configure.</param>
    /// <param name="padding">The uniform padding value to apply to all sides in units.</param>
    /// <returns>A new TabsLayout instance with the updated uniform padding setting.</returns>
    public static TabsLayout Padding(this TabsLayout tabsLayout, int padding)
    {
        return tabsLayout with { Padding = new Thickness(padding) };
    }

    /// <summary>
    /// Sets different padding values for vertical and horizontal directions.
    /// This convenience method allows you to specify different spacing for top/bottom
    /// versus left/right sides of the tabbed interface.
    /// </summary>
    /// <param name="tabsLayout">The TabsLayout to configure.</param>
    /// <param name="verticalPadding">The padding value for top and bottom sides in units.</param>
    /// <param name="horizontalPadding">The padding value for left and right sides in units.</param>
    /// <returns>A new TabsLayout instance with the updated directional padding setting.</returns>
    public static TabsLayout Padding(this TabsLayout tabsLayout, int verticalPadding, int horizontalPadding)
    {
        return tabsLayout with { Padding = new Thickness(horizontalPadding, verticalPadding) };
    }

    /// <summary>
    /// Sets specific padding values for each side of the tabs layout container.
    /// This method provides the most precise control over padding, allowing you to
    /// specify different values for left, top, right, and bottom sides independently.
    /// </summary>
    /// <param name="tabsLayout">The TabsLayout to configure.</param>
    /// <param name="left">The padding value for the left side in units.</param>
    /// <param name="top">The padding value for the top side in units.</param>
    /// <param name="right">The padding value for the right side in units.</param>
    /// <param name="bottom">The padding value for the bottom side in units.</param>
    /// <returns>A new TabsLayout instance with the updated specific padding setting.</returns>
    public static TabsLayout Padding(this TabsLayout tabsLayout, int left, int top, int right, int bottom)
    {
        return tabsLayout with { Padding = new Thickness(left, top, right, bottom) };
    }
}

/// <summary>
/// Represents an individual tab within a TabsLayout that contains a title, optional icon,
/// optional badge, and content. Each tab represents a distinct section or view that users
/// can switch between in the tabbed interface.
/// 
/// The Tab widget provides visual customization options including icons and badges to
/// enhance user experience and provide visual context for different content sections.
/// </summary>
public record Tab : WidgetBase<Tab>
{
    /// <summary>
    /// Initializes a new instance of the Tab class with the specified title and optional content.
    /// The tab will display the title in the tab header and can contain any combination
    /// of widgets or content elements in the tab body.
    /// </summary>
    /// <param name="title">The text label displayed in the tab header for user identification
    /// and navigation. This should be descriptive and concise for optimal user experience.</param>
    /// <param name="content">Optional content to display in the tab body when the tab is active.
    /// When null, the tab header is created but no content is displayed until content is added later.</param>
    public Tab(string title, object? content = null) : base(content != null ? [content] : [])
    {
        Title = title;
    }

    /// <summary>
    /// Gets or sets the text label displayed in the tab header.
    /// This property provides the primary identification for the tab and should be
    /// descriptive and concise to help users understand what content the tab contains.
    /// </summary>
    [Prop] public string Title { get; set; }

    /// <summary>
    /// Gets or sets the optional icon displayed alongside the tab title.
    /// This property allows you to add visual context to tabs, making them more
    /// recognizable and improving the overall user experience of the tabbed interface.
    /// Default is null (no icon displayed).
    /// </summary>
    [Prop] public Icons? Icon { get; set; }

    /// <summary>
    /// Gets or sets the optional badge text displayed on the tab.
    /// This property allows you to show additional information such as counts,
    /// status indicators, or notifications directly on the tab header.
    /// Default is null (no badge displayed).
    /// </summary>
    [Prop] public string? Badge { get; set; }
}

/// <summary>
/// Provides extension methods for the Tab widget that enable a fluent API for
/// customizing tab appearance and visual elements. These methods allow you to easily
/// add icons and badges to tabs for enhanced visual representation and user experience.
/// </summary>
public static class TabExtensions
{
    /// <summary>
    /// Sets an icon to be displayed alongside the tab title.
    /// This method allows you to add visual context to tabs, making them more
    /// recognizable and improving the overall user experience of the tabbed interface.
    /// </summary>
    /// <param name="tab">The Tab to configure.</param>
    /// <param name="icon">The icon to display alongside the tab title, or null to remove the icon.</param>
    /// <returns>A new Tab instance with the updated icon setting.</returns>
    public static Tab Icon(this Tab tab, Icons? icon)
    {
        return tab with { Icon = icon };
    }

    /// <summary>
    /// Sets a badge to be displayed on the tab.
    /// This method allows you to show additional information such as counts,
    /// status indicators, or notifications directly on the tab header.
    /// </summary>
    /// <param name="tab">The Tab to configure.</param>
    /// <param name="badge">The badge text to display on the tab, or null to remove the badge.</param>
    /// <returns>A new Tab instance with the updated badge setting.</returns>
    public static Tab Badge(this Tab tab, string badge)
    {
        return tab with { Badge = badge };
    }
}
