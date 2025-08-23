using System.Reflection;
using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Hooks;
using Ivy.Views;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

/// <summary>
/// Base interface for asynchronous select input controls that load options dynamically.
/// Extends IAnyInput with placeholder functionality for async select inputs that fetch
/// their options from external data sources or APIs.
/// </summary>
public interface IAnyAsyncSelectInputBase : IAnyInput
{
    /// <summary>
    /// Gets or sets the placeholder text displayed when no option is selected.
    /// Provides guidance to users about what type of selection is expected.
    /// </summary>
    /// <value>The placeholder text, or null if no placeholder should be displayed.</value>
    public string? Placeholder { get; set; }
}

/// <summary>
/// Delegate for asynchronously querying and filtering options based on user input.
/// Used to search and retrieve options that match the user's search query.
/// </summary>
/// <typeparam name="T">The type of the option values.</typeparam>
/// <param name="query">The search query string entered by the user.</param>
/// <returns>A task that resolves to an array of matching options.</returns>
public delegate Task<Option<T>[]> AsyncSelectQueryDelegate<T>(string query);

/// <summary>
/// Delegate for asynchronously looking up a specific option by its value.
/// Used to retrieve the display information for a selected value, typically
/// when initializing the input with an existing value.
/// </summary>
/// <typeparam name="T">The type of the option value.</typeparam>
/// <param name="id">The value to look up.</param>
/// <returns>A task that resolves to the option information, or null if not found.</returns>
public delegate Task<Option<T>?> AsyncSelectLookupDelegate<T>(T id);

/// <summary>
/// Asynchronous select input control that allows users to search and select from dynamically loaded options.
/// This input provides a searchable interface that queries external data sources and displays results
/// in a sheet overlay, perfect for selecting from large datasets or API-driven option lists.
/// </summary>
/// <typeparam name="TValue">The type of the selected value.</typeparam>
public class AsyncSelectInputView<TValue> : ViewBase, IAnyAsyncSelectInputBase, IInput<TValue>
{
    /// <summary>
    /// Returns an empty array as this input doesn't support automatic state type binding
    /// due to its dynamic, async nature requiring explicit query and lookup delegates.
    /// </summary>
    /// <returns>An empty array of supported types.</returns>
    public Type[] SupportedStateTypes() => [];

    /// <summary>
    /// Initializes a new instance bound to a state object for automatic value synchronization.
    /// </summary>
    /// <param name="state">The state object to bind to for automatic value updates.</param>
    /// <param name="query">Delegate for querying options based on search input.</param>
    /// <param name="lookup">Delegate for looking up option display information by value.</param>
    /// <param name="placeholder">Optional placeholder text when no option is selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    public AsyncSelectInputView(IAnyState state, AsyncSelectQueryDelegate<TValue> query, AsyncSelectLookupDelegate<TValue> lookup, string? placeholder = null, bool disabled = false)
        : this(query, lookup, placeholder, disabled)
    {
        var typedState = state.As<TValue>();
        Value = typedState.Value;
        OnChange = e => typedState.Set(e.Value);
    }

