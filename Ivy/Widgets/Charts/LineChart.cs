using Ivy.Charts;
using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a line chart widget that displays quantitative data over time or categories using connected line segments.
/// This widget provides comprehensive configuration options for styling, axes, legends,
/// tooltips, and reference elements.
/// </summary>
public record LineChart : WidgetBase<LineChart>
{
    /// <summary>
    /// Initializes a new instance of the LineChart class with the specified data and line configurations.
    /// </summary>
    /// <param name="data">The data source containing the values to be displayed in the line chart.</param>
    /// <param name="lines">Variable number of Line configurations defining the data series to display.</param>
    public LineChart(object data, params Line[] lines)
    {
        Data = data;
        Lines = lines;
        Width = Size.Full();
        Height = Size.Full();
    }

    /// <summary>
    /// Initializes a new instance of the LineChart class with the specified data, data key, and name key.
    /// This constructor automatically creates a basic line configuration with default axes and tooltip.
    /// </summary>
    /// <param name="data">The data source containing the values to be displayed in the line chart.</param>
    /// <param name="dataKey">The key that identifies the data property containing the numerical values.</param>
    /// <param name="nameKey">The key that identifies the property containing the names/labels for data points.</param>
    public LineChart(object data, string dataKey, string nameKey)
    {
        Data = data;
        Lines = [new Line(dataKey, Utils.SplitPascalCase(dataKey))];
        XAxis = [new XAxis(nameKey)];
        YAxis = [new YAxis(dataKey)];
        Tooltip = new();
        Width = Size.Full();
        Height = Size.Full();
    }

    /// <summary>
    /// Gets or sets the data source containing the values to be displayed in the line chart.
    /// This can be any enumerable collection of objects with properties that match the data keys.
    /// </summary>
    [Prop] public object Data { get; init; }

    /// <summary>
    /// Gets or sets the layout orientation for the line chart.
    /// Default is <see cref="Layouts.Vertical"/>.
    /// </summary>
    [Prop] public Layouts Layout { get; init; } = Layouts.Vertical; //todo: not implemented on the frontend

    /// <summary>
    /// Gets or sets the color scheme used for the line chart.
    /// This determines the palette of colors used for different data series.
    /// Default is <see cref="ColorScheme.Default"/>.
    /// </summary>
    [Prop] public ColorScheme ColorScheme { get; init; } = ColorScheme.Default;

    /// <summary>
    /// Gets or sets the array of Line configurations defining the data series to display.
    /// Each Line represents a separate data series with its own styling and behavior.
    /// </summary>
    [Prop] public Line[] Lines { get; init; }

    /// <summary>
    /// Gets or sets the Cartesian grid configuration for the line chart.
    /// This controls the appearance and behavior of grid lines that help with data reading.
    /// Default is null (no grid displayed).
    /// </summary>
    [Prop] public CartesianGrid? CartesianGrid { get; init; }

    /// <summary>
    /// Gets or sets the tooltip configuration for the line chart.
    /// This controls the interactive information display when hovering over chart elements.
    /// Default is null (no tooltip displayed).
    /// </summary>
    [Prop] public Ivy.Charts.Tooltip? Tooltip { get; init; }

    /// <summary>
    /// Gets or sets the legend configuration for the line chart.
    /// This controls the display of series identifiers and color mappings.
    /// Default is null (no legend displayed).
    /// </summary>
    [Prop] public Legend? Legend { get; init; } = null;

    /// <summary>
    /// Gets or sets the array of X-axis configurations for the line chart.
    /// Multiple X-axes can be configured for complex chart layouts.
    /// Default is an empty array (no custom X-axes).
    /// </summary>
    [Prop] public XAxis[] XAxis { get; init; } = [];

    /// <summary>
    /// Gets or sets the array of Y-axis configurations for the line chart.
    /// Multiple Y-axes can be configured for complex chart layouts.
    /// Default is an empty array (no custom Y-axes).
    /// </summary>
    [Prop] public YAxis[] YAxis { get; init; } = [];

