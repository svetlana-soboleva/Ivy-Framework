using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon: Icons.LassoSelect, path: ["Widgets", "Inputs"])]
public class SelectInputApp : SampleBase
{
    private static readonly Option<Guid>[] GuidOptions =
    [
        new("Niels", Guid.NewGuid()),
        new("Frida", Guid.NewGuid()),
        new("Julia", Guid.NewGuid()),
        new("Olivia", Guid.NewGuid())
    ];

    private static readonly IAnyOption[] IntOptions =
    [
        new Option<int>("Niels", 1),
        new Option<int>("Frida", 2),
        new Option<int>("Julia", 3),
        new Option<int>("Olivia", 4)
    ];

    private static readonly IAnyOption[] StringOptions =
    [
        new Option<string>("Niels"),
        new Option<string>("Frida"),
        new Option<string>("Julia"),
        new Option<string>("Olivia")
    ];

    private enum Colors
    {
        Red,
        Green,
        Blue,
        Yellow
    }

    protected override object? BuildSample()
    {
        var variants = CreateVariantsSection();
        var multiSelectVariants = CreateMultiSelectVariantsSection();
        var dataBinding = CreateDataBindingTests();
        var nullableTest = CreateNullableTestSection();

        return Layout.Vertical()
               | Text.H1("Select Inputs")
               | Text.H2("Nullable Test")
               | nullableTest
               | Text.H2("Variants")
               | variants
               | Text.H2("Multi-Select Variants")
               | multiSelectVariants
               | Text.H2("Data Binding")
               | dataBinding
               ;
    }

    private object CreateVariantsSection()
    {
        var colorState = UseState(Colors.Red);
        var nullableColorState = UseState((Colors?)null);
        var colorArrayState = UseState(new Colors[0]);
        var nullableColorArrayState = UseState<Colors[]?>(() => null);
        var colorOptions = typeof(Colors).ToOptions();

        return Layout.Grid().Columns(7)
               | Text.InlineCode("Variant")
               | Text.InlineCode("Default")
               | Text.InlineCode("Disabled")
               | Text.InlineCode("Invalid")
               | Text.InlineCode("With Placeholder")
               | Text.InlineCode("Nullable")
               | Text.InlineCode("Nullable Invalid")

               | Text.InlineCode("SelectInputs.Select")
               | colorState.ToSelectInput(colorOptions)
               | colorState
                    .ToSelectInput(colorOptions)
                    .Disabled()
               | colorState
                    .ToSelectInput(colorOptions)
                    .Invalid("Invalid")
               | colorState
                    .ToSelectInput(colorOptions)
                    .Placeholder("Select a color")
               | nullableColorState
                    .ToSelectInput(colorOptions)
               | nullableColorState
                    .ToSelectInput(colorOptions)
                    .Invalid("Invalid")

               | Text.InlineCode("SelectInputs.List")
               | colorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
               | colorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
                    .Disabled()
               | colorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
                    .Invalid("Invalid")
               | colorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
                    .Placeholder("Select colors")
               | nullableColorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
               | nullableColorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
                    .Invalid("Invalid")

