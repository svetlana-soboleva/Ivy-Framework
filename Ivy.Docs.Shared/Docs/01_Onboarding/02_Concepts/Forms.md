---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# Forms

<Ingress>
Build robust forms with built-in state management, validation, and submission handling for collecting and processing user input.
</Ingress>

**Recommended Approach**: Forms in Ivy should always be built using the `.ToForm()` extension method on state objects. This approach provides automatic scaffolding, state management, validation, and submission handling through the FormBuilder pattern.

<Callout Type="important">
Do not manually create form layouts. Always use `.ToForm()` on your state objects for type safety, automatic state management, and built-in validation.
</Callout>

## Core Concepts

### The FormBuilder Pattern

Ivy forms use a builder pattern that starts with a model (typically a C# record or class) and automatically scaffolds appropriate input fields based on the property types. This approach provides several benefits:

- **Type Safety**: Your forms are strongly typed and match your data models
- **Automatic Scaffolding**: Input types are automatically determined from property types
- **Validation Integration**: Built-in validation based on data annotations and custom validators
- **State Management**: Automatic state synchronization between form and model

```csharp
// Define your model
public record UserModel(string Name, string Email, bool IsActive);

// Create a form from the model
var user = UseState(() => new UserModel("", "", true));
var form = user.ToForm(); // Automatically creates appropriate inputs
```

### Automatic Field Generation

The FormBuilder automatically maps C# types to appropriate input widgets:

| C# Type | Generated Input | Notes |
|---------|----------------|-------|
| `string` | `TextInput` | Can be customized to email, password, textarea, etc. |
| `bool`, `bool?` | `BoolInput` | Checkbox or toggle |
| `int`, `decimal`, `double` | `NumberInput` | Number input with validation |
| `DateTime`, `DateOnly` | `DateTimeInput` | Date/time picker |
| `Enum` | `SelectInput` | Dropdown with enum values |
| `List<Enum>` | `SelectInput` with multiple selection | Multi-select dropdown |
| Properties ending in "Id" | `ReadOnlyInput` | Typically for system-generated IDs |
| Properties ending in "Email" | `TextInput` with email validation | Email-specific input |
| Properties ending in "Password" | `PasswordInput` | Hidden text input |

### FormBuilder Methods

The FormBuilder provides a fluent API for customizing forms:

#### Field Configuration

- `.Label(field, label)` - Set custom field labels
- `.Description(field, description)` - Add help text
- `.Builder(field, inputFactory)` - Use custom input builders
- `.Required(fields...)` - Mark fields as required
- `.Validate<T>(field, validator)` - Add custom validation

#### Layout Control

- `.Place(fields...)` - Control field placement
- `.Place(true, fields...)` - Place fields on the same row
- `.Group(name, fields...)` - Group related fields
- `.Remove(fields...)` - Remove fields from form
- `.Clear()` - Remove all fields

#### Visibility Control

- `.Visible(field, predicate)` - Show/hide fields conditionally
- `.Disabled(disabled, fields...)` - Enable/disable fields

### State Management Integration

The key advantage of `.ToForm()` is seamless bi-directional state management:

```csharp
public class FormExample : ViewBase
{
    public override object? Build()
    {
        // State manages the entire form's data
        var model = UseState(() => new UserModel("", "", true));
        
        // Form automatically updates model.Value as user types
        var form = model.ToForm()
            .Required(m => m.Name, m => m.Email);
            
        // Access current form data in real-time
        var isValid = !string.IsNullOrEmpty(model.Value.Name) && 
                     !string.IsNullOrEmpty(model.Value.Email);
        
        return Layout.Vertical()
            | Text.Block($"Current name: {model.Value.Name}")
            | Text.Block($"Form is valid: {isValid}")
            | form;
    }
}
```

**Key State Management Benefits:**

- **Automatic Updates**: Form inputs automatically update `model.Value`
- **Real-time Access**: Read current form data anytime via `model.Value`
- **Type Safety**: All form data is strongly typed
- **State Persistence**: Form state persists across re-renders
- **Easy Reset**: Reset form by calling `model.Set(new Model())`

## Form Validation

### Built-in Validation

Ivy forms provide several built-in validation mechanisms:

1. **Required Fields**: Based on non-nullable types or explicit `.Required()` calls
2. **Type Validation**: Automatic validation based on property types (e.g., email format)
3. **Data Annotations**: Support for .NET validation attributes
4. **Custom Validators**: Flexible validation functions

### Custom Validation

You can add custom validation logic to any field:

```csharp
model.ToForm()
    .Validate<string>(m => m.Email, email => 
        (email.Contains("@") && email.Contains("."), "Invalid email format"))
    .Validate<int>(m => m.Age, age => 
        (age >= 18, "Must be 18 or older"))
```

### Validation Feedback

The forms system provides automatic validation feedback:

- Invalid fields are visually highlighted
- Error messages appear near the invalid fields
- Form submission is blocked until all validation passes
- Summary validation messages show total invalid field count

## Form Submission

### Basic Submission Pattern

```csharp
var formBuilder = model.ToForm();
var (onSubmit, formView, validationView, loading) = formBuilder.UseForm(this.Context);

async void HandleSubmit()
{
    if (await onSubmit()) // Returns true if valid
    {
        // Form data is automatically copied to model.Value
        client.Toast("Saved successfully!");
    }
    // If invalid, validation errors are shown automatically
}
```

### Form States

Forms track several important states:

- **Loading**: When form submission is in progress
- **Valid/Invalid**: Based on current validation results
- **Dirty**: Whether the form has been modified
- **Submitting**: During the submission process

## Advanced Features

### Conditional Fields

Show or hide fields based on other field values:

```csharp
model.ToForm()
    .Visible(m => m.AlternateEmail, m => m.HasAlternateEmail)
    .Visible(m => m.CompanyName, m => m.AccountType == AccountType.Business)
```

### Dynamic Field Configuration

Configure fields based on runtime conditions:

```csharp
var form = model.ToForm();

if (isEditMode)
{
    form.Remove(m => m.Id); // Hide ID in edit mode
}
else
{
    form.Builder(m => m.Password, s => s.ToPasswordInput()); // Require password for new users
}
```

### Forms in UI Components

Forms can be embedded in various UI components:

#### Inline Forms

```csharp
return Layout.Vertical()
    | Text.H2("User Settings")
    | model.ToForm()
    | new Button("Save", HandleSave);
```

#### Sheet Forms

```csharp
model.ToForm().ToSheet(isOpen, "Edit User", "Update user information");
```

#### Dialog Forms

```csharp
model.ToForm().ToDialog(isOpen, "Create User", width: Size.Units(400));
```

### Form Layout Patterns

#### Two-Column Layout

```csharp
model.ToForm()
    .Place(true, m => m.FirstName, m => m.LastName)
    .Place(true, m => m.City, m => m.State)
```

#### Grouped Fields

```csharp
model.ToForm()
    .Group("Personal", m => m.FirstName, m => m.LastName, m => m.Email)
    .Group("Address", m => m.Street, m => m.City, m => m.State, m => m.Zip)
```

#### Custom Field Ordering

```csharp
model.ToForm()
    .Clear() // Remove all auto-generated fields
    .Add(m => m.Name)
    .Add(m => m.Email)
    .Add(m => m.IsActive)
```

## Integration with Backend Services

Forms work seamlessly with Ivy's service layer:

```csharp
public class UserFormView : ViewBase
{
    public override object? Build()
    {
        var userService = UseService<IUserService>();
        var user = UseState(() => new UserModel("", "", true));
        
        var form = user.ToForm();
        var (onSubmit, formView, validationView, loading) = form.UseForm(this.Context);
        
        async void HandleSave()
        {
            if (await onSubmit())
            {
                await userService.SaveUser(user.Value);
                client.Toast("User saved successfully!");
            }
        }
        
        return Layout.Vertical()
            | formView
            | new Button("Save").HandleClick(new Action(HandleSave).ToEventHandler<Button>())
                .Loading(loading).Disabled(loading)
            | validationView;
    }
}
```

## Field Configuration Examples

You can configure individual fields using various builder methods:

```csharp demo-tabs
public record ContactModel(
    string Name,
    string Email,
    string Phone,
    string Message,
    bool Subscribe,
    Gender Gender = Gender.Other
);

public enum Gender { Male, Female, Other }

public class ConfiguredFormDemo : ViewBase
{
    public override object? Build()
    {
        var contact = UseState(() => new ContactModel("", "", "", "", false));
        
        return contact.ToForm()
            .Label(m => m.Name, "Full Name")
            .Description(m => m.Name, "Enter your full name as it appears on official documents")
            .Label(m => m.Email, "Email Address")
            .Description(m => m.Email, "We'll use this to send you updates")
            .Builder(m => m.Phone, s => s.ToTelInput())
            .Label(m => m.Phone, "Phone Number")
            .Builder(m => m.Message, s => s.ToTextAreaInput())
            .Label(m => m.Message, "Your Message")
            .Required(m => m.Name, m => m.Email);
    }
}
```

## Migration from Manual Forms

If you're currently using manual form layouts, migrate to `.ToForm()`:

```csharp
// ❌ Don't do this - manual form layout
var form = new Form(
    new TextInput(name),
    new TextInput(email),
    new Button("Submit")
);

// ✅ Do this instead - use .ToForm()
var model = UseState(() => new UserModel("", ""));
return model.ToForm()
    .Required(m => m.Name, m => m.Email);
```

<Callout Type="warning">
Avoid manually creating form layouts. Always use `.ToForm()` on your state objects for better state management, validation, and type safety.
</Callout>

This comprehensive forms system makes it easy to build robust, user-friendly data collection interfaces in your Ivy applications.
