// ReSharper disable once CheckNamespace

using Ivy.Shared;

namespace Ivy.Charts;

/// <summary>
/// Represents a Cartesian grid configuration for charts, providing control over the positioning, sizing, and styling
/// of chart grid lines. This class allows you to customize the appearance and behavior of both horizontal and vertical
/// grid lines in your charts, including their position, dimensions, and visual styling. Grid lines help improve
/// readability by providing visual reference points for data values.
/// </summary>
public record CartesianGrid
{
    /// <summary>
    /// Gets or sets the X-coordinate position of the grid in pixels. This determines the horizontal position
    /// where the grid will be rendered on the chart.
    /// Default is null (automatic positioning).
    /// </summary>
    public double? X { get; set; } = null;

    /// <summary>
    /// Gets or sets the Y-coordinate position of the grid in pixels. This determines the vertical position
    /// where the grid will be rendered on the chart.
    /// Default is null (automatic positioning).
    /// </summary>
    public double? Y { get; set; } = null;

    /// <summary>
    /// Gets or sets the width of the grid in pixels. This controls the horizontal extent of the grid rendering.
    /// Default is null (automatic sizing based on chart dimensions).
    /// </summary>
    public double? Width { get; set; } = null;

    /// <summary>
    /// Gets or sets the height of the grid in pixels. This controls the vertical extent of the grid rendering.
    /// Default is null (automatic sizing based on chart dimensions).
    /// </summary>
    public double? Height { get; set; } = null;

    /// <summary>
    /// Gets or sets whether horizontal grid lines are displayed. When true, horizontal lines extend across
    /// the chart to help with value reading and visual alignment.
    /// Default is true.
    /// </summary>
    public bool Horizontal { get; set; } = true;

    /// <summary>
    /// Gets or sets whether vertical grid lines are displayed. When true, vertical lines extend up and down
    /// the chart to help with value reading and visual alignment.
    /// Default is true.
    /// </summary>
    public bool Vertical { get; set; } = true;

    /// <summary>
    /// Gets or sets the fill color for the grid lines. If null, a default color from the chart's
    /// color scheme will be used.
    /// <see cref="Colors"/> is an enum that contains all the available colors for the chart.
    /// Default is null.
    /// </summary>
    public Colors? Fill { get; set; } = null;

    /// <summary>
    /// Gets or sets the opacity of the fill color for the grid lines. Value ranges from 0.0 (transparent)
    /// to 1.0 (opaque). This allows you to create subtle grid lines that don't interfere with the main chart content.
    /// Default is null.
    /// </summary>
    public double? FillOpacity { get; set; } = null;

    /// <summary>
    /// Gets or sets the dash pattern for the grid lines, creating dashed or dotted patterns.
    /// Examples include "5,5" for dashed lines, "10,5,5,5" for dash-dot patterns, or "1,1" for dotted lines.
    /// Default is null (solid lines).
    /// </summary>
    public string? StrokeDashArray { get; set; }
}

/// <summary>
/// Extension methods for the CartesianGrid class that provide a fluent API for easy configuration.
/// These methods allow you to chain multiple configuration calls for better readability and maintainability.
/// Each method returns a new CartesianGrid instance with the updated configuration, following the immutable pattern.
/// </summary>
public static class CartesianGridExtensions
{
    /// <summary>
    /// Sets the X-coordinate position of the grid in pixels. This determines the horizontal position
    /// where the grid will be rendered on the chart.
    /// </summary>
    /// <param name="cartesianGrid">The CartesianGrid to configure.</param>
    /// <param name="x">The X-coordinate position in pixels.</param>
    /// <returns>A new CartesianGrid instance with the updated X position.</returns>
    public static CartesianGrid X(this CartesianGrid cartesianGrid, double x)
    {
        return cartesianGrid with { X = x };
    }

    /// <summary>
    /// Sets the Y-coordinate position of the grid in pixels. This determines the vertical position
    /// where the grid will be rendered on the chart.
    /// </summary>
    /// <param name="cartesianGrid">The CartesianGrid to configure.</param>
    /// <param name="y">The Y-coordinate position in pixels.</param>
    /// <returns>A new CartesianGrid instance with the updated Y position.</returns>
    public static CartesianGrid Y(this CartesianGrid cartesianGrid, double y)
    {
        return cartesianGrid with { Y = y };
    }

