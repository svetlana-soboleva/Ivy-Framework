using Ivy.Core;
using Ivy.Core.Hooks;
using Ivy.Shared;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Represents a progress widget that visually represents the completion status
/// of a task or process. This widget displays a visual progress bar that can
/// be customized with different color variants and can be bound to state for
/// dynamic updates and real-time progress tracking.
/// 
/// The Progress widget is ideal for showing task completion, file upload progress,
/// loading states, and any other process that benefits from visual progress
/// representation. It supports both static values and state-bound updates for
/// dynamic progress tracking throughout application workflows.
/// </summary>
public record Progress : WidgetBase<Progress>
{
    /// <summary>
    /// Defines the available color variants for progress bars, controlling
    /// their visual appearance and color scheme to match different design
    /// requirements and application themes.
    /// </summary>
    public enum ColorVariants
    {
        /// <summary>Default primary color variant with standard progress bar styling.</summary>
        Primary,
        /// <summary>Emerald gradient variant with enhanced visual appeal and modern styling.</summary>
        EmeraldGradient
    }

    /// <summary>
    /// Initializes a new instance of the Progress class with a state-bound value.
    /// This constructor automatically updates the progress bar whenever the
    /// bound state value changes, enabling dynamic progress tracking.
    /// </summary>
    /// <param name="state">The IState&lt;int&gt; object that provides the progress
    /// value. The progress bar will automatically update whenever this state
    /// changes, enabling real-time progress tracking and dynamic updates.</param>
    public Progress(IState<int> state) : this(state.Value)
    {
    }

    /// <summary>
    /// Initializes a new instance of the Progress class with the specified
    /// progress value. The progress bar will display the completion percentage
    /// based on the provided value, with 0 representing no progress and 100
    /// representing complete progress.
    /// </summary>
    /// <param name="value">The progress value as a percentage from 0 to 100.
    /// This value determines the visual completion level of the progress bar,
    /// with null representing an indeterminate or loading state.</param>
    public Progress(int? value = 0)
    {
        Value = value;
        Width = Size.Full();
    }

    /// <summary>
    /// Gets or sets the current progress value as a percentage.
    /// This property controls the visual completion level of the progress bar,
    /// with values ranging from 0 (no progress) to 100 (complete).
    /// 
    /// When set to null, the progress bar displays an indeterminate or loading
    /// state, useful for processes where the exact progress cannot be determined.
    /// The progress bar automatically adjusts its visual representation based
    /// on this value, providing clear visual feedback to users.
    /// Default is 0 (no progress).
    /// </summary>
    [Prop] public int? Value { get; set; }

    /// <summary>
    /// Gets or sets the goal or description text displayed alongside the progress bar.
    /// This property provides context about what the progress represents, helping
    /// users understand what task or process is being tracked.
    /// 
    /// The goal text is typically displayed above or below the progress bar and
    /// can be updated dynamically to reflect the current stage of the process.
    /// When null, no goal text is displayed, creating a cleaner visual appearance.
    /// Default is null (no goal text).
    /// </summary>
    [Prop] public string? Goal { get; set; }

    /// <summary>
    /// Gets or sets the color variant for the progress bar.
    /// This property controls the visual styling and color scheme of the
    /// progress bar, allowing you to choose between different visual themes
    /// to match your application's design requirements.
    /// 
    /// Different color variants provide different visual styles, from standard
    /// primary colors to enhanced gradient effects, enabling you to create
    /// progress bars that complement your application's visual design.
    /// Default is <see cref="ColorVariants.Primary"/>.
    /// </summary>
    [Prop] public ColorVariants ColorVariant { get; set; } = ColorVariants.Primary;

    /// <summary>
    /// Operator overload that prevents adding children to the Progress using the pipe operator.
    /// Progress widgets are self-contained components that don't support additional
    /// child content beyond their configured progress value and styling properties.
    /// 
    /// This restriction ensures that progress bars maintain their intended focused
    /// design and prevent accidental modification of their structure.
    /// </summary>
    /// <param name="widget">The Progress widget.</param>
    /// <param name="child">The child content to add (not supported).</param>
    /// <returns>This method always throws a NotSupportedException.</returns>
    /// <exception cref="NotSupportedException">Always thrown, as Progress does not support additional children.</exception>
    public static Progress operator |(Progress widget, object child)
    {
        throw new NotSupportedException("Progress does not support children.");
    }
}

/// <summary>
/// Provides extension methods for the Progress widget that enable a fluent API for
/// configuring progress appearance, behavior, and content. These methods allow you
/// to easily set progress values, goals, and color variants for optimal progress
/// bar presentation and functionality.
/// </summary>
public static class ProgressExtensions
{
    /// <summary>
    /// Sets the progress value using a state-bound integer value.
    /// This method allows you to bind the progress bar to a state object,
    /// enabling automatic updates whenever the state value changes.
    /// 
    /// State binding is useful for creating dynamic progress bars that
    /// automatically reflect changes in application state, such as file
    /// upload progress, task completion, or loading states.
    /// </summary>
    /// <param name="progress">The Progress widget to configure.</param>
    /// <param name="value">The IState&lt;int&gt; object that provides the progress value.</param>
    /// <returns>A new Progress instance with the updated value from the state.</returns>
    public static Progress Value(this Progress progress, IState<int> value)
    {
        return progress with { Value = value.Value };
    }

    /// <summary>
    /// Sets the goal or description text for the progress bar.
    /// This method allows you to provide context about what the progress
    /// represents, helping users understand the current task or process
    /// being tracked.
    /// 
    /// Goal text can be updated dynamically to reflect different stages
    /// of a process, such as "Preparing files...", "Uploading...", or
    /// "Processing..." for file upload workflows.
    /// </summary>
    /// <param name="progress">The Progress widget to configure.</param>
    /// <param name="goal">The goal text to display alongside the progress bar, or null to remove the goal.</param>
    /// <returns>A new Progress instance with the updated goal text.</returns>
    public static Progress Goal(this Progress progress, string? goal)
    {
        return progress with { Goal = goal };
    }

    /// <summary>
    /// Sets the color variant for the progress bar.
    /// This method allows you to change the visual styling and color scheme
    /// of the progress bar, enabling you to match different design requirements
    /// and application themes.
    /// 
    /// Color variants provide different visual styles, from standard primary
    /// colors to enhanced gradient effects, allowing you to create progress
    /// bars that complement your application's visual design.
    /// </summary>
    /// <param name="progress">The Progress widget to configure.</param>
    /// <param name="variant">The color variant to apply to the progress bar.</param>
    /// <returns>A new Progress instance with the updated color variant.</returns>
    public static Progress ColorVariant(this Progress progress, Progress.ColorVariants variant)
    {
        return progress with { ColorVariant = variant };
    }
}