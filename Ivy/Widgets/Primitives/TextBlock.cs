using Ivy.Core;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Defines the visual style and semantic meaning of text content.
/// </summary>
public enum TextVariant
{
    /// <summary>Plain text with no special formatting.</summary>
    Literal,
    /// <summary>Main heading (h1 element).</summary>
    H1,
    /// <summary>Secondary heading (h2 element).</summary>
    H2,
    /// <summary>Tertiary heading (h3 element).</summary>
    H3,
    /// <summary>Quaternary heading (h4 element).</summary>
    H4,
    /// <summary>Block-level text container.</summary>
    Block,
    /// <summary>Paragraph text (p element).</summary>
    P,
    /// <summary>Inline text span.</summary>
    Inline,
    /// <summary>Blockquote text for quotations.</summary>
    Blockquote,
    /// <summary>Inline code text with monospace font.</summary>
    InlineCode,
    /// <summary>Lead text for prominent introductory content.</summary>
    Lead,
    /// <summary>Large text for emphasis.</summary>
    Large,
    /// <summary>Small text for secondary information.</summary>
    Small,
    /// <summary>Muted text with reduced opacity.</summary>
    Muted,
    /// <summary>Danger/error text with red styling.</summary>
    Danger,
    /// <summary>Warning text with amber styling.</summary>
    Warning,
    /// <summary>Success text with green styling.</summary>
    Success,
    //Invalid values. Only used in Text helper.
    /// <summary>Code text variant (used internally by Text helper).</summary>
    Code,
    /// <summary>Markdown text variant (used internally by Text helper).</summary>
    Markdown,
    /// <summary>JSON text variant (used internally by Text helper).</summary>
    Json,
    /// <summary>XML text variant (used internally by Text helper).</summary>
    Xml,
    /// <summary>HTML text variant (used internally by Text helper).</summary>
    Html,
    /// <summary>LaTeX text variant (used internally by Text helper).</summary>
    Latex,
    /// <summary>Label text for form fields and UI labels.</summary>
    Label,
    /// <summary>Strong/bold text for emphasis.</summary>
    Strong
}

/// <summary>
/// A low-level text widget that renders text content with customizable styling and variants.
/// </summary>
/// <remarks>
/// This widget is rarely used directly. Instead, use the <c>Text</c> helper class which provides 
/// a more user-friendly API for creating text elements with various styles and formatting options.
/// </remarks>
public record TextBlock : WidgetBase<TextBlock>
{
    /// <summary>
    /// Initializes a new TextBlock with the specified content and styling options.
    /// </summary>
    /// <param name="content">The text content to display.</param>
    /// <param name="variant">The text variant that determines styling and semantic meaning.</param>
    /// <param name="width">Optional width constraint for the text.</param>
    /// <param name="strikeThrough">Whether to apply strikethrough styling.</param>
    /// <param name="color">Optional color override for the text.</param>
    /// <param name="noWrap">Whether to prevent text wrapping.</param>
    /// <param name="overflow">How to handle text overflow.</param>
    internal TextBlock(string content = "", TextVariant variant = TextVariant.Literal, Size? width = null,
        bool strikeThrough = false, Colors? color = null, bool noWrap = false, Overflow? overflow = null)
    {
        Content = content;
        Variant = variant;
        StrikeThrough = strikeThrough;
        Width = width;
        Color = color;
        NoWrap = noWrap;
        Overflow = overflow;
    }

    /// <summary>Gets or sets how text overflow is handled.</summary>
    /// <value>The overflow behavior for text that exceeds the container bounds.</value>
    [Prop] public Overflow? Overflow { get; set; }

    /// <summary>Gets or sets whether text wrapping is disabled.</summary>
    /// <value>True to prevent text from wrapping to new lines, false to allow wrapping.</value>
    [Prop] public bool NoWrap { get; set; }

    /// <summary>Gets or sets the text content to display.</summary>
    /// <value>The text content string.</value>
    [Prop] public string Content { get; set; }

    /// <summary>Gets or sets the text variant that determines styling and semantic meaning.</summary>
    /// <value>The text variant from the TextVariant enumeration.</value>
    [Prop] public TextVariant Variant { get; set; }

    /// <summary>Gets or sets whether strikethrough styling is applied.</summary>
    /// <value>True to apply strikethrough styling, false for normal text.</value>
    [Prop] public bool StrikeThrough { get; set; }

    /// <summary>Gets or sets the color override for the text.</summary>
    /// <value>The color from the Colors enumeration, or null to use the default color for the variant.</value>
    [Prop] public Colors? Color { get; set; }
}