    /// <summary>
    /// Initializes a new instance with an explicit value and change handler.
    /// </summary>
    /// <param name="value">The initial selected value.</param>
    /// <param name="onChange">Event handler called when the selection changes.</param>
    /// <param name="query">Delegate for querying options based on search input.</param>
    /// <param name="lookup">Delegate for looking up option display information by value.</param>
    /// <param name="placeholder">Optional placeholder text when no option is selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    public AsyncSelectInputView(TValue value, Action<Event<IInput<TValue>, TValue>>? onChange, AsyncSelectQueryDelegate<TValue> query, AsyncSelectLookupDelegate<TValue> lookup, string? placeholder = null, bool disabled = false)
        : this(query, lookup, placeholder, disabled)
    {
        OnChange = onChange;
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with query and lookup delegates.
    /// This is the base constructor that sets up the core async select functionality.
    /// </summary>
    /// <param name="query">Delegate for querying options based on search input.</param>
    /// <param name="lookup">Delegate for looking up option display information by value.</param>
    /// <param name="placeholder">Optional placeholder text when no option is selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    public AsyncSelectInputView(AsyncSelectQueryDelegate<TValue> query, AsyncSelectLookupDelegate<TValue> lookup, string? placeholder = null, bool disabled = false)
    {
        Query = query;
        Lookup = lookup;
        Placeholder = placeholder;
        Disabled = disabled;
    }

    /// <summary>
    /// Gets the delegate used for querying options based on user search input.
    /// </summary>
    /// <value>The query delegate that searches for matching options.</value>
    public AsyncSelectQueryDelegate<TValue> Query { get; }

    /// <summary>
    /// Gets the delegate used for looking up option display information by value.
    /// </summary>
    /// <value>The lookup delegate that retrieves option details for selected values.</value>
    public AsyncSelectLookupDelegate<TValue> Lookup { get; }

    /// <summary>
    /// Gets the currently selected value.
    /// </summary>
    /// <value>The selected value, or the default value for the type if nothing is selected.</value>
    public TValue Value { get; private set; } = typeof(TValue).IsValueType ? Activator.CreateInstance<TValue>() : default!;

    /// <summary>
    /// Gets or sets whether the input accepts null values.
    /// Automatically determined based on whether TValue is a nullable type.
    /// </summary>
    /// <value>true if null values are allowed; false otherwise.</value>
    public bool Nullable { get; set; } = typeof(TValue).IsNullableType();

    /// <summary>
    /// Gets the event handler called when the selected value changes.
    /// </summary>
    /// <value>The change event handler, or null if no handler is set.</value>
    public Action<Event<IInput<TValue>, TValue>>? OnChange { get; }

    /// <summary>
    /// Gets or sets the event handler called when the input loses focus.
    /// </summary>
    /// <value>The blur event handler, or null if no handler is set.</value>
    public Action<Event<IAnyInput>>? OnBlur { get; set; }

    /// <summary>
    /// Gets or sets whether the input is disabled.
    /// </summary>
    /// <value>true if the input is disabled; false if it's interactive.</value>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the validation error message.
    /// </summary>
    /// <value>The error message, or null if the input is valid.</value>
    public string? Invalid { get; set; }

    /// <summary>
    /// Gets or sets the placeholder text displayed when no option is selected.
    /// </summary>
    /// <value>The placeholder text, or null if no placeholder should be shown.</value>
    public string? Placeholder { get; set; }

    /// <summary>
    /// Builds the async select input UI, including the input field and selection sheet.
    /// Manages state for display value, loading indicator, and sheet visibility.
    /// </summary>
    /// <returns>A fragment containing the input field and optional selection sheet.</returns>
    public override object? Build()
    {
        IState<string?> displayValue = UseState<string?>((string?)null!);
        var open = UseState(false);
        var loading = UseState(false);
        var refreshToken = this.UseRefreshToken();
        var client = UseService<IClientProvider>();

        UseEffect(async () =>
        {
            open.Set(false);

            if (refreshToken.IsRefreshed)
            {
                Value = (TValue)refreshToken.ReturnValue!;
                OnChange?.Invoke(new Event<IInput<TValue>, TValue>("OnChange", this, Value));
            }

            if (!(Value?.Equals(typeof(TValue).IsValueType ? Activator.CreateInstance<TValue>() : default!) ?? true))
            {
                loading.Set(true);
                try
                {
                    if ((await Lookup(Value)) is { } option)
                    {
                        displayValue.Set(option.Label);
                        return;
                    }
                }
                finally
                {
                    loading.Set(false);
                }
            }
            displayValue.Set((string?)null!);
        }, [EffectTrigger.AfterInit(), refreshToken]);

        void OnSelect(Event<AsyncSelectInput> _)
        {
            open.Set(true);
        }

        void OnClose(Event<Sheet> _)
        {
            open.Set(false);
        }

        return new Fragment(
            new AsyncSelectInput()
            {
                Placeholder = Placeholder,
                Disabled = Disabled,
                Invalid = Invalid,
                DisplayValue = displayValue.Value,
                OnSelect = OnSelect,
                Loading = loading.Value
            },
            open.Value ? new Sheet(
                OnClose,
                new AsyncSelectListSheet<TValue>(refreshToken, Query),
                title: Placeholder
                ) : null
        );
    }
}

/// <summary>
/// Sheet view that displays a searchable list of options for async select input.
/// Provides a filtered list interface where users can search and select from dynamically loaded options.
/// </summary>
/// <typeparam name="T">The type of the option values.</typeparam>
/// <param name="refreshToken">Token used to communicate selection back to the parent input.</param>
/// <param name="query">Delegate for querying options based on search input.</param>
public class AsyncSelectListSheet<T>(RefreshToken refreshToken, AsyncSelectQueryDelegate<T> query) : ViewBase
{
    /// <summary>
    /// Builds the searchable option list interface.
    /// Creates a filtered list view that allows users to search and select options.
    /// </summary>
    /// <returns>A filtered list view for option selection.</returns>
    public override object? Build()
    {
        var onItemClicked = new Action<Event<ListItem>>(e =>
        {
            var option = (Option<T>)e.Sender.Tag!;
            refreshToken.Refresh(option.TypedValue);
        });

        ListItem CreateItem(Option<T> option) =>
            new(title: option.Label, onClick: onItemClicked, tag: option);

        return new FilteredListView<Option<T>>(filter => query(filter), CreateItem);
    }
}

/// <summary>
/// Provides extension methods for creating async select inputs from state objects.
/// Enables fluent creation of async select inputs with automatic state binding.
/// </summary>
public static class AsyncSelectInputViewExtensions
{
    /// <summary>
    /// Creates an async select input bound to the specified state object.
    /// The input will automatically update the state when selections change.
    /// </summary>
    /// <typeparam name="TValue">The type of the selected value.</typeparam>
    /// <param name="state">The state object to bind to.</param>
    /// <param name="query">Delegate for querying options based on search input.</param>
    /// <param name="lookup">Delegate for looking up option display information by value.</param>
    /// <param name="placeholder">Optional placeholder text when no option is selected.</param>
    /// <param name="disabled">Whether the input should be disabled initially.</param>
    /// <returns>An async select input bound to the state object.</returns>
    public static IAnyAsyncSelectInputBase ToAsyncSelectInput<TValue>(
        this IAnyState state,
        AsyncSelectQueryDelegate<TValue> query,
        AsyncSelectLookupDelegate<TValue> lookup,
        string? placeholder = null,
        bool disabled = false
        )
    {
        var type = typeof(TValue);
        Type genericType = typeof(AsyncSelectInputView<>).MakeGenericType(type);

        try
        {
            IAnyAsyncSelectInputBase input = (IAnyAsyncSelectInputBase)Activator
                .CreateInstance(genericType, state, query, lookup, placeholder, disabled)!;
            return input;
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException ?? ex;
        }
    }
}

/// <summary>
/// Internal widget that represents the visual async select input control.
/// This is the actual UI widget that displays the input field with loading states and selection triggers.
/// </summary>
internal record AsyncSelectInput : WidgetBase<AsyncSelectInput>
{
    /// <summary>Gets the placeholder text displayed when no option is selected.</summary>
    [Prop] public string? Placeholder { get; init; }

    /// <summary>Gets whether the input is disabled.</summary>
    [Prop] public bool Disabled { get; init; }

    /// <summary>Gets the validation error message.</summary>
    [Prop] public string? Invalid { get; init; }

    /// <summary>Gets the display text for the currently selected option.</summary>
    [Prop] public string? DisplayValue { get; init; }

    /// <summary>Gets whether the input is currently loading option data.</summary>
    [Prop] public bool Loading { get; init; }

    /// <summary>Gets the event handler called when the user triggers option selection.</summary>
    [Event] public Action<Event<AsyncSelectInput>>? OnSelect { get; init; }
}