    /// <summary>
    /// Gets or sets the array of reference area configurations for highlighting specific regions.
    /// Reference areas provide visual context and can mark zones of interest or thresholds.
    /// Default is an empty array (no reference areas).
    /// </summary>
    [Prop] public ReferenceArea[] ReferenceAreas { get; init; } = [];

    /// <summary>
    /// Gets or sets the array of reference dot configurations for marking specific data points.
    /// Reference dots can highlight individual values or important data points.
    /// Default is an empty array (no reference dots).
    /// </summary>
    [Prop] public ReferenceDot[] ReferenceDots { get; init; } = [];

    /// <summary>
    /// Gets or sets the array of reference line configurations for marking thresholds or boundaries.
    /// Reference lines can indicate target values, averages, or other important reference points.
    /// Default is an empty array (no reference lines).
    /// </summary>
    [Prop] public ReferenceLine[] ReferenceLines { get; init; } = [];

    /// <summary>
    /// Operator overload that prevents LineChart from accepting child widgets.
    /// Line charts are self-contained and do not support child widget composition.
    /// </summary>
    /// <param name="widget">The LineChart widget.</param>
    /// <param name="child">The child widget (not supported).</param>
    /// <returns>Throws NotSupportedException.</returns>
    /// <exception cref="NotSupportedException">LineChart does not support children.</exception>
    public static LineChart operator |(LineChart widget, object child)
    {
        throw new NotSupportedException("LineChart does not support children.");
    }
}

/// <summary>
/// Extension methods for the LineChart class that provide a fluent API for easy configuration.
/// Each method returns a new LineChart instance with the updated configuration, following the immutable pattern.
/// </summary>
public static class LineChartExtensions
{
    /// <summary>
    /// Sets the layout orientation for the line chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="layout">The layout orientation to use.</param>
    /// <returns>A new LineChart instance with the updated layout.</returns>
    public static LineChart Layout(this LineChart chart, Layouts layout)
    {
        return chart with { Layout = layout };
    }

    /// <summary>
    /// Sets the line chart layout to horizontal orientation.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <returns>A new LineChart instance with horizontal layout.</returns>
    public static LineChart Horizontal(this LineChart chart)
    {
        return chart with { Layout = Layouts.Horizontal };
    }

    /// <summary>
    /// Sets the line chart layout to vertical orientation.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <returns>A new LineChart instance with vertical layout.</returns>
    public static LineChart Vertical(this LineChart chart)
    {
        return chart with { Layout = Layouts.Vertical };
    }

    /// <summary>
    /// Adds one or more Line configurations to the existing line list, preserving any existing lines.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="lines">Variable number of Line configurations to add.</param>
    /// <returns>A new LineChart instance with the additional line configurations.</returns>
    public static LineChart Line(this LineChart chart, params Line[] lines)
    {
        return chart with { Lines = [.. chart.Lines, .. lines] };
    }

    /// <summary>
    /// Adds a simple line configuration for the specified data key to the existing line list.
    /// This creates a basic line with optional naming.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="dataKey">The data property key to create a line for.</param>
    /// <param name="name">Optional display name for the line. If not provided, will be auto-generated from dataKey.</param>
    /// <returns>A new LineChart instance with the additional line configuration.</returns>
    public static LineChart Line(this LineChart chart, string dataKey, string? name = null)
    {
        return chart with { Lines = [.. chart.Lines, new Line(dataKey, name ?? Utils.SplitPascalCase(dataKey))] };
    }

    /// <summary>
    /// Sets the Cartesian grid configuration for the line chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="cartesianGrid">The CartesianGrid configuration to use.</param>
    /// <returns>A new LineChart instance with the updated Cartesian grid configuration.</returns>
    public static LineChart CartesianGrid(this LineChart chart, CartesianGrid cartesianGrid)
    {
        return chart with { CartesianGrid = cartesianGrid };
    }

