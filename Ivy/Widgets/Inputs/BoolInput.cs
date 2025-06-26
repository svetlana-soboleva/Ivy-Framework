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
        var stateType = state.GetStateType();
        var isNullable = stateType.IsNullableType();
        
        // Create the appropriate BoolInput based on the original state type
        if (isNullable)
        {
            var boolValue = ConvertStateToNullableBool(state);
            var input = new BoolInput<bool?>(boolValue, e => SetStateValue(state, e.Value), label, disabled, variant);
            input.ScaffoldDefaults(null!, stateType);
            return input;
        }
        else
        {
            var boolValue = ConvertStateToBool(state);
            var input = new BoolInput<bool>(boolValue, e => SetStateValue(state, e.Value), label, disabled, variant);
            input.ScaffoldDefaults(null!, stateType);
            return input;
        }
    }
    
    private static bool ConvertStateToBool(IAnyState state)
    {
        var stateType = state.GetStateType();
        
        return stateType switch
        {
            // Boolean types - direct conversion
            _ when stateType == typeof(bool) => state.As<bool>().Value,
            _ when stateType == typeof(bool?) => state.As<bool?>().Value ?? false,
            
            // Numeric types - convert to boolean (0 = false, non-zero = true)
            _ when stateType.IsNumeric() => Convert.ToBoolean(state.As<object>().Value),
            
            // Other types - try BestGuessConvert, fallback to false
            _ => Core.Utils.BestGuessConvert(state.As<object>().Value, typeof(bool)) is bool b && b
        };
    }
    
    private static bool? ConvertStateToNullableBool(IAnyState state)
    {
        var stateType = state.GetStateType();
        
        return stateType switch
        {
            // Boolean types - direct conversion
            _ when stateType == typeof(bool) => state.As<bool>().Value,
            _ when stateType == typeof(bool?) => state.As<bool?>().Value,
            
            // Numeric types - convert to boolean (0 = false, non-zero = true)
            _ when stateType.IsNumeric() => ConvertNullableNumericToBool(state),
            
            // Other types - try BestGuessConvert, fallback to false
            _ => Core.Utils.BestGuessConvert(state.As<object>().Value, typeof(bool)) is bool b ? b : false
        };
    }
    
    private static bool? ConvertNullableNumericToBool(IAnyState state)
    {
        var stateType = state.GetStateType();
        var underlyingType = Nullable.GetUnderlyingType(stateType);
        
        if (underlyingType == null)
        {
            // Not nullable, convert directly
            return Convert.ToBoolean(state.As<object>().Value);
        }
        
        // Handle nullable numeric types
        var value = state.As<object>().Value;
        if (value == null)
        {
            return null;
        }
        
        return Convert.ToBoolean(value);
    }
    
    private static void SetStateValue(IAnyState state, bool boolValue)
    {
        var stateType = state.GetStateType();
        
        // Convert boolean back to the original state type
        var convertedValue = stateType switch
        {
            // Boolean types - direct conversion
            _ when stateType == typeof(bool) => (object)boolValue,
            _ when stateType == typeof(bool?) => (object)boolValue,
            
            // Numeric types - convert boolean to numeric (false = 0, true = 1)
            _ when stateType.IsNumeric() => Convert.ChangeType(boolValue ? 1 : 0, stateType),
            
            // Other types - use BestGuessConvert
            _ => Core.Utils.BestGuessConvert(boolValue, stateType)
        };
        
        // Set the state value
        state.As<object>().Set(convertedValue);
    }
    
    private static void SetStateValue(IAnyState state, bool? boolValue)
    {
        var stateType = state.GetStateType();
        
        // Convert boolean back to the original state type
        var convertedValue = stateType switch
        {
            // Boolean types - direct conversion
            _ when stateType == typeof(bool) => (object)(boolValue ?? false),
            _ when stateType == typeof(bool?) => (object)boolValue,
            
            // Numeric types - convert boolean to numeric (false = 0, true = 1, null = null)
            _ when stateType.IsNumeric() => ConvertBoolToNullableNumeric(boolValue, stateType),
            
            // Other types - use BestGuessConvert
            _ => Core.Utils.BestGuessConvert(boolValue ?? false, stateType)
        };
        
        // Set the state value
        state.As<object>().Set(convertedValue);
    }
    
    private static object ConvertBoolToNullableNumeric(bool? boolValue, Type targetType)
    {
        var underlyingType = Nullable.GetUnderlyingType(targetType);
        
        if (underlyingType == null)
        {
            // Not nullable, convert directly
            return Convert.ChangeType(boolValue == true ? 1 : 0, targetType);
        }
        
        // Handle nullable numeric types
        if (boolValue == null)
        {
            // Return null for nullable types
            return null!;
        }
        
        // Convert to the underlying type
        var numericValue = boolValue == true ? 1 : 0;
        return Convert.ChangeType(numericValue, underlyingType);
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