---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# FeedbackInput

The FeedbackInput widget provides a specialized input for collecting user feedback. It typically includes rating options and a text field for comments, making it ideal for surveys, reviews, and feedback forms.

## Basic Usage

Here's a simple example of a FeedbackInput with a default variant:

```csharp demo-below
public class BasicFeedbackDemo : ViewBase
{    
    public override object? Build()
    {    
        var starStates = Enumerable.Range(1,5).Select(t => UseState(t)).ToList();
        var layout  = Layout.Vertical();
        
        starStates.ForEach( state => 
             layout = layout | state.ToFeedbackInput().Variant(FeedbackInputs.Stars));
        
       return layout;
    }
}    
```

## Variants

FeedbackInputs come in several variants to suit different use cases:

```csharp demo-below
public class FeedbackDemo : ViewBase
{
    public override object? Build()
    {    
        // Initial guess feedbacks 
        var starFeedback = UseState(3);
        var thumbsFeedback = UseState(true);
        var emojiFeedback = UseState(4);
        return Layout.Vertical()
                | H3 ("Simple movie review")
                | Text.Block("Did you like the movie ?")
                | new FeedbackInput<bool>(thumbsFeedback).Variant(FeedbackInputs.Thumbs)
                | Text.Block("How would you like to rate the movie ?")
                | new FeedbackInput<int>(starFeedback).Variant(FeedbackInputs.Stars)
                | Text.Block("How do you feel after seeing the movie ?")
                | new FeedbackInput<int>(emojiFeedback).Variant(FeedbackInputs.Emojis);
    }  
}    
```

## Event Handling

The following example shows how change events can be handled for `FeedbackInput`s. 

```csharp demo-below
public class FeedbackHandling: ViewBase
{    
    public override object? Build()
    {    
        var feedbackState = UseState(1);
        var exclamation = UseState("");
        switch(feedbackState.Value)
        {
            case 1: exclamation.Set("Seriously?");
                    break;
            case 2: exclamation.Set("Oh! is it that bad?");
                    break;
            case 3: exclamation.Set("Ah! you almost liked it!");
                    break;
            case 4: exclamation.Set("Cool! Tell me more!");                    
                    break;
            case 5: exclamation.Set("WOW! Would you recommend it?");
                    break;
        }
        return Layout.Vertical() 
                | new FeedbackInput<int>(feedbackState)
                | Text.Block(exclamation);
    }    
}
```

## Styling and Customization

`FeedbackInput`s can be customized with various styling options:

### Disabled 

To render a feedback input in disabled state, this function `Disabled` should be used. 

```csharp demo-below
public class DisabledFeedbackDemo : ViewBase
{
    public override object? Build()
    {    
        var fdb = UseState(3);
        return Layout.Vertical()
            | new FeedbackInput<int>(fdb)
                    .Variant(FeedbackInputs.Stars)
                    .Disabled();
    }
}        
```

### Invalid

To render a feedback input in invalid (or error) state, the function `Invalid` should be used.

```csharp demo-below
public class InvalidFeedbackDemo : ViewBase
{
    public override object? Build()
    {    
        var fdb = UseState(3);
        return Layout.Vertical()
            | new FeedbackInput<int>(fdb)
                    .Variant(FeedbackInputs.Stars)
                    .Invalid("We are maintaining this.");
    }
}        
```

<WidgetDocs Type="Ivy.FeedbackInput" ExtensionTypes="Ivy.FeedbackInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/FeedbackInput.cs"/>