using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Defines the visual variants available for select input controls.
/// Each variant provides a different user interface for option selection with distinct interaction patterns.
/// </summary>
public enum SelectInputs
{
    /// <summary>Standard dropdown select control with a collapsible option list.</summary>
    Select,
    /// <summary>List-style selection with all options visible simultaneously.</summary>
    List,
    /// <summary>Toggle-style selection for boolean or small option sets.</summary>
    Toggle
}

/// <summary>
/// Interface for select input controls that extends IAnyInput with selection-specific properties.
/// Provides functionality for option-based selection with placeholder text and visual variant configuration
/// for dropdown, list, and toggle-style selection interfaces.
/// </summary>
public interface IAnySelectInput : IAnyInput
{
    /// <summary>Gets or sets the placeholder text displayed when no option is selected.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    public string? Placeholder { get; set; }

    /// <summary>Gets or sets the visual variant of the select input.</summary>
    /// <value>The selection variant (Select, List, or Toggle).</value>
    public SelectInputs Variant { get; set; }
}

/// <summary>
/// Abstract base class for select input controls that provides common selection functionality.
/// Supports single and multiple selection modes with configurable visual variants, placeholder text,
/// and customizable value separation for multi-select scenarios.
/// </summary>
public abstract record SelectInputBase : WidgetBase<SelectInputBase>, IAnySelectInput
{
    /// <summary>Gets or sets whether the input is disabled.</summary>
    /// <value>true if the input is disabled; false if it's interactive.</value>
    [Prop] public bool Disabled { get; set; }

    /// <summary>Gets or sets the validation error message.</summary>
    /// <value>The error message, or null if the input is valid.</value>
    [Prop] public string? Invalid { get; set; }

    /// <summary>Gets or sets the placeholder text displayed when no option is selected.</summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    [Prop] public string? Placeholder { get; set; }

    /// <summary>Gets or sets the visual variant of the select input.</summary>
    /// <value>The selection variant (Select, List, or Toggle).</value>
    [Prop] public SelectInputs Variant { get; set; }

    /// <summary>Gets or sets whether multiple options can be selected simultaneously.</summary>
    /// <value>true for multi-select mode; false for single selection (default).</value>
    [Prop] public bool SelectMany { get; set; } = false;

    /// <summary>Gets or sets the character used to separate multiple selected values in display and serialization.</summary>
    /// <value>The separator character (default is semicolon ';').</value>
    [Prop] public char Separator { get; set; } = ';';

    /// <summary>Gets or sets the event handler called when the input loses focus.</summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    [Event] public Action<Event<IAnyInput>>? OnBlur { get; set; }

    /// <summary>
    /// Returns the types that this select input can bind to and work with.
    /// Base implementation returns empty array; derived classes should override to specify supported types.
    /// </summary>
    /// <returns>An empty array in the base implementation.</returns>
    public Type[] SupportedStateTypes() => [];
}

/// <summary>
/// Generic select input control that provides type-safe option selection functionality.
/// Supports single and multiple selection modes with various visual variants (dropdown, list, toggle),
/// automatic enum handling, and comprehensive state binding for option-based user input scenarios.
/// </summary>
/// <typeparam name="TValue">The type of the selected value(s) - can be single values or collections for multi-select.</typeparam>
public record SelectInput<TValue> : SelectInputBase, IInput<TValue>, IAnySelectInput
{
    /// <summary>
    /// Initializes a new select input bound to a state object for automatic value synchronization.
    /// The input will display the current state value and update the state when selection changes.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates and change handling.</param>
    /// <param name="options">The collection of options available for selection.</param>
    /// <param name="placeholder">Optional placeholder text displayed when no option is selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the select input.</param>
    /// <param name="selectMany">Whether multiple options can be selected simultaneously.</param>
    public SelectInput(IAnyState state, IEnumerable<IAnyOption> options, string? placeholder = null, bool disabled = false, SelectInputs variant = SelectInputs.Select, bool selectMany = false)
        : this(options, placeholder, disabled, variant, selectMany)
    {
        var typedState = state.As<TValue>();
        Value = typedState.Value;
        OnChange = e => typedState.Set(e.Value);
    }

    /// <summary>
    /// Initializes a new select input with an explicit value and change handler.
    /// Useful for manual state management or when custom change handling is required.
    /// </summary>
    /// <param name="value">The initial selected value.</param>
    /// <param name="onChange">The event handler called when the selection changes.</param>
    /// <param name="options">The collection of options available for selection.</param>
    /// <param name="placeholder">Optional placeholder text displayed when no option is selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the select input.</param>
    /// <param name="selectMany">Whether multiple options can be selected simultaneously.</param>
    public SelectInput(TValue value, Action<Event<IInput<TValue>, TValue>>? onChange, IEnumerable<IAnyOption> options, string? placeholder = null, bool disabled = false, SelectInputs variant = SelectInputs.Select, bool selectMany = false)
        : this(options, placeholder, disabled, variant, selectMany)
    {
        OnChange = onChange;
        Value = value;
    }

    /// <summary>
    /// Initializes a new select input with basic configuration.
    /// Requires separate value and change handler assignment for functionality.
    /// </summary>
    /// <param name="options">The collection of options available for selection.</param>
    /// <param name="placeholder">Optional placeholder text displayed when no option is selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the select input.</param>
    /// <param name="selectMany">Whether multiple options can be selected simultaneously.</param>
    public SelectInput(IEnumerable<IAnyOption> options, string? placeholder = null, bool disabled = false, SelectInputs variant = SelectInputs.Select, bool selectMany = false)
    {
        Placeholder = placeholder;
        Variant = variant;
        Disabled = disabled;
        Options = [.. options];
        SelectMany = selectMany;
    }

