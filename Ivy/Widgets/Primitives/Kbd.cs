using Ivy.Core;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// A widget for displaying keyboard keys and key combinations with distinctive styling.
/// Provides semantic representation of keyboard input with appropriate visual styling to indicate
/// keys, shortcuts, and interactive elements that users can press or activate through keyboard interaction.
/// </summary>
/// <remarks>
/// The Kbd widget is designed for comprehensive keyboard input representation:
/// <list type="bullet">
/// <item><description><strong>Keyboard shortcuts:</strong> Display key combinations and shortcuts in documentation, help text, and user interfaces</description></item>
/// <item><description><strong>Interactive guidance:</strong> Show users which keys to press for specific actions and navigation</description></item>
/// <item><description><strong>Documentation:</strong> Provide clear visual indication of keyboard inputs in tutorials, help systems, and API documentation</description></item>
/// <item><description><strong>Accessibility:</strong> Semantic markup that screen readers can properly interpret as keyboard input instructions</description></item>
/// </list>
/// <para>The widget automatically applies distinctive styling (typically bordered, monospace font) to make keyboard keys visually distinct from regular text, following web standards and accessibility guidelines for keyboard input representation.</para>
/// </remarks>
public record Kbd : WidgetBase<Kbd>
{
    /// <summary>
    /// Initializes a new keyboard input widget with the specified key or key combination content.
    /// Creates a semantically marked keyboard input display with distinctive styling to clearly indicate
    /// keyboard keys, shortcuts, or interactive elements that users can activate through keyboard input.
    /// </summary>
    /// <param name="content">The keyboard key, key combination, or input instruction to display. Can be text, other widgets, or complex key combinations.</param>
    /// <remarks>
    /// The Kbd constructor provides flexible keyboard input representation with semantic meaning:
    /// <list type="bullet">
    /// <item><description><strong>Single keys:</strong> Display individual keys like "Enter", "Escape", "Space", or letter/number keys</description></item>
    /// <item><description><strong>Key combinations:</strong> Show modifier combinations like "Ctrl+C", "Alt+Tab", or "Cmd+Shift+P"</description></item>
    /// <item><description><strong>Complex content:</strong> Support for nested widgets to create sophisticated key combination displays</description></item>
    /// <item><description><strong>Semantic markup:</strong> Provides proper semantic meaning for assistive technologies and screen readers</description></item>
    /// </list>
    /// <para>Use this widget whenever you need to indicate keyboard input to users, whether in documentation, help text, tooltips, or interactive guidance. The distinctive styling helps users quickly identify actionable keyboard inputs.</para>
    /// </remarks>
    public Kbd(object content) : base(content)
    {
    }
}