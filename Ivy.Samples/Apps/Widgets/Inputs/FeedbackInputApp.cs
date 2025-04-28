using Ivy.Shared;
using Microsoft.SemanticKernel.Text;

namespace Ivy.Samples.Apps.Widgets.Inputs;

[App(icon:Icons.ThumbsUp, path: [ "Widgets", "Inputs" ])]
public class FeedbackInputApp : SampleBase
{
    protected override object? BuildSample()
    {
        var zeroState = this.UseState(0);
        var twoState = this.UseState(2);

        var variants = Layout.Grid().Columns(5)
               | null!
               | Text.Block("Zero")
               | Text.Block("Two")
               | Text.Block("Disabled")
               | Text.Block("Invalid (NOT IMPLEMENTED)")

               | Text.InlineCode("FeedbackInputs.Stars")
               | zeroState.ToFeedbackInput().Variant(FeedbackInputs.Stars)
               | twoState.ToFeedbackInput().Variant(FeedbackInputs.Stars)
               | twoState.ToFeedbackInput().Variant(FeedbackInputs.Stars).Disabled()
               | twoState.ToFeedbackInput().Variant(FeedbackInputs.Stars).Invalid("Invalid feedback")
               
               | Text.InlineCode("FeedbackInputs.Emojis")
               | zeroState.ToFeedbackInput().Variant(FeedbackInputs.Emojis)
               | twoState.ToFeedbackInput().Variant(FeedbackInputs.Emojis)
               | twoState.ToFeedbackInput().Variant(FeedbackInputs.Emojis).Disabled()
               | twoState.ToFeedbackInput().Variant(FeedbackInputs.Emojis).Invalid("Invalid feedback")
               
               | Text.InlineCode("FeedbackInputs.Thumbs")
               | zeroState.ToFeedbackInput().Variant(FeedbackInputs.Thumbs)
               | twoState.ToFeedbackInput().Variant(FeedbackInputs.Thumbs)
               | twoState.ToFeedbackInput().Variant(FeedbackInputs.Thumbs).Disabled()
               | twoState.ToFeedbackInput().Variant(FeedbackInputs.Thumbs).Invalid("Invalid feedback")
            ;

        var intState = this.UseState(0);
        var nullableIntState = this.UseState<int?>((int?)null);
        var floatState = this.UseState(0.0f);
        var nullableFloatState = this.UseState<float?>((float?)null);
        var boolState = this.UseState(false);
        var nullableBoolState = this.UseState(false);
        
        var dataBinding = Layout.Grid().Columns(3)
                          | Text.InlineCode("int")
                          | intState.ToFeedbackInput()
                          | intState
                          
                          | Text.InlineCode("int?")
                          | nullableIntState.ToFeedbackInput()
                          | (nullableIntState.Value == null ? Text.InlineCode("null") : nullableIntState.Value.ToString())!
                          
                          | Text.InlineCode("float")
                          | floatState.ToFeedbackInput()
                          | floatState
                          
                          | Text.InlineCode("float?")
                          | nullableFloatState.ToFeedbackInput()
                          | (nullableFloatState.Value == null ? Text.InlineCode("null") : nullableFloatState.Value.ToString())!
                          
                          | Text.InlineCode("bool (NOT WORKING)")
                          | boolState.ToFeedbackInput()
                          | boolState
                          
                          | Text.InlineCode("bool? (NOT WORKING)")
                          | nullableBoolState.ToFeedbackInput()
                          | nullableBoolState
                          
            ;
        
        return Layout.Vertical()
               | Text.H1("Feedback Inputs")
               | Text.H2("Variants")
               | variants
               | Text.H2("Data Binding")
               | dataBinding

            ;

    }
}