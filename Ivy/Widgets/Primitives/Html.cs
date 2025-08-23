using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// A widget for rendering raw HTML content directly within the application interface.
/// Provides the ability to embed custom HTML markup, third-party widgets, or legacy content
/// while maintaining integration with the Ivy widget system and ensuring proper security considerations.
/// </summary>
/// <remarks>
/// The Html widget is designed for scenarios requiring direct HTML rendering:
/// <list type="bullet">
/// <item><description><strong>Legacy integration:</strong> Embed existing HTML content or legacy components into modern Ivy applications</description></item>
/// <item><description><strong>Third-party widgets:</strong> Include external HTML widgets, embeds, or interactive content from other systems</description></item>
/// <item><description><strong>Custom markup:</strong> Render specialized HTML structures not available through standard Ivy widgets</description></item>
/// <item><description><strong>Rich content:</strong> Display complex formatted content with custom styling and interactive elements</description></item>
/// </list>
/// <para><strong>Security Warning:</strong> Raw HTML content should be properly sanitized to prevent XSS attacks and other security vulnerabilities. Always validate and sanitize user-provided HTML content before rendering.</para>
/// </remarks>
public record Html : WidgetBase<Html>
{
    /// <summary>
    /// Initializes a new HTML widget with the specified raw HTML content.
    /// Creates a widget that renders the provided HTML markup directly within the application,
    /// enabling integration of custom HTML structures and third-party content.
    /// </summary>
    /// <param name="content">The raw HTML content to render. Should be properly formatted and sanitized HTML markup.</param>
    /// <remarks>
    /// The Html constructor enables direct HTML rendering with important considerations:
    /// <list type="bullet">
    /// <item><description><strong>Direct rendering:</strong> HTML content is rendered as-is without modification or escaping</description></item>
    /// <item><description><strong>Security responsibility:</strong> Content should be sanitized to prevent XSS and other security issues</description></item>
    /// <item><description><strong>Formatting preservation:</strong> Maintains original HTML structure, styling, and interactive elements</description></item>
    /// <item><description><strong>Integration support:</strong> Allows embedding of external widgets and legacy content</description></item>
    /// </list>
    /// <para>Use this widget when standard Ivy widgets cannot provide the required functionality or when integrating existing HTML-based content.</para>
    /// </remarks>
    public Html(string content)
    {
        Content = content;
    }

    /// <summary>Gets or sets the raw HTML content to be rendered by the widget.</summary>
    /// <value>The HTML markup string that will be rendered directly in the application interface.</value>
    /// <remarks>
    /// The Content property holds the raw HTML markup that will be rendered without modification:
    /// <list type="bullet">
    /// <item><description><strong>Raw HTML:</strong> Content is rendered as-is, preserving all HTML tags, attributes, and structure</description></item>
    /// <item><description><strong>Interactive elements:</strong> Supports JavaScript, CSS, and interactive HTML elements within the content</description></item>
    /// <item><description><strong>Styling preservation:</strong> Maintains custom CSS styles and formatting from the original HTML</description></item>
    /// <item><description><strong>Security considerations:</strong> Content should be validated and sanitized to prevent security vulnerabilities</description></item>
    /// </list>
    /// <para>When updating this property, ensure the new HTML content is properly formatted and secure for rendering in the application context.</para>
    /// </remarks>
    [Prop] public string Content { get; set; }
}