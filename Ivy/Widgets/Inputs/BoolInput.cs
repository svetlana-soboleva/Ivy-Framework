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
    public Type[] SupportedStateTypes() => [
        // Boolean types
        typeof(bool), typeof(bool?),
        // Signed integer types
        typeof(sbyte), typeof(sbyte?), typeof(short), typeof(short?), 
        typeof(int), typeof(int?), typeof(long), typeof(long?),
        typeof(Int128), typeof(Int128?),
        // Unsigned integer types
        typeof(byte), typeof(byte?), typeof(ushort), typeof(ushort?),
        typeof(uint), typeof(uint?), typeof(ulong), typeof(ulong?),
        typeof(UInt128), typeof(UInt128?),
        // Floating-point types
        typeof(float), typeof(float?),
        typeof(double), typeof(double?), typeof(decimal), typeof(decimal?)
    ];
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


    private static IState<bool> ConvertToBoolState(IAnyState state)
    {
        var stateType = state.GetStateType();
        
        return stateType switch
        {
            // Boolean types - direct conversion
            _ when stateType == typeof(bool) => state.As<bool>(),
            _ when stateType == typeof(bool?) => new State<bool>(state.As<bool?>().Value ?? false),
            
            // Numeric types - convert to boolean (0 = false, non-zero = true)
            _ when stateType.IsNumeric() => new State<bool>(Convert.ToBoolean(state.As<object>().Value)),
            
            // Other types - try BestGuessConvert, fallback to false
            _ => new State<bool>(Core.Utils.BestGuessConvert(state.As<object>().Value, typeof(bool)) is bool b && b)
        };
    }

    private static TBool ConvertToTBool(bool boolValue)
    {
        if (typeof(TBool) == typeof(bool))
            return (TBool)(object)boolValue;
        if (typeof(TBool) == typeof(bool?))
            return (TBool)(object)boolValue;
        
        // For other types, convert using BestGuessConvert
        var converted = Core.Utils.BestGuessConvert(boolValue, typeof(TBool));
        return converted != null ? (TBool)converted : default!;
    }

    private static bool ConvertToBool(TBool value)
    {
        if (typeof(TBool) == typeof(bool))
            return (bool)(object)value;
        if (typeof(TBool) == typeof(bool?))
            return ((bool?)(object)value) ?? false;
        
        // For other types, convert to boolean
        return Convert.ToBoolean(value);
    }
}

public static class BoolInputExtensions
{
    public static BoolInputBase ToBoolInput(this IAnyState state, string? label = null, bool disabled = false, BoolInputs variant = BoolInputs.Checkbox)
    {
        var type = state.GetStateType();
        var isNullable = type.IsNullableType();
        
        // Always create BoolInput<bool> or BoolInput<bool?> based on nullability
        var genericType = isNullable ? typeof(BoolInput<bool?>) : typeof(BoolInput<bool>);
        var input = (BoolInputBase)Activator.CreateInstance(genericType, state, label, disabled, variant)!;
        input.ScaffoldDefaults(null!, type);
        return input;
    }
    
    public static BoolInputBase ToSwitchInput(this IAnyState state, string? label = null, bool disabled = false) 
        => state.ToBoolInput(label, disabled, BoolInputs.Switch);

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
    
    public static BoolInputBase Label(this BoolInputBase widget, string label) => widget with { Label = label };
    public static BoolInputBase Disabled(this BoolInputBase widget, bool disabled = true) => widget with { Disabled = disabled };
    public static BoolInputBase Variant(this BoolInputBase widget, BoolInputs variant) => widget with { Variant = variant };
    public static BoolInputBase Icon(this BoolInputBase widget, Icons icon) => widget with { Icon = icon };
    public static BoolInputBase Description(this BoolInputBase widget, string description) => widget with { Description = description };
    public static BoolInputBase Invalid(this BoolInputBase widget, string? invalid) => widget with { Invalid = invalid };
}