               | Text.InlineCode("SelectInputs.Toggle")
               | colorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)
               | colorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)
                    .Disabled()
               | colorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)
                    .Invalid("Invalid")
               | colorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)
                    .Placeholder("Select a color")
               | nullableColorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)
               | nullableColorArrayState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)
                    .Invalid("Invalid");
    }

    private object CreateMultiSelectVariantsSection()
    {
        var colorStateSelectDefault = UseState<Colors[]>([]);
        var colorStateSelectDisabled = UseState<Colors[]>([]);
        var colorStateSelectInvalid = UseState<Colors[]>([]);
        var colorStateSelectPlaceholder = UseState<Colors[]>([]);
        var colorStateListDefault = UseState<Colors[]>([]);
        var colorStateListDisabled = UseState<Colors[]>([]);
        var colorStateListInvalid = UseState<Colors[]>([]);
        var colorStateListPlaceholder = UseState<Colors[]>([]);
        var colorStateToggleDefault = UseState<Colors[]>([]);
        var colorStateToggleDisabled = UseState<Colors[]>([]);
        var colorStateToggleInvalid = UseState<Colors[]>([]);
        var colorStateTogglePlaceholder = UseState<Colors[]>([]);
        var colorOptions = typeof(Colors).ToOptions();

        return Layout.Grid().Columns(6)
               | Text.InlineCode("Variant")
               | Text.InlineCode("Default")
               | Text.InlineCode("Disabled")
               | Text.InlineCode("Invalid")
               | Text.InlineCode("With Placeholder")
               | Text.InlineCode("State")

               | Text.InlineCode("SelectInputs.Select")
               | colorStateSelectDefault
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Select)
               | colorStateSelectDisabled
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Select)
                    .Disabled()
               | colorStateSelectInvalid
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Select)
                    .Invalid("Invalid")
               | colorStateSelectPlaceholder
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Select)
                    .Placeholder("Select colors")
               | Text.InlineCode($"[{string.Join(", ", colorStateSelectDefault.Value)}]")

               | Text.InlineCode("SelectInputs.List")
               | colorStateListDefault
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
               | colorStateListDisabled
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
                    .Disabled()
               | colorStateListInvalid
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
                    .Invalid("Invalid")
               | colorStateListPlaceholder
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
                    .Placeholder("Select colors")
               | Text.InlineCode($"[{string.Join(", ", colorStateListDefault.Value)}]")

               | Text.InlineCode("SelectInputs.Toggle")
               | colorStateToggleDefault
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)
               | colorStateToggleDisabled
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)
                    .Disabled()
               | colorStateToggleInvalid
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)
                    .Invalid("Invalid")
               | colorStateTogglePlaceholder
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)
                    .Placeholder("Select colors")
               | Text.InlineCode($"[{string.Join(", ", colorStateToggleDefault.Value)}]");
    }

    private object CreateDataBindingTests()
    {
        var dataTypes = new (string TypeName, object NonNullableState, object NullableState, IAnyOption[] Options)[]
        {
            // Enum types
            ("Colors", UseState(Colors.Red), UseState((Colors?)null), typeof(Colors).ToOptions()),
            // String types
            ("string", UseState("Niels"), UseState((string?)null), StringOptions),
            // Integer types
            ("int", UseState(1), UseState((int?)null), IntOptions),
            // Guid types
            ("Guid", UseState(GuidOptions[0].TypedValue), UseState((Guid?)null), GuidOptions),
            // Array types
            ("Colors[]", UseState<Colors[]>([]), UseState((Colors[]?)null), typeof(Colors).ToOptions()),
            ("string[]", UseState<string[]>([]), UseState((string[]?)null), StringOptions),
            ("int[]", UseState<int[]>([]), UseState((int[]?)null), IntOptions),
            ("Guid[]", UseState<Guid[]>([]), UseState((Guid[]?)null), GuidOptions),
            // List types
            ("List<Colors>", UseState<List<Colors>>([]), UseState((List<Colors>?)null), typeof(Colors).ToOptions()),
            ("List<string>", UseState<List<string>>([]), UseState((List<string>?)null), StringOptions),
            ("List<int>", UseState<List<int>>([]), UseState((List<int>?)null), IntOptions),
            ("List<Guid>", UseState<List<Guid>>([]), UseState((List<Guid>?)null), GuidOptions)
        };

        var gridRows = new List<object>
        {
            Text.InlineCode("Type"),
            Text.InlineCode("Non-Nullable"),
            Text.InlineCode("State"),
            Text.InlineCode("Type"),
            Text.InlineCode("Nullable"),
            Text.InlineCode("State")
        };

        foreach (var (typeName, nonNullableState, nullableState, options) in dataTypes)
        {
            // Non-nullable
            var nonNullableAnyState = nonNullableState as IAnyState;
            object? nonNullableValue = null;
            if (nonNullableAnyState != null)
            {
                var prop = nonNullableAnyState.GetType().GetProperty("Value");
                nonNullableValue = prop?.GetValue(nonNullableAnyState);
            }
            var nonNullableCell = Layout.Vertical()
                | CreateSelectInputVariants(nonNullableState, options);

            // Nullable
            object? nullableValue = null;
            if (nullableState is IAnyState anyState)
            {
                var prop = anyState.GetType().GetProperty("Value");
                nullableValue = prop?.GetValue(anyState);
            }
            var nullableCell = Layout.Vertical()
                | CreateSelectInputVariants(nullableState, options);

            gridRows.Add(Text.InlineCode(typeName));
            gridRows.Add(nonNullableCell);
            gridRows.Add(FormatStateValue(typeName, nonNullableValue, false));
            gridRows.Add(Text.InlineCode($"{typeName}?"));
            gridRows.Add(nullableCell);
            gridRows.Add(FormatStateValue(typeName, nullableValue, true));
        }

        return Layout.Grid().Columns(6) | gridRows.ToArray();
    }

    private static object CreateSelectInputVariants(object state, IAnyOption[] options)
    {
        if (state is not IAnyState anyState)
            return Text.Block("Not an IAnyState");

        var stateType = anyState.GetStateType();
        var isCollection = stateType.IsCollectionType();

        if (isCollection)
        {
            return Layout.Vertical()
                   | anyState.ToSelectInput(options)
                   | anyState.ToSelectInput(options).Variant(SelectInputs.List)
                   | anyState.ToSelectInput(options).Variant(SelectInputs.Toggle);
        }

        // For non-collection states, show all three variants
        return Layout.Vertical()
               | anyState.ToSelectInput(options)
               | anyState.ToSelectInput(options).Variant(SelectInputs.List)
               | anyState.ToSelectInput(options).Variant(SelectInputs.Toggle);
    }

    private object CreateNullableTestSection()
    {
        var nullableColorState = UseState((Colors?)null);
        var nonNullableColorState = UseState(Colors.Red);
        var colorOptions = typeof(Colors).ToOptions();

        return Layout.Grid().Columns(4)
               | Text.InlineCode("Type")
               | Text.InlineCode("Select")
               | Text.InlineCode("List")
               | Text.InlineCode("Toggle")

               | Text.InlineCode("Nullable")
               | nullableColorState.ToSelectInput(colorOptions)
               | nullableColorState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
               | nullableColorState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle)

               | Text.InlineCode("Non-Nullable")
               | nonNullableColorState.ToSelectInput(colorOptions)
               | nonNullableColorState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.List)
               | nonNullableColorState
                    .ToSelectInput(colorOptions)
                    .Variant(SelectInputs.Toggle);
    }

    private static object FormatStateValue(string typeName, object? value, bool isNullable)
    {
        return value switch
        {
            null => isNullable ? Text.InlineCode("Null") : Text.InlineCode("Default"),
            Array array => array.Length == 0 ? Text.InlineCode("[]") : Text.InlineCode($"[{array.Length} items]"),
            System.Collections.IList list => list.Count == 0
                ? Text.InlineCode("[]")
                : Text.InlineCode($"[{list.Count} items]"),
            _ => Text.InlineCode(value.ToString()!)
        };
    }
}