    /// <summary>
    /// Enables the Cartesian grid with default configuration for the line chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <returns>A new LineChart instance with default Cartesian grid enabled.</returns>
    public static LineChart CartesianGrid(this LineChart chart)
    {
        return chart with { CartesianGrid = new CartesianGrid() };
    }

    /// <summary>
    /// Adds an X-axis configuration to the existing X-axis list, preserving any existing X-axes.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="xAxis">The XAxis configuration to add.</param>
    /// <returns>A new LineChart instance with the additional X-axis configuration.</returns>
    public static LineChart XAxis(this LineChart chart, XAxis xAxis)
    {
        return chart with { XAxis = [.. chart.XAxis, xAxis] };
    }

    /// <summary>
    /// Adds a simple X-axis configuration for the specified data key to the existing X-axis list.
    /// This creates a basic X-axis that represents the specified data property.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="dataKey">The data property key to create an X-axis for.</param>
    /// <returns>A new LineChart instance with the additional X-axis configuration.</returns>
    public static LineChart XAxis(this LineChart chart, string dataKey)
    {
        return chart with { XAxis = [.. chart.XAxis, new XAxis(dataKey)] };
    }

    /// <summary>
    /// Adds a Y-axis configuration to the existing Y-axis list, preserving any existing Y-axes.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="yAxis">The YAxis configuration to add.</param>
    /// <returns>A new LineChart instance with the additional Y-axis configuration.</returns>
    public static LineChart YAxis(this LineChart chart, YAxis yAxis)
    {
        return chart with { YAxis = [.. chart.YAxis, yAxis] };
    }

    /// <summary>
    /// Adds a simple Y-axis configuration for the specified data key to the existing Y-axis list.
    /// This creates a basic Y-axis that represents the specified data property.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="dataKey">The data property key to create a Y-axis for.</param>
    /// <returns>A new LineChart instance with the additional Y-axis configuration.</returns>
    public static LineChart YAxis(this LineChart chart, string dataKey)
    {
        return chart with { YAxis = [.. chart.YAxis, new YAxis(dataKey)] };
    }

    /// <summary>
    /// Adds a default Y-axis configuration to the existing Y-axis list.
    /// This creates a basic Y-axis with default settings.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <returns>A new LineChart instance with the additional default Y-axis configuration.</returns>
    public static LineChart YAxis(this LineChart chart)
    {
        return chart with { YAxis = [.. chart.YAxis, new YAxis()] };
    }

    /// <summary>
    /// Sets the tooltip configuration for the line chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="tooltip">The Tooltip configuration to use, or null to disable tooltips.</param>
    /// <returns>A new LineChart instance with the updated tooltip configuration.</returns>
    public static LineChart Tooltip(this LineChart chart, Ivy.Charts.Tooltip? tooltip)
    {
        return chart with { Tooltip = tooltip };
    }

    /// <summary>
    /// Enables the tooltip with default configuration for the line chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <returns>A new LineChart instance with default tooltip enabled.</returns>
    public static LineChart Tooltip(this LineChart chart)
    {
        return chart with { Tooltip = new Ivy.Charts.Tooltip() };
    }

    /// <summary>
    /// Sets the legend configuration for the line chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="legend">The Legend configuration to use.</param>
    /// <returns>A new LineChart instance with the updated legend configuration.</returns>
    public static LineChart Legend(this LineChart chart, Legend legend)
    {
        return chart with { Legend = legend };
    }

    /// <summary>
    /// Enables the legend with default configuration for the line chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <returns>A new LineChart instance with default legend enabled.</returns>
    public static LineChart Legend(this LineChart chart)
    {
        return chart with { Legend = new Legend() };
    }

