using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon: Icons.PaintBucket, path: ["Widgets", "Inputs"])]
public class ColorInputApp : SampleBase
{
    protected override object? BuildSample()
    {
        var variants = CreateVariantsSection();
        var dataBinding = CreateDataBindingTests();
        var formatTests = CreateFormatTests();

        return Layout.Vertical()
               | Text.H1("ColorInput")
               | Text.H2("Variants")
               | variants
               | Text.H2("Format Tests")
               | formatTests
               | Text.H2("Data Binding")
               | dataBinding
            ;
    }

    private object CreateVariantsSection()
    {
        var hexState = UseState("#ff0000");
        var rgbState = UseState("rgb(255, 0, 0)");
        var oklchState = UseState("oklch(0.5, 0.2, 240)");
        var nullState = UseState((string?)null);

        return Layout.Grid().Columns(6)
               | null!
               | Text.Block("Default")
               | Text.Block("Disabled")
               | Text.Block("Invalid")
               | Text.Block("With Placeholder")
               | Text.Block("Nullable")

               | Text.InlineCode("Hex")
               | hexState.ToColorInput()
               | hexState.ToColorInput().Disabled()
               | hexState.ToColorInput().Invalid("Invalid color")
               | hexState.ToColorInput().Placeholder("Select color")
               | nullState.ToColorInput()

               | Text.InlineCode("RGB")
               | rgbState.ToColorInput()
               | rgbState.ToColorInput().Disabled()
               | rgbState.ToColorInput().Invalid("Invalid color")
               | rgbState.ToColorInput().Placeholder("Select color")
               | null!

               | Text.InlineCode("OKLCH")
               | oklchState.ToColorInput()
               | oklchState.ToColorInput().Disabled()
               | oklchState.ToColorInput().Invalid("Invalid color")
               | oklchState.ToColorInput().Placeholder("Select color")
               | null!
            ;
    }

    private object CreateFormatTests()
    {
        var hexState = UseState("#ff0000");
        var rgbState = UseState("rgb(255, 0, 0)");
        var oklchState = UseState("oklch(0.5, 0.2, 240)");
        var enumState = UseState(Colors.Red);

        return Layout.Grid().Columns(4)
               | Text.InlineCode("Format")
               | Text.InlineCode("Input")
               | Text.InlineCode("Display Value")
               | Text.InlineCode("Stored Value")

               | Text.InlineCode("Hex")
               | hexState.ToColorInput()
               | Text.InlineCode(hexState.Value)
               | Text.InlineCode(hexState.Value)

               | Text.InlineCode("RGB")
               | rgbState.ToColorInput()
               | Text.InlineCode(rgbState.Value)
               | Text.InlineCode(ConvertToHex(rgbState.Value))

               | Text.InlineCode("OKLCH")
               | oklchState.ToColorInput()
               | Text.InlineCode(oklchState.Value)
               | Text.InlineCode(ConvertToHex(oklchState.Value))

               | Text.InlineCode("Enum")
               | enumState.ToColorInput()
               | Text.InlineCode(enumState.Value.ToString())
               | Text.InlineCode(ConvertToHex(enumState.Value.ToString()))
            ;
    }

    private object CreateDataBindingTests()
    {
        var colorTypes = new (string TypeName, object NonNullableState, object NullableState)[]
        {
            ("string", UseState("#ff0000"), UseState((string?)null)),
            ("Colors", UseState(Colors.Red), UseState((Colors?)null))
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

        foreach (var (typeName, nonNullableState, nullableState) in colorTypes)
        {
            // Non-nullable columns (first 3)
            gridItems.Add(Text.InlineCode(typeName));
            gridItems.Add(CreateColorInputVariants(nonNullableState));

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
            gridItems.Add(CreateColorInputVariants(nullableState));

            var anyState = nullableState as IAnyState;
            object? value = null;
            if (anyState != null)
            {
                var prop = anyState.GetType().GetProperty("Value");
                value = prop?.GetValue(anyState);
            }

            gridItems.Add(FormatStateValue(typeName, value, true));
        }

        return Layout.Grid().Columns(6) | gridItems.ToArray();
    }

    private static object CreateColorInputVariants(object state)
    {
        if (state is not IAnyState anyState)
            return Text.Block("Not an IAnyState");

        var stateType = anyState.GetStateType();
        var isNullable = stateType.IsNullableType();

        if (isNullable)
        {
            // For nullable states, show basic variant
            return anyState.ToColorInput();
        }

        // For non-nullable states, show all variants
        return Layout.Vertical()
               | anyState.ToColorInput()
               | anyState.ToColorInput().Placeholder("Select color")
               | anyState.ToColorInput().Disabled();
    }

    private static object FormatStateValue(string typeName, object? value, bool isNullable)
    {
        return value switch
        {
            null => isNullable ? Text.InlineCode("Null") : Text.InlineCode("Default"),
            string s => Text.InlineCode(s),
            Colors c => Text.InlineCode(c.ToString()),
            _ => Text.InlineCode(value?.ToString() ?? "null")
        };
    }

    private static string ConvertToHex(string? colorValue)
    {
        if (string.IsNullOrEmpty(colorValue))
            return "null";

        // Simple conversion for demo purposes
        // In a real implementation, you'd want proper color parsing
        return colorValue.StartsWith("#") ? colorValue : $"#{colorValue.GetHashCode():X6}";
    }
}