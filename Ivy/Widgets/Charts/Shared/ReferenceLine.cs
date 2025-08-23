

// ReSharper disable once CheckNamespace
namespace Ivy.Charts;

/// <summary>
/// Represents a reference line configuration for charts, providing a straight line marker that can be used
/// to highlight specific values, mark thresholds, or provide visual reference lines for data analysis.
/// Reference lines are useful for indicating target values, boundaries, averages, or any horizontal/vertical
/// reference that helps users interpret chart data more effectively.
/// </summary>
public record ReferenceLine
{
    /// <summary>
    /// Initializes a new instance of the ReferenceLine class with the specified coordinates and optional label.
    /// Either X or Y should be specified to create a vertical or horizontal reference line respectively.
    /// </summary>
    /// <param name="x">The X coordinate for a vertical reference line, or null for a horizontal line.</param>
    /// <param name="y">The Y coordinate for a horizontal reference line, or null for a vertical line.</param>
    /// <param name="label">Optional text label to display on or near the reference line.</param>
    public ReferenceLine(double? x, double? y, string? label = null)
    {
        X = x;
        Y = y;
        Label = label;
    }

    /// <summary>
    /// Gets or sets the X coordinate for a vertical reference line.
    /// When specified, creates a vertical line at the given X position. When null, the line is horizontal.
    /// This allows you to create either vertical or horizontal reference lines based on which coordinate is set.
    /// Default is null (no vertical reference line).
    /// </summary>
    public double? X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate for a horizontal reference line.
    /// When specified, creates a horizontal line at the given Y position. When null, the line is vertical.
    /// This allows you to create either horizontal or vertical reference lines based on which coordinate is set.
    /// Default is null (no horizontal reference line).
    /// </summary>
    public double? Y { get; set; }

    /// <summary>
    /// Gets or sets the optional text label to display on or near the reference line.
    /// This label can provide context, explanation, or identification for the reference line.
    /// Default is null (no label displayed).
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the width of the reference line stroke in pixels. Thicker lines can improve visibility
    /// and emphasize the reference line, while thinner lines are more subtle.
    /// Default is 1 pixel.
    /// </summary>
    public int StrokeWidth { get; set; } = 1;
}

/// <summary>
/// Extension methods for the ReferenceLine class that provide a fluent API for easy configuration.
/// These methods allow you to chain multiple configuration calls for better readability and maintainability.
/// Each method returns a new ReferenceLine instance with the updated configuration, following the immutable pattern.
/// </summary>
public static class ReferenceLineExtensions
{
    /// <summary>
    /// Sets the text label to display on or near the reference line.
    /// This label can provide context, explanation, or identification for the reference line.
    /// </summary>
    /// <param name="referenceLine">The ReferenceLine to configure.</param>
    /// <param name="label">The text label to display.</param>
    /// <returns>A new ReferenceLine instance with the updated label.</returns>
    public static ReferenceLine Label(this ReferenceLine referenceLine, string label)
    {
        return referenceLine with { Label = label };
    }

    /// <summary>
    /// Sets the width of the reference line stroke in pixels. Thicker lines improve visibility and emphasis.
    /// </summary>
    /// <param name="referenceLine">The ReferenceLine to configure.</param>
    /// <param name="strokeWidth">The width of the reference line stroke in pixels.</param>
    /// <returns>A new ReferenceLine instance with the updated stroke width.</returns>
    public static ReferenceLine StrokeWidth(this ReferenceLine referenceLine, int strokeWidth)
    {
        return referenceLine with { StrokeWidth = strokeWidth };
    }
}