    /// <summary>
    /// Adds a reference area configuration to the existing reference area list, preserving any existing areas.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="referenceArea">The ReferenceArea configuration to add.</param>
    /// <returns>A new LineChart instance with the additional reference area configuration.</returns>
    public static LineChart ReferenceArea(this LineChart chart, ReferenceArea referenceArea)
    {
        return chart with { ReferenceAreas = [.. chart.ReferenceAreas, referenceArea] };
    }

    /// <summary>
    /// Adds a simple reference area configuration with the specified coordinates to the existing list.
    /// This creates a basic reference area that highlights a rectangular region on the chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="x1">The leftmost X coordinate of the reference area.</param>
    /// <param name="y1">The topmost Y coordinate of the reference area.</param>
    /// <param name="x2">The rightmost X coordinate of the reference area.</param>
    /// <param name="y2">The bottommost Y coordinate of the reference area.</param>
    /// <param name="label">Optional text label to display on or near the reference area.</param>
    /// <returns>A new LineChart instance with the additional reference area configuration.</returns>
    public static LineChart ReferenceArea(this LineChart chart, double x1, double y1, double x2, double y2, string? label = null)
    {
        return chart with { ReferenceAreas = [.. chart.ReferenceAreas, new ReferenceArea(x1, y1, x2, y2, label)] };
    }

    /// <summary>
    /// Adds a reference dot configuration to the existing reference dot list, preserving any existing dots.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="referenceDot">The ReferenceDot configuration to add.</param>
    /// <returns>A new LineChart instance with the additional reference dot configuration.</returns>
    public static LineChart ReferenceDot(this LineChart chart, ReferenceDot referenceDot)
    {
        return chart with { ReferenceDots = [.. chart.ReferenceDots, referenceDot] };
    }

    /// <summary>
    /// Adds a simple reference dot configuration with the specified coordinates to the existing list.
    /// This creates a basic reference dot that marks a specific point on the chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="x">The X coordinate of the reference dot.</param>
    /// <param name="y">The Y coordinate of the reference dot.</param>
    /// <param name="label">Optional text label to display on or near the reference dot.</param>
    /// <returns>A new LineChart instance with the additional reference dot configuration.</returns>
    public static LineChart ReferenceDot(this LineChart chart, double x, double y, string? label = null)
    {
        return chart with { ReferenceDots = [.. chart.ReferenceDots, new ReferenceDot(x, y, label)] };
    }

    /// <summary>
    /// Adds a reference line configuration to the existing reference line list, preserving any existing lines.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="referenceLine">The ReferenceLine configuration to add.</param>
    /// <returns>A new LineChart instance with the additional reference line configuration.</returns>
    public static LineChart ReferenceLine(this LineChart chart, ReferenceLine referenceLine)
    {
        return chart with { ReferenceLines = [.. chart.ReferenceLines, referenceLine] };
    }

    /// <summary>
    /// Adds a simple reference line configuration with the specified coordinates to the existing list.
    /// This creates a basic reference line that marks a threshold or boundary on the chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="x">The X coordinate for a vertical reference line, or null for a horizontal line.</param>
    /// <param name="y">The Y coordinate for a horizontal reference line, or null for a vertical line.</param>
    /// <param name="label">Optional text label to display on or near the reference line.</param>
    /// <returns>A new LineChart instance with the additional reference line configuration.</returns>
    public static LineChart ReferenceLine(this LineChart chart, double? x, double? y, string? label = null)
    {
        return chart with { ReferenceLines = [.. chart.ReferenceLines, new ReferenceLine(x, y, label)] };
    }

    /// <summary>
    /// Sets the color scheme used for the line chart.
    /// </summary>
    /// <param name="chart">The LineChart to configure.</param>
    /// <param name="colorScheme">The color scheme to use for the chart.</param>
    /// <returns>A new LineChart instance with the updated color scheme.</returns>
    public static LineChart ColorScheme(this LineChart chart, ColorScheme colorScheme)
    {
        return chart with { ColorScheme = colorScheme };
    }
}

