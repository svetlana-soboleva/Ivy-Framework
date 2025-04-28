using Ivy.Shared;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon:Icons.Check, path: ["Widgets", "Inputs"])]
public class BoolInputApp : SampleBase
{
    protected override object? BuildSample()
    {
        var falseState = this.UseState(false);
        var trueState = this.UseState(true);
        var nullState = this.UseState((bool?)null);
        var intState = this.UseState(0);
        
        return intState.ToBoolInput();
        
        return Layout.Grid().Columns(6)
               | null!
               | Text.Block("True")
               | Text.Block("False")
               | Text.Block("Disabled")
               | Text.Block("Invalid")
               | Text.Block("Null")

               | Text.InlineCode("BoolInputs.Checkbox")
               | trueState.ToBoolInput().Label("Label").Description("Description")
               | falseState.ToBoolInput().Label("Label").Description("Description")
               | trueState.ToBoolInput().Label("Label").Description("Description").Disabled()
               | new Box("Not Implemented")
               | nullState.ToBoolInput().Label("Label").Description("Description")
               
               | null!
               | trueState.ToBoolInput().Label("Label")
               | falseState.ToBoolInput().Label("Label")
               | trueState.ToBoolInput().Label("Label").Disabled()
               | new Box("Not Implemented")
               | nullState.ToBoolInput().Label("Label")
               
               | Text.InlineCode("BoolInputs.Switch")
               | trueState.ToSwitchInput().Label("Label").Description("Description")
               | falseState.ToSwitchInput().Label("Label").Description("Description")
               | trueState.ToSwitchInput().Label("Label").Description("Description").Disabled()
               | new Box("Not Implemented")
               | new Box("Not Implemented")
               
               | null!
               | trueState.ToSwitchInput().Label("Label")
               | falseState.ToSwitchInput().Label("Label")
               | trueState.ToSwitchInput().Label("Label").Disabled()
               | new Box("Not Implemented")
               | new Box("Not Implemented")
               
               | Text.InlineCode("BoolInputs.Toggle")
               | trueState.ToToggleInput().Label("Label").Description("Description")
               | falseState.ToToggleInput().Label("Label").Description("Description")
               | trueState.ToToggleInput().Label("Label").Description("Description").Disabled()
               | new Box("Not Implemented")
               | new Box("Not Implemented")
               
               | null!
               | trueState.ToToggleInput(Icons.Baby).Label("Label")
               | falseState.ToToggleInput(Icons.Baby).Label("Label")
               | trueState.ToToggleInput(Icons.Baby).Label("Label").Disabled()
               | new Box("Not Implemented")
               | new Box("Not Implemented")
               
            ; 
    }
}