    /// <summary>
    /// Sets the width of the grid in pixels. This controls the horizontal extent of the grid rendering.
    /// </summary>
    /// <param name="cartesianGrid">The CartesianGrid to configure.</param>
    /// <param name="width">The width of the grid in pixels.</param>
    /// <returns>A new CartesianGrid instance with the updated width.</returns>
    public static CartesianGrid Width(this CartesianGrid cartesianGrid, double width)
    {
        return cartesianGrid with { Width = width };
    }

    /// <summary>
    /// Sets the height of the grid in pixels. This controls the vertical extent of the grid rendering.
    /// </summary>
    /// <param name="cartesianGrid">The CartesianGrid to configure.</param>
    /// <param name="height">The height of the grid in pixels.</param>
    /// <returns>A new CartesianGrid instance with the updated height.</returns>
    public static CartesianGrid Height(this CartesianGrid cartesianGrid, double height)
    {
        return cartesianGrid with { Height = height };
    }

    /// <summary>
    /// Enables only horizontal grid lines and disables vertical grid lines. This creates a grid pattern
    /// with horizontal lines only, which is useful for charts where you want to emphasize horizontal
    /// value comparisons.
    /// </summary>
    /// <param name="cartesianGrid">The CartesianGrid to configure.</param>
    /// <returns>A new CartesianGrid instance with only horizontal grid lines enabled.</returns>
    public static CartesianGrid Horizontal(this CartesianGrid cartesianGrid)
    {
        return cartesianGrid with { Horizontal = true, Vertical = false };
    }

    /// <summary>
    /// Enables only vertical grid lines and disables horizontal grid lines. This creates a grid pattern
    /// with vertical lines only, which is useful for charts where you want to emphasize vertical
    /// value comparisons.
    /// </summary>
    /// <param name="cartesianGrid">The CartesianGrid to configure.</param>
    /// <returns>A new CartesianGrid instance with only vertical grid lines enabled.</returns>
    public static CartesianGrid Vertical(this CartesianGrid cartesianGrid)
    {
        return cartesianGrid with { Vertical = true, Horizontal = false };
    }

    /// <summary>
    /// Sets the fill color for the grid lines. This color will be applied to all grid lines
    /// rendered by this grid configuration.
    /// </summary>
    /// <param name="cartesianGrid">The CartesianGrid to configure.</param>
    /// <param name="fill">The color to use for the grid lines.</param>
    /// <returns>A new CartesianGrid instance with the updated fill color.</returns>
    public static CartesianGrid Fill(this CartesianGrid cartesianGrid, Colors fill)
    {
        return cartesianGrid with { Fill = fill };
    }

    /// <summary>
    /// Sets the opacity of the fill color for the grid lines. This allows you to create subtle
    /// grid lines that don't interfere with the main chart content while still providing
    /// visual reference points.
    /// </summary>
    /// <param name="cartesianGrid">The CartesianGrid to configure.</param>
    /// <param name="fillOpacity">The opacity value ranging from 0.0 (transparent) to 1.0 (opaque).</param>
    /// <returns>A new CartesianGrid instance with the updated fill opacity.</returns>
    public static CartesianGrid FillOpacity(this CartesianGrid cartesianGrid, double fillOpacity)
    {
        return cartesianGrid with { FillOpacity = fillOpacity };
    }

    /// <summary>
    /// Sets the dash pattern for the grid lines, creating dashed or dotted patterns.
    /// This is useful for creating different visual styles for grid lines to distinguish them
    /// from other chart elements or to create specific visual effects.
    /// </summary>
    /// <param name="cartesianGrid">The CartesianGrid to configure.</param>
    /// <param name="strokeDashArray">The dash pattern (e.g., "5,5" for dashed lines, "10,5,5,5" for dash-dot pattern).</param>
    /// <returns>A new CartesianGrid instance with the updated stroke dash array.</returns>
    public static CartesianGrid StrokeDashArray(this CartesianGrid cartesianGrid, string strokeDashArray)
    {
        return cartesianGrid with { StrokeDashArray = strokeDashArray };
    }
}