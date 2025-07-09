using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon: Icons.PaintBucket, path: ["Widgets", "Inputs"])]
public class ColorInputApp : SampleBase
{
    protected override object? BuildSample()
    {
        var variants = CreateVariantsSection();
        var stateTracking = CreateStateTrackingSection();
        var colorFormats = CreateColorFormatsSection();
        var validationStates = CreateValidationStatesSection();

        return Layout.Vertical()
               | Text.H1("Color Input")
               | Text.H2("Variants")
               | variants
               | Text.H2("State Tracking")
               | stateTracking
               | Text.H2("Color Formats")
               | colorFormats
               | Text.H2("Validation States")
               | validationStates;
    }

    private object CreateVariantsSection()
    {
        var hexState = UseState("#ff0000");
        var enumState = UseState(Colors.Blue);
        var rgbState = UseState("rgb(255, 0, 0)");
        var oklchState = UseState("oklch(0.5, 0.2, 240)");

        return Layout.Grid().Columns(2)
               | (Layout.Vertical().Padding(10) | Text.H3("Hex Color") | hexState.ToColorInput() | hexState)
               | (Layout.Vertical().Padding(10) | Text.H3("Colors Enum") | enumState.ToColorInput() | enumState)
               | (Layout.Vertical().Padding(10) | Text.H3("RGB Color") | rgbState.ToColorInput() | rgbState)
               | (Layout.Vertical().Padding(10) | Text.H3("OKLCH Color") | oklchState.ToColorInput() | oklchState);
    }

    private object CreateStateTrackingSection()
    {
        var stringState = UseState("#00ff00");
        var colorsState = UseState(Colors.Green);
        var nullableStringState = UseState((string?)null);
        var nullableColorsState = UseState((Colors?)null);

        return Layout.Grid().Columns(2)
               | (Layout.Vertical().Padding(10) | Text.H3("String State") | stringState.ToColorInput() | stringState)
               | (Layout.Vertical().Padding(10) | Text.H3("Colors State") | colorsState.ToColorInput() | colorsState)
               | (Layout.Vertical().Padding(10) | Text.H3("Nullable String") | nullableStringState.ToColorInput() | nullableStringState)
               | (Layout.Vertical().Padding(10) | Text.H3("Nullable Colors") | nullableColorsState.ToColorInput() | nullableColorsState);
    }

    private object CreateColorFormatsSection()
    {
        var hexInput = UseState("#ff6b35");
        var rgbInput = UseState("rgb(255, 107, 53)");
        var oklchInput = UseState("oklch(0.7, 0.15, 25)");
        var enumInput = UseState(Colors.Orange);

        return Layout.Vertical()
               | Text.H3("Different Color Formats")
               | Layout.Grid().Columns(2)
                 | (Layout.Vertical().Padding(10) | Text.H4("Hex Format") | hexInput.ToColorInput().Placeholder("Enter hex color (e.g., #ff6b35)") | hexInput)
                 | (Layout.Vertical().Padding(10) | Text.H4("RGB Format") | rgbInput.ToColorInput().Placeholder("Enter rgb color (e.g., rgb(255, 107, 53))") | rgbInput)
                 | (Layout.Vertical().Padding(10) | Text.H4("OKLCH Format") | oklchInput.ToColorInput().Placeholder("Enter oklch color (e.g., oklch(0.7, 0.15, 25))") | oklchInput)
                 | (Layout.Vertical().Padding(10) | Text.H4("Colors Enum") | enumInput.ToColorInput().Placeholder("Enter color name (e.g., Orange)") | enumInput);
    }

    private object CreateValidationStatesSection()
    {
        var validState = UseState("#4CAF50");
        var invalidState = UseState("invalid-color");
        var disabledState = UseState("#2196F3");

        return Layout.Vertical()
               | Text.H3("Validation and States")
               | Layout.Grid().Columns(3)
                 | (Layout.Vertical().Padding(10) | Text.H4("Valid Color") | validState.ToColorInput() | validState)
                 | (Layout.Vertical().Padding(10) | Text.H4("Invalid Color") | invalidState.ToColorInput().Invalid("Please enter a valid color format") | invalidState)
                 | (Layout.Vertical().Padding(10) | Text.H4("Disabled") | disabledState.ToColorInput().Disabled(true) | disabledState);
    }
}