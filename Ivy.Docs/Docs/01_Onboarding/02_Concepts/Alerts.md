# Alerts

Alerts in Ivy are powerful notification components that provide feedback to users about important information, warnings, or errors in your application. They are designed to be visually distinct and attention-grabbing while maintaining a clean and professional appearance.

## Overview

Alerts are typically used to:
- Display success messages after completing an action
- Show warnings about potential issues
- Present error messages when something goes wrong
- Provide important information to users

## Basic Usage

Here's a simple example of creating an alert:

```csharp
return new Alert(
    "Operation completed successfully!",
    variant: AlertVariant.Success
);
```

## Variants

Alerts come in several variants to match different use cases:

```csharp
Layout.Vertical(
    new Alert("Success message", variant: AlertVariant.Success),
    new Alert("Warning message", variant: AlertVariant.Warning),
    new Alert("Error message", variant: AlertVariant.Error),
    new Alert("Info message", variant: AlertVariant.Info)
)
```

## Icons

Alerts can include icons to enhance their visual meaning:

```csharp
new Alert(
    "File uploaded successfully",
    icon: Icons.CheckCircle,
    variant: AlertVariant.Success
)
```

## Dismissible Alerts

Alerts can be made dismissible, allowing users to close them:

```csharp
var isVisible = UseState(true);
return isVisible.Value
    ? new Alert(
        "This alert can be dismissed",
        onDismiss: () => isVisible.Set(false)
      )
    : null;
```

## Best Practices

1. **Clarity**: Keep alert messages clear and concise
2. **Appropriate Variants**: Use the correct variant for the type of message
3. **Timing**: Consider auto-dismissing alerts for non-critical messages
4. **Accessibility**: Ensure alerts are accessible to screen readers
5. **Consistency**: Maintain consistent alert styling across your application

## Examples

### Form Submission Feedback

```csharp
public class FormView : ViewBase
{
    public override object? Build()
    {
        var submitStatus = UseState<AlertVariant?>(null);
        var message = UseState("");

        return Layout.Vertical(
            submitStatus.Value != null
                ? new Alert(
                    message.Value,
                    variant: submitStatus.Value,
                    onDismiss: () => submitStatus.Set(null)
                  )
                : null,
            new Form(
                onSubmit: async () => {
                    try {
                        await api.Submit(formData);
                        message.Set("Form submitted successfully!");
                        submitStatus.Set(AlertVariant.Success);
                    } catch (Exception ex) {
                        message.Set("Failed to submit form: " + ex.Message);
                        submitStatus.Set(AlertVariant.Error);
                    }
                }
            )
        );
    }
}
```

### Toast Alerts

For temporary notifications, consider using the client's toast functionality:

```csharp
var client = this.UseService<IClientProvider>();
client.Toast("Operation completed!", variant: AlertVariant.Success);
```

