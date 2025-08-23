using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy.Charts;

/// <summary>
/// Represents a label list configuration for chart elements, providing comprehensive control over the positioning,
/// rotation, styling, and formatting of multiple labels that display values on or near chart components.
/// LabelList is more feature-rich than the basic Label class and is commonly used for displaying data values,
/// percentages, or other contextual information directly on charts with advanced formatting options.
/// </summary>
public record LabelList
{
    /// <summary>
    /// Initializes a new instance of the LabelList class with the specified data key.
    /// The data key identifies which data property the labels will display values from.
    /// </summary>
    /// <param name="dataKey">The key that identifies the data property to display in the labels.</param>
    public LabelList(string dataKey)
    {
        DataKey = dataKey;
    }

    /// <summary>
    /// Gets or sets the key that identifies the data property to display in the labels.
    /// This key should match a property name in your data objects.
    /// </summary>
    public string DataKey { get; set; }

    /// <summary>
    /// Gets or sets the offset distance of the labels from their associated chart element in pixels.
    /// This determines how far the labels are positioned from the data points, lines, or areas they represent.
    /// Default is 5 pixels.
    /// </summary>
    public double Offset { get; set; } = 5;

    /// <summary>
    /// Gets or sets the rotation angle of the label text in degrees. Positive values rotate clockwise.
    /// This is useful for fitting labels in tight spaces or creating angled text for better readability.
    /// Default is null (no rotation).
    /// </summary>
    public double? Angle { get; set; } = null;

    /// <summary>
    /// Gets or sets the position of the labels relative to their associated chart element.
    /// This determines where the labels appear in relation to the data points, lines, or areas.
    /// <see cref="Positions"/> is an enum that contains all the available positioning options.
    /// Default is <see cref="Positions.Outside"/>.
    /// </summary>
    public Positions? Position { get; set; } = Positions.Outside;

    /// <summary>
    /// Gets or sets the fill color of the label text. This color will be applied to the label text,
    /// allowing you to customize the appearance to match your design requirements.
    /// <see cref="Colors"/> is an enum that contains all the available colors for the chart.
    /// Default is <see cref="Colors.Black"/>.
    /// </summary>
    public Colors? Fill { get; set; } = Colors.Black;

    /// <summary>
    /// Gets or sets the opacity of the fill color for the label text. Value ranges from 0.0 (transparent)
    /// to 1.0 (opaque). This allows you to create subtle labels that don't interfere with the main chart content.
    /// Default is null.
    /// </summary>
    public double? FillOpacity { get; set; } = null;

    /// <summary>
    /// Gets or sets the font size of the label text in pixels. This controls the size of the text displayed
    /// in the labels, allowing you to adjust readability and visual hierarchy.
    /// Default is null (inherits from chart default).
    /// </summary>
    public int? FontSize { get; set; } = null;

    /// <summary>
    /// Gets or sets the font family for the label text. This allows you to specify custom fonts
    /// to match your design requirements or brand guidelines.
    /// Default is null (inherits from chart default).
    /// </summary>
    public string? FontFamily { get; set; } = null;

    /// <summary>
    /// Gets or sets the number format string for the label values. This allows you to control how
    /// numerical values are displayed, including decimal places, currency symbols, and percentage formatting.
    /// Examples: "0.0%" for percentages, "$0.00" for currency, "0.0" for one decimal place.
    /// Default is null (no special formatting).
    /// </summary>
    public string? NumberFormat { get; set; } = null;
}

/// <summary>
/// Extension methods for the LabelList class that provide a fluent API for easy configuration.
/// These methods allow you to chain multiple configuration calls for better readability and maintainability.
/// Each method returns a new LabelList instance with the updated configuration, following the immutable pattern.
/// </summary>
public static class LabelListExtensions
{
    /// <summary>
    /// Sets the offset distance of the labels from their associated chart element in pixels.
    /// This determines how far the labels are positioned from the data points, lines, or areas they represent.
    /// </summary>
    /// <param name="label">The LabelList to configure.</param>
    /// <param name="offset">The offset distance in pixels.</param>
    /// <returns>A new LabelList instance with the updated offset.</returns>
    public static LabelList Offset(this LabelList label, double offset)
    {
        return label with { Offset = offset };
    }

