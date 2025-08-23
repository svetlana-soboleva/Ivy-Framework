

// ReSharper disable once CheckNamespace
namespace Ivy.Charts;

/// <summary>
/// Represents a tooltip configuration for charts, providing control over the interactive information display
/// that appears when users hover over chart elements. Tooltips enhance user experience by showing detailed
/// data values, labels, and context information without cluttering the main chart visualization.
/// </summary>
public record Tooltip
{
    /// <summary>
    /// Initializes a new instance of the Tooltip class with default values.
    /// The default configuration provides a non-animated tooltip with standard behavior.
    /// </summary>
    public Tooltip()
    {

    }

    /// <summary>
    /// Gets or sets whether the tooltip should be animated when appearing, disappearing, or updating.
    /// Animation can make tooltips more engaging and help draw attention to data changes.
    /// Default is false (no animation).
    /// </summary>
    public bool Animated { get; set; } = false;

}

/// <summary>
/// Extension methods for the Tooltip class that provide a fluent API for easy configuration.
/// These methods allow you to chain multiple configuration calls for better readability and maintainability.
/// Each method returns a new Tooltip instance with the updated configuration, following the immutable pattern.
/// </summary>
public static class TooltipExtensions
{
    /// <summary>
    /// Sets whether the tooltip should be animated when appearing, disappearing, or updating.
    /// Animation can make tooltips more engaging and help draw attention to data changes.
    /// </summary>
    /// <param name="tooltip">The Tooltip to configure.</param>
    /// <param name="animated">True to enable animation, false to disable.</param>
    /// <returns>A new Tooltip instance with the updated animation setting.</returns>
    public static Tooltip Animated(this Tooltip tooltip, bool animated)
    {
        return tooltip with { Animated = animated };
    }
}