using Ivy.Core;

namespace Ivy.Widgets.Internal;

/// <summary>
/// Represents a single line of terminal output or input, including the content text, command status,
/// and optional custom prompt. This record is used to distinguish between user commands and system output
/// in terminal displays, allowing for proper formatting and visual representation of terminal interactions.
/// </summary>
/// <param name="Content">The text content of the terminal line.</param>
/// <param name="IsCommand">Whether this line represents a user command (true) or system output (false). Default is false.</param>
/// <param name="Prompt">The prompt symbol to display before the line content. Default is ">".</param>
public record TerminalLine(string Content, bool IsCommand = false, string Prompt = ">");

/// <summary>
/// Represents a terminal widget that displays command-line interface interactions with support for
/// command input, output display, and terminal-like formatting. This widget is ideal for creating
/// command-line interfaces, displaying command history, showing system logs, or simulating terminal
/// environments within web applications.
/// 
/// The Terminal widget maintains an array of TerminalLine records, each representing either a user
/// command or system output, allowing for rich terminal-like experiences with proper command/output
/// distinction and customizable prompts.
/// </summary>
public record Terminal : WidgetBase<Terminal>
{
    /// <summary>
    /// Gets or sets the array of terminal lines that make up the terminal content.
    /// Each line can represent either a user command or system output, with the TerminalLine
    /// record providing the content, command status, and prompt information for proper formatting.
    /// 
    /// The lines are displayed in chronological order, creating a scrollable terminal history
    /// that shows the complete interaction sequence between user commands and system responses.
    /// Default is an empty array (no terminal content).
    /// </summary>
    [Prop] public TerminalLine[] Lines { get; init; } = [];

    /// <summary>
    /// Gets or sets the optional title displayed at the top of the terminal widget.
    /// This title provides context about the terminal's purpose, such as "System Console",
    /// "Command History", or "Debug Terminal", helping users understand what the terminal
    /// is displaying or controlling.
    /// 
    /// When null, no title is displayed, creating a clean terminal interface without header text.
    /// Default is null (no title displayed).
    /// </summary>
    [Prop] public string? Title { get; init; }

    /// <summary>
    /// Gets or sets whether the terminal header (including title and terminal controls) is displayed.
    /// When true, the terminal shows a header section that may include the title, close buttons,
    /// or other terminal-specific controls. When false, the terminal displays only the content
    /// lines without any header elements.
    /// 
    /// This property allows you to create both full-featured terminals with headers and minimal
    /// terminal displays that focus purely on content output.
    /// Default is true (header is displayed).
    /// </summary>
    [Prop] public bool ShowHeader { get; init; } = true;
}

/// <summary>
/// Provides extension methods for the Terminal widget that enable a fluent API for building
/// terminal content and configuring terminal behavior. These methods allow you to easily add
/// commands and output, set titles, and build terminal interactions in a readable, chainable manner.
/// </summary>
public static class TerminalExtensions
{
    /// <summary>
    /// Private helper method that adds a new line to the terminal with the specified content,
    /// command status, and prompt. This method creates a new TerminalLine record and appends
    /// it to the existing Lines array, returning a new Terminal instance with the updated content.
    /// </summary>
    /// <param name="terminal">The terminal to add the line to.</param>
    /// <param name="content">The text content for the new terminal line.</param>
    /// <param name="isCommand">Whether the line represents a user command (true) or system output (false). Default is false.</param>
    /// <param name="prompt">The prompt symbol to display before the line content. Default is ">".</param>
    /// <returns>A new Terminal instance with the additional line appended to the Lines array.</returns>
    private static Terminal AddLine(this Terminal terminal, string content, bool isCommand = false, string prompt = ">")
    {
        return terminal with { Lines = terminal.Lines.Append(new TerminalLine(content, isCommand, prompt)).ToArray() };
    }

    /// <summary>
    /// Adds a user command line to the terminal with the specified command text.
    /// This method creates a new terminal line marked as a command (IsCommand = true) and
    /// appends it to the terminal's line history, allowing users to see their command input
    /// in the terminal display.
    /// </summary>
    /// <param name="terminal">The terminal to add the command to.</param>
    /// <param name="command">The command text to display in the terminal.</param>
    /// <returns>A new Terminal instance with the command line added to the Lines array.</returns>
    public static Terminal AddCommand(this Terminal terminal, string command)
    {
        return terminal.AddLine(command, true);
    }

    /// <summary>
    /// Adds a system output line to the terminal with the specified output text.
    /// This method creates a new terminal line marked as output (IsCommand = false) and
    /// appends it to the terminal's line history, representing system responses, command
    /// results, or informational messages displayed to the user.
    /// </summary>
    /// <param name="terminal">The terminal to add the output to.</param>
    /// <param name="output">The output text to display in the terminal.</param>
    /// <returns>A new Terminal instance with the output line added to the Lines array.</returns>
    public static Terminal AddOutput(this Terminal terminal, string output)
    {
        return terminal.AddLine(output);
    }

    /// <summary>
    /// Sets the title of the terminal widget to the specified text.
    /// This method allows you to configure the terminal title that will be displayed
    /// in the header (when ShowHeader is true), providing context about the terminal's
    /// purpose or current state.
    /// </summary>
    /// <param name="terminal">The terminal to set the title for.</param>
    /// <param name="title">The title text to display in the terminal header.</param>
    /// <returns>A new Terminal instance with the updated title.</returns>
    public static Terminal Title(this Terminal terminal, string title)
    {
        return terminal with { Title = title };
    }
}