    /// <summary>
    /// Sets the rotation angle of the label text in degrees. Positive values rotate clockwise.
    /// This is useful for fitting labels in tight spaces or creating angled text for better readability.
    /// </summary>
    /// <param name="label">The LabelList to configure.</param>
    /// <param name="angle">The rotation angle in degrees.</param>
    /// <returns>A new LabelList instance with the updated rotation angle.</returns>
    public static LabelList Angle(this LabelList label, double angle)
    {
        return label with { Angle = angle };
    }

    /// <summary>
    /// Sets the position of the labels relative to their associated chart element.
    /// This determines where the labels appear in relation to the data points, lines, or areas.
    /// </summary>
    /// <param name="label">The LabelList to configure.</param>
    /// <param name="position">The position for the labels.</param>
    /// <returns>A new LabelList instance with the updated position.</returns>
    public static LabelList Position(this LabelList label, Positions position)
    {
        return label with { Position = position };
    }

    /// <summary>
    /// Sets the fill color of the label text. This color will be applied to the label text,
    /// allowing you to customize the appearance to match your design requirements.
    /// </summary>
    /// <param name="label">The LabelList to configure.</param>
    /// <param name="color">The color to use for the label text.</param>
    /// <returns>A new LabelList instance with the updated fill color.</returns>
    public static LabelList Fill(this LabelList label, Colors color)
    {
        return label with { Fill = color };
    }

    /// <summary>
    /// Sets the opacity of the fill color for the label text. This allows you to create subtle
    /// labels that don't interfere with the main chart content while still providing clear information.
    /// </summary>
    /// <param name="label">The LabelList to configure.</param>
    /// <param name="fillOpacity">The opacity value ranging from 0.0 (transparent) to 1.0 (opaque).</param>
    /// <returns>A new LabelList instance with the updated fill opacity.</returns>
    public static LabelList FillOpacity(this LabelList label, double fillOpacity)
    {
        return label with { FillOpacity = fillOpacity };
    }

    /// <summary>
    /// Sets the font size of the label text in pixels. This controls the size of the text displayed
    /// in the labels, allowing you to adjust readability and visual hierarchy.
    /// </summary>
    /// <param name="label">The LabelList to configure.</param>
    /// <param name="fontSize">The font size in pixels.</param>
    /// <returns>A new LabelList instance with the updated font size.</returns>
    public static LabelList FontSize(this LabelList label, int fontSize)
    {
        return label with { FontSize = fontSize };
    }

    /// <summary>
    /// Sets the font family for the label text. This allows you to specify custom fonts
    /// to match your design requirements or brand guidelines.
    /// </summary>
    /// <param name="label">The LabelList to configure.</param>
    /// <param name="fontFamily">The font family name (e.g., "Arial", "Helvetica", "Times New Roman").</param>
    /// <returns>A new LabelList instance with the updated font family.</returns>
    public static LabelList FontFamily(this LabelList label, string fontFamily)
    {
        return label with { FontFamily = fontFamily };
    }

    /// <summary>
    /// Sets the number format string for the label values. This allows you to control how
    /// numerical values are displayed, including decimal places, currency symbols, and percentage formatting.
    /// </summary>
    /// <param name="label">The LabelList to configure.</param>
    /// <param name="format">The number format string (e.g., "0.0%" for percentages, "$0.00" for currency).</param>
    /// <returns>A new LabelList instance with the updated number format.</returns>
    public static LabelList NumberFormat(this LabelList label, string format)
    {
        return label with { NumberFormat = format };
    }
}