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

        return Layout.Vertical()
               | Text.H1("Select Inputs")
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
        var colorOptions = typeof(Colors).ToOptions();

        return Layout.Grid().Columns(5)
               | null!
               | Text.Block("Default")
               | Text.Block("Disabled")
               | Text.Block("Invalid")
               | Text.Block("With Placeholder")

               | Text.InlineCode("SelectInputs.Select")
               | colorState.ToSelectInput(colorOptions)
               | colorState.ToSelectInput(colorOptions).Disabled()
               | colorState.ToSelectInput(colorOptions).Invalid("Invalid")
               | colorState.ToSelectInput(colorOptions).Placeholder("Select a color")

               | null!
               | colorState.ToSelectInput(colorOptions)
               | colorState.ToSelectInput(colorOptions).Disabled()
               | colorState.ToSelectInput(colorOptions).Invalid("Invalid")
               | colorState.ToSelectInput(colorOptions).Placeholder("Select a color")

               | Text.InlineCode("SelectInputs.List")
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.List)
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.List).Disabled()
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.List).Invalid("Invalid")
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.List).Placeholder("Select a color")

               | null!
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.List)
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.List).Disabled()
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.List).Invalid("Invalid")
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.List).Placeholder("Select a color")

               | Text.InlineCode("SelectInputs.Toggle")
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle)
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle).Disabled()
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle).Invalid("Invalid")
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle).Placeholder("Select a color")

               | null!
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle)
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle).Disabled()
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle).Invalid("Invalid")
               | colorState.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle).Placeholder("Select a color");
    }

    private object CreateMultiSelectVariantsSection()
    {
        var colorStateList = UseState(new List<Colors>());
        var colorStateToggle = UseState(new List<Colors>());
        var colorOptions = typeof(Colors).ToOptions();

        return Layout.Grid().Columns(6)
               | null!
               | Text.Block("Default")
               | Text.Block("Disabled")
               | Text.Block("Invalid")
               | Text.Block("With Placeholder")
               | Text.Block("State")

               | Text.InlineCode("SelectInputs.List")
               | colorStateList.ToSelectInput(colorOptions).Variant(SelectInputs.List).SelectMany()
               | colorStateList.ToSelectInput(colorOptions).Variant(SelectInputs.List).SelectMany().Disabled()
               | colorStateList.ToSelectInput(colorOptions).Variant(SelectInputs.List).SelectMany().Invalid("Invalid")
               | colorStateList.ToSelectInput(colorOptions).Variant(SelectInputs.List).SelectMany().Placeholder("Select colors")
               | Text.InlineCode($"[{string.Join(", ", colorStateList.Value)}]")

               | Text.InlineCode("SelectInputs.Toggle")
               | colorStateToggle.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle).SelectMany()
               | colorStateToggle.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle).SelectMany().Disabled()
               | colorStateToggle.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle).SelectMany().Invalid("Invalid")
               | colorStateToggle.ToSelectInput(colorOptions).Variant(SelectInputs.Toggle).SelectMany().Placeholder("Select colors")
               | Text.InlineCode($"[{string.Join(", ", colorStateToggle.Value)}]");
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

        var gridItems = new List<object>
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
            // Non-nullable columns (first 3)
            gridItems.Add(Text.InlineCode(typeName));
            gridItems.Add(CreateSelectInputVariants(nonNullableState, options));

            var nonNullableAnyState = nonNullableState as IAnyState;
            object? nonNullableValue = null;
            if (nonNullableAnyState != null)
            {
                var prop = nonNullableAnyState.GetType().GetProperty("Value");
                nonNullableValue = prop?.GetValue(nonNullableAnyState);
            }
            gridItems.Add(FormatStateValue(typeName, nonNullableValue, false));

            // Nullable columns (next 3)
            gridItems.Add(Text.InlineCode($"{typeName}?"));
            gridItems.Add(CreateSelectInputVariants(nullableState, options));

            object? value = null;
            if (nullableState is IAnyState anyState)
            {
                var prop = anyState.GetType().GetProperty("Value");
                value = prop?.GetValue(anyState);
            }
            gridItems.Add(FormatStateValue(typeName, value, true));
        }

        return Layout.Grid().Columns(6) | gridItems.ToArray();
    }

    private static object CreateSelectInputVariants(object state, IAnyOption[] options)
    {
        if (state is not IAnyState anyState)
            return Text.Block("Not an IAnyState");

        var stateType = anyState.GetStateType();
        var isCollection = stateType.IsCollectionType();

        if (isCollection)
        {
            // For collection states, show all three variants with SelectMany
            return Layout.Vertical()
                   | anyState.ToSelectInput(options).SelectMany()
                   | anyState.ToSelectInput(options).Variant(SelectInputs.List).SelectMany()
                   | anyState.ToSelectInput(options).Variant(SelectInputs.Toggle).SelectMany();
        }

        // For non-collection states, show all three variants
        return Layout.Vertical()
               | anyState.ToSelectInput(options)
               | anyState.ToSelectInput(options).Variant(SelectInputs.List)
               | anyState.ToSelectInput(options).Variant(SelectInputs.Toggle);
    }

    private static object FormatStateValue(string typeName, object? value, bool isNullable)
    {
        if (value == null)
            return isNullable ? Text.InlineCode("Null") : Text.InlineCode("Default");

        if (value is Array array)
        {
            return array.Length == 0 ? Text.InlineCode("[]") : Text.InlineCode($"[{array.Length} items]");
        }

        if (value is System.Collections.IList list)
        {
            return list.Count == 0 ? Text.InlineCode("[]") : Text.InlineCode($"[{list.Count} items]");
        }

        return Text.InlineCode(value.ToString()!);
    }
}