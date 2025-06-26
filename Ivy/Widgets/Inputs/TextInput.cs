using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

public enum TextInputs
{
    Text,
    Textarea,
    Email,
    Tel,
    Url,
    Password,
    Search
}

public interface IAnyTextInput : IAnyInput
{
    public string? Placeholder { get; set; }
    public TextInputs Variant { get; set; }
}

public abstract record TextInputBase : WidgetBase<TextInputBase>, IAnyTextInput
{
    [Prop] public bool Disabled { get; set; }
    [Prop] public string? Invalid { get; set; }
    [Prop] public string? Placeholder { get; set; }
    [Prop] public TextInputs Variant { get; set; }
    [Prop] public string? ShortcutKey { get; set; }
    [Event] public Action<Event<IAnyInput>>? OnBlur { get; set; }
    public Type[] SupportedStateTypes() => [];
}

public record TextInput<TString> : TextInputBase, IInput<TString>
{
    public TextInput(IAnyState state, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : this(placeholder, disabled, variant)
    {
        var typedState = state.As<TString>();
        Value = typedState.Value;
        OnChange = e => typedState.Set(e.Value);
    }

    public TextInput(TString value, Action<Event<IInput<TString>, TString>>? onChange = null, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : this(placeholder, disabled, variant)
    {
        OnChange = onChange;
        Value = value;
    }

    public TextInput(string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
    {
        Placeholder = placeholder;
        Variant = variant;
        Disabled = disabled;
    }

    [Prop] public TString Value { get; } = default!;
    [Event] public Action<Event<IInput<TString>, TString>>? OnChange { get; }
}

public record TextInput : TextInput<string>
{
    public TextInput(IAnyState state, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : base(state, placeholder, disabled, variant)
    {
    }

    public TextInput(string value, Action<Event<IInput<string>, string>>? onChange = null, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : base(value, onChange, placeholder, disabled, variant)
    {
    }

    public TextInput(string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
        : base(placeholder, disabled, variant)
    {
    }
}

public static class TextInputExtensions
{
    public static TextInputBase ToTextInput(this IAnyState state, string? placeholder = null, bool disabled = false, TextInputs variant = TextInputs.Text)
    {
        var type = state.GetStateType();
        Type genericType = typeof(TextInput<>).MakeGenericType(type);
        TextInputBase input = (TextInputBase)Activator.CreateInstance(genericType, state, placeholder, disabled, variant)!;
        return input;
    }

    public static TextInputBase ToTextAreaInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Textarea);
    public static TextInputBase ToSearchInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Search);
    public static TextInputBase ToPasswordInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Password);
    public static TextInputBase ToEmailInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Email);
    public static TextInputBase ToUrlInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Url);
    public static TextInputBase ToTelInput(this IAnyState state, string? placeholder = null, bool disabled = false) => state.ToTextInput(placeholder, disabled, TextInputs.Tel);

    public static TextInputBase Placeholder(this TextInputBase widget, string placeholder) => widget with { Placeholder = placeholder };
    public static TextInputBase Disabled(this TextInputBase widget, bool disabled = true) => widget with { Disabled = disabled };
    public static TextInputBase Variant(this TextInputBase widget, TextInputs variant) => widget with { Variant = variant };
    public static TextInputBase Invalid(this TextInputBase widget, string invalid) => widget with { Invalid = invalid };
    public static TextInputBase ShortcutKey(this TextInputBase widget, string shortcutKey) => widget with { ShortcutKey = shortcutKey };
}