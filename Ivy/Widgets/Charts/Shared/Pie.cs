using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy.Charts;

/// <summary>
/// Represents a pie chart configuration with customizable styling, positioning, and appearance options.
/// Pie charts are ideal for showing proportions and percentages of a whole, displaying data as slices
/// of a circular chart. This class provides comprehensive control over pie appearance, including
/// positioning, sizing, colors, labels, and animation effects.
/// </summary>
public record Pie
{
    /// <summary>
    /// Initializes a new instance of the Pie class with the specified data and name keys.
    /// </summary>
    /// <param name="dataKey">The key that identifies the data property containing the numerical values for pie slices.</param>
    /// <param name="nameKey">The key that identifies the property containing the names/labels for pie slices.</param>
    public Pie(string dataKey, string nameKey)
    {
        DataKey = dataKey;
        NameKey = nameKey;
    }

    /// <summary>
    /// Gets the key that identifies which data property contains the numerical values for pie slices.
    /// This property is immutable and set during construction.
    /// </summary>
    public string DataKey { get; }

    /// <summary>
    /// Gets or sets the key that identifies which property contains the names/labels for pie slices.
    /// These names are displayed in tooltips, legends, and labels to identify each slice.
    /// </summary>
    public string NameKey { get; set; }

    /// <summary>
    /// Gets or sets the visual representation type for this pie in the legend.
    /// This determines how the pie appears in chart legends, affecting user understanding of the data series.
    /// Default is <see cref="LegendTypes.Line"/>.
    /// </summary>
    public LegendTypes LegendType { get; set; } = LegendTypes.Line;

    /// <summary>
    /// Gets or sets the color of the pie slice borders/strokes. If null, the chart's default color scheme will be used.
    /// This allows for custom colorization of pie slice borders for better visual distinction.
    /// Default is null (uses chart color scheme).
    /// </summary>
    public Colors? Stroke { get; set; } = null;

    /// <summary>
    /// Gets or sets the width of the pie slice borders/strokes in pixels. Thicker borders can improve visibility
    /// and emphasize slice boundaries, while thinner borders are more subtle.
    /// Default is 1 pixel.
    /// </summary>
    public int StrokeWidth { get; set; } = 1;

    /// <summary>
    /// Gets or sets the fill color for pie slices. If null, the chart's default color scheme will be used.
    /// This allows for custom colorization of individual pie slices for better visual distinction.
    /// Default is null (uses chart color scheme).
    /// </summary>
    public Colors? Fill { get; set; } = null;

    /// <summary>
    /// Gets or sets the opacity of the pie slice fill colors. Values range from 0.0 (transparent) to 1.0 (opaque).
    /// This can create semi-transparent effects and help with visual layering.
    /// Default is null (uses default opacity).
    /// </summary>
    public double? FillOpacity { get; set; } = null;

    /// <summary>
    /// Gets or sets the dash pattern for the pie slice borders/strokes. This creates dashed or dotted border effects
    /// that can help distinguish between different pie types or add visual interest.
    /// Examples: "5,5" for dashed, "2,2" for dotted, "10,5,5,5" for complex patterns.
    /// Default is null (solid borders).
    /// </summary>
    public string? StrokeDashArray { get; set; }

    /// <summary>
    /// Gets or sets whether this pie chart should be animated when first rendered or updated.
    /// Animation can make charts more engaging and help draw attention to data changes.
    /// Default is false.
    /// </summary>
    public bool Animated { get; set; } = false;

    /// <summary>
    /// Gets or sets the horizontal center position of the pie chart. Can be specified as an integer (pixels)
    /// or a string (percentage, CSS units). This controls the horizontal positioning of the pie chart.
    /// Default is null (auto-centered).
    /// </summary>
    public object? Cx { get; set; }

    /// <summary>
    /// Gets or sets the vertical center position of the pie chart. Can be specified as an integer (pixels)
    /// or a string (percentage, CSS units). This controls the vertical positioning of the pie chart.
    /// Default is null (auto-centered).
    /// </summary>
    public object? Cy { get; set; }

    /// <summary>
    /// Gets or sets the inner radius of the pie chart, creating a donut chart effect. Can be specified as an integer (pixels)
    /// or a string (percentage, CSS units). When set, the pie chart becomes a ring/donut chart.
    /// Default is null (solid pie chart).
    /// </summary>
    public object? InnerRadius { get; set; }