    /// <summary>Gets the currently selected value(s).</summary>
    /// <value>The selected value of type TValue, which can be a single value or collection depending on SelectMany mode.</value>
    [Prop] public TValue Value { get; } = default!;

    /// <summary>Gets or sets whether the input accepts null values.</summary>
    /// <value>true if null values are allowed; automatically determined based on TValue type nullability.</value>
    [Prop] public bool Nullable { get; set; } = typeof(TValue).IsNullableType();

    /// <summary>Gets or sets the collection of options available for selection.</summary>
    /// <value>An array of IAnyOption instances representing the selectable choices.</value>
    [Prop] public IAnyOption[] Options { get; set; }

    /// <summary>Gets the event handler called when the selection changes.</summary>
    /// <value>The change event handler that receives the select input and the new selected value(s), or null if no handler is set.</value>
    [Event] public Action<Event<IInput<TValue>, TValue>>? OnChange { get; }
}

/// <summary>
/// Provides extension methods for creating and configuring select input controls with fluent syntax.
/// Includes intelligent type detection, automatic enum handling, multi-select support,
/// and comprehensive configuration methods for option-based selection controls.
/// </summary>
public static class SelectInputExtensions
{
    /// <summary>
    /// Creates a select input from a state object with automatic type detection and intelligent defaults.
    /// Automatically detects enum types and collection types for multi-select scenarios,
    /// generates appropriate options, and configures the input based on the state's type characteristics.
    /// </summary>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="options">Optional collection of options; if null, attempts automatic generation for enums and collections.</param>
    /// <param name="placeholder">Optional placeholder text displayed when no option is selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <param name="variant">The visual variant of the select input.</param>
    /// <returns>A select input bound to the state object with type-appropriate configuration.</returns>
    /// <exception cref="ArgumentException">Thrown when options are null and cannot be automatically generated for the state type.</exception>
    public static SelectInputBase ToSelectInput(this IAnyState state, IEnumerable<IAnyOption>? options = null, string? placeholder = null, bool disabled = false, SelectInputs variant = SelectInputs.Select)
    {
        var type = state.GetStateType();
        bool selectMany = type.IsCollectionType();
        Type genericType = typeof(SelectInput<>).MakeGenericType(type);

        if (options == null)
        {
            if (type.IsEnum)
            {
                options = type.ToOptions();
            }
            else if (selectMany && type.GetCollectionTypeParameter() is { } itemType)
            {
                options = itemType.ToOptions();
            }
            else
            {
                throw new ArgumentException("Options must be provided for non-enum types.", nameof(options));
            }
        }

        // Set a default placeholder for multi-selects if not provided
        if (selectMany && string.IsNullOrWhiteSpace(placeholder))
        {
            placeholder = "Select options...";
        }

        SelectInputBase input = (SelectInputBase)Activator.CreateInstance(genericType, state, options, placeholder, disabled, variant, selectMany)!;
        return input;
    }

    /// <summary>Sets the placeholder text for the select input.</summary>
    /// <param name="widget">The select input to configure.</param>
    /// <param name="title">The placeholder text to display when no option is selected.</param>
    /// <returns>The select input with the specified placeholder text.</returns>
    public static SelectInputBase Placeholder(this SelectInputBase widget, string title)
    {
        return widget with { Placeholder = title };
    }

    /// <summary>Sets the disabled state of the select input.</summary>
    /// <param name="widget">The select input to configure.</param>
    /// <param name="disabled">Whether the input should be disabled.</param>
    /// <returns>The select input with the specified disabled state.</returns>
    public static SelectInputBase Disabled(this SelectInputBase widget, bool disabled = true)
    {
        return widget with { Disabled = disabled };
    }

    /// <summary>Sets the visual variant of the select input.</summary>
    /// <param name="widget">The select input to configure.</param>
    /// <param name="variant">The visual variant (Select, List, or Toggle).</param>
    /// <returns>The select input with the specified variant.</returns>
    public static SelectInputBase Variant(this SelectInputBase widget, SelectInputs variant)
    {
        return widget with { Variant = variant };
    }

    /// <summary>Sets the validation error message for the select input.</summary>
    /// <param name="widget">The select input to configure.</param>
    /// <param name="invalid">The validation error message to display, or null to clear the error.</param>
    /// <returns>The select input with the specified validation error.</returns>
    public static SelectInputBase Invalid(this SelectInputBase widget, string? invalid)
    {
        return widget with { Invalid = invalid };
    }

    /// <summary>Sets the separator character used for multi-select value display and serialization.</summary>
    /// <param name="widget">The select input to configure.</param>
    /// <param name="separator">The character to use for separating multiple selected values.</param>
    /// <returns>The select input with the specified separator character.</returns>
    public static SelectInputBase Separator(this SelectInputBase widget, char separator)
    {
        return widget with { Separator = separator };
    }

    /// <summary>
    /// Configures the select input to use the List variant for displaying all options simultaneously.
    /// Convenience method that sets the variant to SelectInputs.List.
    /// </summary>
    /// <param name="widget">The select input to configure.</param>
    /// <returns>The select input configured with the List variant.</returns>
    public static SelectInputBase List(this SelectInputBase widget)
    {
        return widget with { Variant = SelectInputs.List };
    }
}