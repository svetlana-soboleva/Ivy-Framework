

// ReSharper disable once CheckNamespace
namespace Ivy.Charts;

/// <summary>
/// Represents a reference area configuration for charts, providing a rectangular overlay that can be used
/// to highlight specific regions, define zones of interest, or provide visual context for data analysis.
/// Reference areas are useful for marking thresholds, ranges, or regions that require special attention
/// in chart visualizations.
/// </summary>
public record ReferenceArea
{
    /// <summary>
    /// Initializes a new instance of the ReferenceArea class with the specified coordinates and optional label.
    /// </summary>
    /// <param name="x1">The leftmost X coordinate of the reference area.</param>
    /// <param name="y1">The topmost Y coordinate of the reference area.</param>
    /// <param name="x2">The rightmost X coordinate of the reference area.</param>
    /// <param name="y2">The bottommost Y coordinate of the reference area.</param>
    /// <param name="label">Optional text label to display on or near the reference area.</param>
    public ReferenceArea(double x1, double y1, double x2, double y2, string? label = null)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
        Label = label;
    }

    /// <summary>
    /// Gets or sets the leftmost X coordinate of the reference area.
    /// This defines the left boundary of the rectangular reference area.
    /// </summary>
    public double X1 { get; set; }

    /// <summary>
    /// Gets or sets the topmost Y coordinate of the reference area.
    /// This defines the top boundary of the rectangular reference area.
    /// </summary>
    public double Y1 { get; set; }

    /// <summary>
    /// Gets or sets the rightmost X coordinate of the reference area.
    /// This defines the right boundary of the rectangular reference area.
    /// </summary>
    public double X2 { get; set; }

    /// <summary>
    /// Gets or sets the bottommost Y coordinate of the reference area.
    /// This defines the bottom boundary of the rectangular reference area.
    /// </summary>
    public double Y2 { get; set; }

    /// <summary>
    /// Gets or sets the optional text label to display on or near the reference area.
    /// This label can provide context, explanation, or identification for the highlighted region.
    /// Default is null (no label displayed).
    /// </summary>
    public string? Label { get; set; }

}

/// <summary>
/// Extension methods for the ReferenceArea class that provide a fluent API for easy configuration.
/// These methods allow you to chain multiple configuration calls for better readability and maintainability.
/// Each method returns a new ReferenceArea instance with the updated configuration, following the immutable pattern.
/// </summary>
public static class ReferenceAreaExtensions
{
    /// <summary>
    /// Sets the text label to display on or near the reference area.
    /// This label can provide context, explanation, or identification for the highlighted region.
    /// </summary>
    /// <param name="referenceArea">The ReferenceArea to configure.</param>
    /// <param name="label">The text label to display.</param>
    /// <returns>A new ReferenceArea instance with the updated label.</returns>
    public static ReferenceArea Label(this ReferenceArea referenceArea, string label)
    {
        return referenceArea with { Label = label };
    }
}