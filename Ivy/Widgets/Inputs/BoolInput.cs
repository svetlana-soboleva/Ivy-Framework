using Ivy.Core;
using Ivy.Core.Helpers;
using Ivy.Core.Hooks;
using Ivy.Shared;
using Ivy.Widgets.Inputs;

// ReSharper disable once CheckNamespace
namespace Ivy;

public enum BoolInputs
{
    Checkbox,
    Switch,
    Toggle
}

public interface IAnyBoolInput : IAnyInput
{
    public string? Label { get; set; }
    public string? Description { get; set; }
    public BoolInputs Variant { get; set; }
    public Icons Icon { get; set; }
}

public abstract record BoolInputBase : WidgetBase<BoolInputBase>, IAnyBoolInput
{
    [Prop] public bool Disabled { get; set; }
    [Prop] public string? Invalid { get; set; }
    [Prop] public string? Label { get; set; }
    [Prop] public string? Description { get; set; }
    [Prop] public BoolInputs Variant { get; set; }
    [Prop] public Icons Icon { get; set; }
    [Event] public Action<Event<IAnyInput>>? OnBlur { get; set; }
}

public record BoolInput<TBool> : BoolInputBase, IInput<TBool>
{
    public BoolInput(IAnyState state, string? label = null, bool disabled = false, BoolInputs variant = BoolInputs.Checkbox) 
        : this(label, disabled, variant)
    {
        var typedState = state.As<TBool>();
        Value = typedState.Value;
        OnChange = e => typedState.Set(e.Value);
    }
    
    public BoolInput(TBool value, Action<Event<IInput<TBool>, TBool>> onChange, string? label = null, bool disabled = false, BoolInputs variant = BoolInputs.Checkbox) : this(label, disabled, variant)
    {
        OnChange = onChange;
        Value = value;
    }

    public BoolInput(string? label = null, bool disabled = false, BoolInputs variant = BoolInputs.Checkbox)
    {
        Label = label;
        Disabled = disabled;
        Variant = variant;
    }

    [Prop] public TBool Value { get; } = default!;
    [Prop] public bool Nullable { get; set; } = typeof(TBool) == typeof(bool?);
    [Event] public Action<Event<IInput<TBool>, TBool>>? OnChange { get; }
}

public static class BoolInputExtensions
{
    public static BoolInputBase ToBoolInput(this IAnyState state, string? label = null, bool disabled = false, BoolInputs variant = BoolInputs.Checkbox)
    {
        var type = state.GetStateType();
        Type genericType = typeof(BoolInput<>).MakeGenericType(type);
        BoolInputBase input = (BoolInputBase)Activator.CreateInstance(genericType, state, label, disabled, variant)!;
        input.ScaffoldDefaults(null!, type);
        return input;
    }
    
    public static BoolInputBase ToSwitchInput(this IAnyState state, string? label = null, bool disabled = false)
    {
        return state.ToBoolInput(label, disabled, BoolInputs.Switch);
    }
    
    public static BoolInputBase ToToggleInput(this IAnyState state, Icons? icon = null, string? label = null, bool disabled = false)
    {
        var input = state.ToBoolInput(label, disabled, BoolInputs.Toggle);
        if(icon != null)
        {
            input.Icon = icon.Value;
        }
        return input;
    }

    internal static IAnyBoolInput ScaffoldDefaults(this IAnyBoolInput input, string? name, Type type)
    {
        if(string.IsNullOrEmpty(input.Label))
        {
            input.Label = Utils.SplitPascalCase(name) ?? name;
        }
        return input;
    }
    
    public static BoolInputBase Label(this BoolInputBase widget, string label)
    {
        return widget with { Label = label };
    }
    
    public static BoolInputBase Disabled(this BoolInputBase widget, bool disabled = true)
    {
        return widget with { Disabled = disabled };
    }
    
    public static BoolInputBase Variant(this BoolInputBase widget, BoolInputs variant)
    {
        return widget with { Variant = variant };
    }
    
    public static BoolInputBase Icon(this BoolInputBase widget, Icons icon)
    {
        return widget with { Icon = icon };
    }
    
    public static BoolInputBase Description(this BoolInputBase widget, string description)
    {
        return widget with { Description = description };
    }
    
    public static BoolInputBase Invalid(this BoolInputBase widget, string? invalid)
    {
        return widget with { Invalid = invalid };
    }
}