    /// <summary>
    /// Gets or sets the outer radius of the pie chart, controlling the overall size. Can be specified as an integer (pixels)
    /// or a string (percentage, CSS units). This determines the maximum size of the pie chart.
    /// Default is null (auto-sized).
    /// </summary>
    public object? OuterRadius { get; set; }

    /// <summary>
    /// Gets or sets the starting angle for the pie chart in degrees. This controls where the first slice begins.
    /// Values range from 0 to 360 degrees, where 0 is at the top (12 o'clock position).
    /// Default is 0 degrees (starts at top).
    /// </summary>
    public int StartAngle { get; set; } = 0;

    /// <summary>
    /// Gets or sets the ending angle for the pie chart in degrees. This controls where the last slice ends.
    /// Values range from 0 to 360 degrees. Combined with StartAngle, this can create partial pie charts.
    /// Default is 360 degrees (full circle).
    /// </summary>
    public int EndAngle { get; set; } = 360;

    /// <summary>
    /// Gets or sets an array of label configurations for displaying values on or near pie slices.
    /// Labels can show actual data values, percentages, or other relevant information directly on the chart.
    /// Default is an empty array (no labels displayed).
    /// </summary>
    public LabelList[] LabelLists { get; set; } = [];
}

/// <summary>
/// Extension methods for the Pie class that provide a fluent API for easy configuration.
/// These methods allow you to chain multiple configuration calls for better readability and maintainability.
/// Each method returns a new Pie instance with the updated configuration, following the immutable pattern.
/// </summary>
public static class PieExtensions
{
    /// <summary>
    /// Sets the visual representation type for this pie in the legend.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="legendType">The legend type to use for representing this pie.</param>
    /// <returns>A new Pie instance with the updated legend type.</returns>
    public static Pie LegendType(this Pie pie, LegendTypes legendType)
    {
        return pie with { LegendType = legendType };
    }

    /// <summary>
    /// Sets the color of the pie slice borders/strokes. This overrides the chart's default color scheme.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="stroke">The color to use for the pie slice borders.</param>
    /// <returns>A new Pie instance with the updated stroke color.</returns>
    public static Pie Stroke(this Pie pie, Colors stroke)
    {
        return pie with { Stroke = stroke };
    }

    /// <summary>
    /// Sets the width of the pie slice borders/strokes in pixels. Thicker borders improve visibility and emphasis.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="strokeWidth">The width of the pie slice borders in pixels.</param>
    /// <returns>A new Pie instance with the updated stroke width.</returns>
    public static Pie StrokeWidth(this Pie pie, int strokeWidth)
    {
        return pie with { StrokeWidth = strokeWidth };
    }

    /// <summary>
    /// Sets the dash pattern for the pie slice borders/strokes, creating dashed or dotted border effects.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="strokeDashArray">The dash pattern string (e.g., "5,5" for dashed, "2,2" for dotted).</param>
    /// <returns>A new Pie instance with the updated dash pattern.</returns>
    public static Pie StrokeDashArray(this Pie pie, string strokeDashArray)
    {
        return pie with { StrokeDashArray = strokeDashArray };
    }

    /// <summary>
    /// Sets the fill color for pie slices. This overrides the chart's default color scheme.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="fill">The color to use for filling pie slices.</param>
    /// <returns>A new Pie instance with the updated fill color.</returns>
    public static Pie Fill(this Pie pie, Colors fill)
    {
        return pie with { Fill = fill };
    }

    /// <summary>
    /// Sets the opacity of the pie slice fill colors, creating semi-transparent effects.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="fillOpacity">The opacity value from 0.0 (transparent) to 1.0 (opaque).</param>
    /// <returns>A new Pie instance with the updated fill opacity.</returns>
    public static Pie FillOpacity(this Pie pie, double fillOpacity)
    {
        return pie with { FillOpacity = fillOpacity };
    }

    /// <summary>
    /// Sets whether this pie chart should be animated when rendered or updated.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="animated">True to enable animation, false to disable. Default is true.</param>
    /// <returns>A new Pie instance with the updated animation setting.</returns>
    public static Pie Animated(this Pie pie, bool animated = true)
    {
        return pie with { Animated = animated };
    }

    /// <summary>
    /// Sets the horizontal center position of the pie chart using an integer value in pixels.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="cx">The horizontal center position in pixels.</param>
    /// <returns>A new Pie instance with the updated horizontal center position.</returns>
    public static Pie Cx(this Pie pie, int cx)
    {
        return pie with { Cx = cx };
    }

