# Prompts

<Ingress>
Interact with users and gather input using Ivy's prompt system.
</Ingress>

## Overview

The prompt system in Ivy supports:

- Confirmation dialogs
- Text input prompts
- File selection prompts
- Custom prompts
- Prompt chaining
- Async prompt handling

## Basic Usage

Here's a simple example of showing a confirmation prompt:

```csharp demo-tabs
public class BasicPromptView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        
        return new Button("Delete Item", onClick: async _ =>
        {
            var result = await client.Prompt.ConfirmAsync(
                "Delete Item",
                "Are you sure you want to delete this item?"
            );
            if (result)
            {
                // Item would be deleted here
                client.Toast("Item deleted!", "Success");
            }
        });
    }
}
```

### Confirmation Prompts

Get user confirmation for important actions:

```csharp demo-tabs
public class DeleteView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        
        return new Button(
            "Delete",
            variant: ButtonVariant.Destructive,
            onClick: async _ => {
                var confirmed = await client.Prompt.ConfirmAsync(
                    "Delete Item",
                    "This action cannot be undone. Are you sure?"
                );
                
                if (confirmed)
                {
                    // Item would be deleted here
                    client.Toast("Item deleted!", "Success");
                }
            }
        );
    }
}
```

### Text Input Prompts

Collect text input from users:

```csharp demo-tabs
public class RenameView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        
        return new Button(
            "Rename",
            onClick: async _ => {
                var newName = await client.Prompt.TextAsync(
                    "Rename Item",
                    "Enter new name:",
                    defaultValue: "Current Item Name"
                );
                
                if (newName != null)
                {
                    // Item would be renamed here
                    client.Toast($"Renamed to: {newName}", "Success");
                }
            }
        );
    }
}
```

### File Selection Prompts

Handle file selection through prompts:

```csharp demo-tabs
public class UploadView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        
        return new Button(
            "Upload File",
            onClick: async _ => {
                var files = await client.Prompt.FilesAsync(
                    "Select Files",
                    multiple: true,
                    accept: ".pdf,.doc,.docx"
                );
                
                if (files?.Length > 0)
                {
                    // Files would be uploaded here
                    client.Toast($"Selected {files.Length} file(s)", "Success");
                }
            }
        );
    }
}
```

### Custom Prompts

Create custom prompts for specific needs:

```csharp demo-tabs
public class CustomPromptView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        
        return new Button(
            "Custom Prompt",
            onClick: async _ => {
                var result = await client.Prompt.CustomAsync(
                    "Select Options",
                    new CustomPromptOptions
                    {
                        Content = new VerticalLayout(
                            Text.Inline("Choose your options:"),
                            new Checkbox("Option 1"),
                            new Checkbox("Option 2"),
                            new TextInput("Custom text")
                        ),
                        Buttons = new[]
                        {
                            new PromptButton("Cancel", PromptButtonType.Cancel),
                            new PromptButton("Save", PromptButtonType.Confirm)
                        }
                    }
                );
                
                if (result.Confirmed)
                {
                    // Options would be saved here
                    client.Toast("Options saved!", "Success");
                }
            }
        );
    }
}
```

### Prompt Chaining

Chain multiple prompts together:

```csharp demo-tabs
public class MultiStepView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        
        return new Button(
            "Start Process",
            onClick: async _ => {
                // Step 1: Confirmation
                var confirmed = await client.Prompt.ConfirmAsync(
                    "Start Process",
                    "Begin the multi-step process?"
                );
                
                if (!confirmed) return;
                
                // Step 2: Text Input
                var name = await client.Prompt.TextAsync(
                    "Enter Name",
                    "What is your name?"
                );
                
                if (name == null) return;
                
                // Step 3: File Selection
                var files = await client.Prompt.FilesAsync(
                    "Select Files",
                    multiple: true
                );
                
                if (files?.Length > 0)
                {
                    // Files would be processed here
                    client.Toast($"Processing {files.Length} files for {name}", "Success");
                }
            }
        );
    }
}
```

### Best Practices

1. **Clear Messages**: Use clear and concise prompt messages
2. **Default Values**: Provide sensible default values when appropriate
3. **Validation**: Implement input validation for text prompts
4. **Error Handling**: Handle prompt cancellation and errors gracefully
5. **User Experience**: Keep prompts focused and avoid asking too many questions
6. **Accessibility**: Ensure prompts are accessible to screen readers
7. **Consistency**: Maintain consistent prompt styling across your project

## Examples

### Form Validation with Prompts

```csharp demo-tabs
public class FormView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var name = UseState("");
        
        return Layout.Vertical().Gap(4)
            | name.ToTextInput(placeholder: "Enter your name...")
            | new Button("Submit", onClick: async _ => {
                if (string.IsNullOrEmpty(name.Value))
                {
                    var promptedName = await client.Prompt.TextAsync(
                        "Missing Name",
                        "Please enter your name:"
                    );
                    
                    if (promptedName == null) return;
                    name.Set(promptedName);
                }
                
                // Form would be submitted here
                client.Toast($"Form submitted for {name.Value}", "Success");
            });
    }
}
```

### Confirmation with Custom Options

```csharp demo-tabs
public class DeleteWithOptionsView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        
        return new Button(
            "Delete with Options",
            variant: ButtonVariant.Destructive,
            onClick: async _ => {
                var result = await client.Prompt.CustomAsync(
                    "Delete Options",
                    new CustomPromptOptions
                    {
                        Content = new VerticalLayout(
                            Text.Inline("Choose deletion options:"),
                            new Checkbox("Delete associated files"),
                            new Checkbox("Archive instead of delete"),
                            new TextInput("Reason for deletion")
                        ),
                        Buttons = new[]
                        {
                            new PromptButton("Cancel", PromptButtonType.Cancel),
                            new PromptButton("Delete", PromptButtonType.Destructive)
                        }
                    }
                );
                
                if (result.Confirmed)
                {
                    // Deletion with options would be performed here
                    client.Toast("Item deleted with custom options", "Success");
                }
            }
        );
    }
}
```
