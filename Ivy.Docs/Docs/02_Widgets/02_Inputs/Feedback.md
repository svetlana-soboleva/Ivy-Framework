---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# FeedbackInput

The FeedbackInput widget provides a specialized input for collecting user feedback. It typically includes rating options and a text field for comments, making it ideal for surveys, reviews, and feedback forms.

## Basic Usage

Here's a simple example of a FeedbackInput with a default variant:

```csharp
twoState.ToFeedbackInput().Variant(FeedbackInputs.Stars)
```

```csharp
twoState.ToFeedbackInput().Variant(FeedbackInputs.Stars)
```

## Variants

FeedbackInputs come in several variants to suit different use cases:

```csharp
Layout.Horizontal()
    | new FeedbackInput<int>(0).Variant(FeedbackInputs.Stars)
    | new FeedbackInput<int>(0).Variant(FeedbackInputs.Thumbs)
    | new FeedbackInput<int>(0).Variant(FeedbackInputs.Emojis)
```

## Event Handling

FeedbackInputs can handle change events using the `OnChange` parameter:

```csharp
var feedbackState = this.UseState(0);
var eventHandler = (Event<FeedbackInput<int>, int> e) =>
{
    feedbackState.Set(e.Value);
};
return new FeedbackInput<int>(feedbackState.Value, eventHandler);
```

## Styling and Customization

FeedbackInputs can be customized with various styling options:

```csharp
new FeedbackInput<int>(0)
    .Variant(FeedbackInputs.Stars)
    .Placeholder("Rate your experience")
    .Disabled(false)
```

<WidgetDocs Type="Ivy.FeedbackInput" ExtensionsType="Ivy.FeedbackInputExtensions"/>