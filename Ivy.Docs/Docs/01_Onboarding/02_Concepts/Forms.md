# Forms

Forms in Ivy provide a powerful way to handle user input and data collection. They combine state management with validation and submission handling to create a seamless user experience.

## Overview

Forms in Ivy are built using the `Form` widget and various input widgets. They work together with state management to handle form data and validation.

## Basic Form Structure

```csharp
public class LoginForm : ViewBase
{
    public override object? Build()
    {
        var formData = UseState(new LoginData());
        
        return new Form(
            onSubmit: async () => {
                // Handle form submission
                await api.Login(formData.Value);
            }
        )
        | new TextInput("Username", value: formData.Value.Username, onChange: v => formData.Set(v))
        | new PasswordInput("Password", value: formData.Value.Password, onChange: v => formData.Set(v))
        | new Button("Login");
    }
}
```

## Input Types

Ivy provides various input widgets for different data types:

### Text Input
```csharp
new TextInput("Name", value: name, onChange: v => name.Set(v))
```

### Number Input
```csharp
new NumberInput("Age", value: age, onChange: v => age.Set(v))
    .Min(0)
    .Max(120)
    .Step(1);
```

### Checkbox
```csharp
new Checkbox("Accept terms", value: terms, onChange: v => terms.Set(v))
```

### Select
```csharp
new Select("Country", options: countries, value: country, onChange: v => country.Set(v))
```

## Form Validation

Forms support built-in validation:

```csharp
public class RegistrationForm : ViewBase
{
    public override object? Build()
    {
        var formData = UseState(new RegistrationData());
        var errors = UseState(new Dictionary<string, string>());
        
        return new Form(
            onSubmit: async () => {
                if (!ValidateForm(formData.Value, out var validationErrors))
                {
                    errors.Set(validationErrors);
                    return;
                }
                await api.Register(formData.Value);
            }
        )
        | new TextInput("Email", value: formData.Value.Email, onChange: v => formData.Set(v))
            .Required()
            .Email()
        | new PasswordInput("Password", value: formData.Value.Password, onChange: v => formData.Set(v))
            .Required()
            .MinLength(8)
        | errors
        | new Button("Register");
    }
}
```

## Form State Management

Forms often need to handle multiple pieces of state:

```csharp
public class ComplexForm : ViewBase
{
    public override object? Build()
    {
        var formData = UseState(new ComplexData());
        var isSubmitting = UseState(false);
        var submitError = UseState<string?>(null);
        
        return new Form(
            onSubmit: async () => {
                isSubmitting.Set(true);
                try {
                    await api.Submit(formData.Value);
                    client.Toast("Success!");
                } catch (Exception ex) {
                    submitError.Set(ex.Message);
                } finally {
                    isSubmitting.Set(false);
                }
            }
        )
        | new TextInput("Name", value: formData.Value.Name, onChange: v => formData.Set(v))
        | submitError
        | new Button("Submit", disabled: isSubmitting.Value);
    }
}
```

## Best Practices

1. **State Management**: Use `UseState` for form data and validation state.
2. **Validation**: Implement validation before form submission.
3. **Error Handling**: Show clear error messages to users.
4. **Loading States**: Disable form during submission.
5. **Form Reset**: Provide a way to reset the form when needed.

## Examples

### Multi-step Form

```csharp
public class MultiStepForm : ViewBase
{
    public override object? Build()
    {
        var step = UseState(1);
        var formData = UseState(new MultiStepData());
        
        return new Form(
            onSubmit: async () => {
                if (step.Value < 3)
                {
                    step.Set(step.Value + 1);
                    return;
                }
                await api.Submit(formData.Value);
            }
        )
        | (step.Value == 1 ? StepOne(formData) : null)
        | (step.Value == 2 ? StepTwo(formData) : null)
        | (step.Value == 3 ? StepThree(formData) : null)
        | new Button(step.Value == 3 ? "Submit" : "Next");
    }
}
```

## See Also

- [State Management](./State.md)
- [Clients](./Clients.md)