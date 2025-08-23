

// ReSharper disable once CheckNamespace
namespace Ivy.Charts;

/// <summary>
/// Represents a reference dot configuration for charts, providing a single point marker that can be used
/// to highlight specific data points, mark important values, or provide visual reference for data analysis.
/// Reference dots are useful for pinpointing exact locations, marking thresholds, or highlighting individual
/// data points that require special attention in chart visualizations.
/// </summary>
public record ReferenceDot
{
    /// <summary>
    /// Initializes a new instance of the ReferenceDot class with the specified coordinates and optional label.
    /// </summary>
    /// <param name="x">The X coordinate of the reference dot.</param>
    /// <param name="y">The Y coordinate of the reference dot.</param>
    /// <param name="label">Optional text label to display on or near the reference dot.</param>
    public ReferenceDot(double x, double y, string? label = null)
    {
        X = x;
        Y = y;
        Label = label;
    }

    /// <summary>
    /// Gets or sets the X coordinate of the reference dot.
    /// This defines the horizontal position of the reference point on the chart.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the reference dot.
    /// This defines the vertical position of the reference point on the chart.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the optional text label to display on or near the reference dot.
    /// This label can provide context, explanation, or identification for the marked point.
    /// Default is null (no label displayed).
    /// </summary>
    public string? Label { get; set; }
}

/// <summary>
/// Extension methods for the ReferenceDot class that provide a fluent API for easy configuration.
/// These methods allow you to chain multiple configuration calls for better readability and maintainability.
/// Each method returns a new ReferenceDot instance with the updated configuration, following the immutable pattern.
/// </summary>
public static class ReferenceDotExtensions
{
    /// <summary>
    /// Sets the text label to display on or near the reference dot.
    /// This label can provide context, explanation, or identification for the marked point.
    /// </summary>
    /// <param name="referenceDot">The ReferenceDot to configure.</param>
    /// <param name="label">The text label to display.</param>
    /// <returns>A new ReferenceDot instance with the updated label.</returns>
    public static ReferenceDot Label(this ReferenceDot referenceDot, string label)
    {
        return referenceDot with { Label = label };
    }
}