    /// <summary>
    /// Sets the horizontal center position of the pie chart using a string value (percentage, CSS units).
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="cx">The horizontal center position as a string (e.g., "50%", "200px").</param>
    /// <returns>A new Pie instance with the updated horizontal center position.</returns>
    public static Pie Cx(this Pie pie, string cx)
    {
        return pie with { Cx = cx };
    }

    /// <summary>
    /// Sets the vertical center position of the pie chart using an integer value in pixels.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="cy">The vertical center position in pixels.</param>
    /// <returns>A new Pie instance with the updated vertical center position.</returns>
    public static Pie Cy(this Pie pie, int cy)
    {
        return pie with { Cy = cy };
    }

    /// <summary>
    /// Sets the vertical center position of the pie chart using a string value (percentage, CSS units).
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="cy">The vertical center position as a string (e.g., "50%", "200px").</param>
    /// <returns>A new Pie instance with the updated vertical center position.</returns>
    public static Pie Cy(this Pie pie, string cy)
    {
        return pie with { Cy = cy };
    }

    /// <summary>
    /// Sets the inner radius of the pie chart using an integer value in pixels, creating a donut chart effect.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="innerRadius">The inner radius in pixels.</param>
    /// <returns>A new Pie instance with the updated inner radius.</returns>
    public static Pie InnerRadius(this Pie pie, int innerRadius)
    {
        return pie with { InnerRadius = innerRadius };
    }

    /// <summary>
    /// Sets the inner radius of the pie chart using a string value (percentage, CSS units), creating a donut chart effect.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="innerRadius">The inner radius as a string (e.g., "30%", "100px").</param>
    /// <returns>A new Pie instance with the updated inner radius.</returns>
    public static Pie InnerRadius(this Pie pie, string innerRadius)
    {
        return pie with { InnerRadius = innerRadius };
    }

    /// <summary>
    /// Sets the outer radius of the pie chart using an integer value in pixels, controlling the overall size.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="outerRadius">The outer radius in pixels.</param>
    /// <returns>A new Pie instance with the updated outer radius.</returns>
    public static Pie OuterRadius(this Pie pie, int outerRadius)
    {
        return pie with { OuterRadius = outerRadius };
    }

    /// <summary>
    /// Sets the outer radius of the pie chart using a string value (percentage, CSS units), controlling the overall size.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="outerRadius">The outer radius as a string (e.g., "80%", "300px").</param>
    /// <returns>A new Pie instance with the updated outer radius.</returns>
    public static Pie OuterRadius(this Pie pie, string outerRadius)
    {
        return pie with { OuterRadius = outerRadius };
    }

    /// <summary>
    /// Sets the starting angle for the pie chart in degrees, controlling where the first slice begins.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="startAngle">The starting angle in degrees (0-360, where 0 is at the top).</param>
    /// <returns>A new Pie instance with the updated starting angle.</returns>
    public static Pie StartAngle(this Pie pie, int startAngle)
    {
        return pie with { StartAngle = startAngle };
    }

    /// <summary>
    /// Sets the ending angle for the pie chart in degrees, controlling where the last slice ends.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="endAngle">The ending angle in degrees (0-360).</param>
    /// <returns>A new Pie instance with the updated ending angle.</returns>
    public static Pie EndAngle(this Pie pie, int endAngle)
    {
        return pie with { EndAngle = endAngle };
    }

    /// <summary>
    /// Sets the complete array of label configurations for displaying values on or near pie slices.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="labelLists">The array of label configurations to use.</param>
    /// <returns>A new Pie instance with the updated label configurations.</returns>
    public static Pie LabelLists(this Pie pie, LabelList[] labelLists)
    {
        return pie with { LabelLists = labelLists };
    }

    /// <summary>
    /// Adds a single label configuration to the existing label list, preserving any existing labels.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="labelList">The label configuration to add.</param>
    /// <returns>A new Pie instance with the additional label configuration.</returns>
    public static Pie LabelList(this Pie pie, LabelList labelList)
    {
        return pie with { LabelLists = [.. pie.LabelLists, labelList] };
    }

    /// <summary>
    /// Adds a simple label configuration for the specified data key to the existing label list.
    /// This creates a basic label that displays the data value for the specified property.
    /// </summary>
    /// <param name="pie">The Pie to configure.</param>
    /// <param name="dataKey">The data property key to create a label for.</param>
    /// <returns>A new Pie instance with the additional label configuration.</returns>
    public static Pie LabelList(this Pie pie, string dataKey)
    {
        return pie with { LabelLists = [.. pie.LabelLists, new LabelList(dataKey)] };
    }
}

