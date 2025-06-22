---

prepare: |

  var client = this.UseService<IClientProvider>();

---


# TextInput


The`TextInput` widget provides a standard text entry field. It supports various text input types including single-line text, multi-line text, password fields, email, phone number, URL and offers features like placeholder text, validation, shortcut keys and text formatting.


## Basic Usage

Here's a simple example of a text input with a placeholder:

```csharp demo-tabs
public class BasicUsageDemo : ViewBase
{
    public override object? Build()
    { 
        var withoutValue = UseState((string?)null);
        return Layout.Horizontal()
            | new TextInput<string>(withoutValue, placeholder: "Enter text here...");
    }
}
```

## Variants
TextInputs come in several variants to suit different use cases:
The following blocks shows how to use these. 

### Password
For capturing passwords, `TextInputs.Password` variant needs to be used. The following code shows how to capture
a new password.   

```csharp
new TextInput<string>(password, placeholder: "Password")
    .Variant(TextInputs.Password)
```
See it in action here. 

```csharp demo-tabs
public class PasswordCaptureDemo: ViewBase
{
    public override object? Build()
    {
        var password = UseState("");
        return Layout.Horizontal(
             Text.Block("Enter Pasword"),
             new TextInput<string>(password, placeholder: "Password")
                .Variant(TextInputs.Password));         
    }
}
```

### TextArea
When a multiline text is needed, `TextInputs.Textarea` variant should be used. A common use-case is for capturing address
that typically spans over multiple lines. The following demo shows how  to use it. 

```csharp
 Text.Block("Address"),
             new TextInput<string>(address, placeholder: "Åkervägen 9, \n132 39 Saltsjö-Boo, \nSweden")
                .Variant(TextInputs.Textarea)
```
See it in action here.

```csharp demo-tabs
public class CaptureAddressDemo: ViewBase
{
    public override object? Build()
    {
        var address = UseState("");
        return Layout.Horizontal(
             Text.Block("Address"),
             new TextInput<string>(address, placeholder: "Åkervägen 9, \n132 39 Saltsjö-Boo, \nSweden")
                .Variant(TextInputs.Textarea)
                .Height(30) // Set the height
                .Width(100));// Set the width          
    }
}
```
Please note that how the newlines (`\n`) are recognized and used to create newlines in the textarea. 

### Search
When it is needed to find an element from a collection of items, it is better to give users a visual clue.  
Using the `TextInputs.Search` variant, this visual clue (with a looking glass icon) becomes obvious. 
The following demo shows how to add such an text input. 

```csharp
 new TextInput<string>(searchThis, placeholder: "search for?")
                 .Variant(TextInputs.Search)
```
see it in action here.

```csharp demo-tabs
public class SearchBarDemo: ViewBase
{
    public override object? Build()
    {
        var searchThis = UseState("");
        return Layout.Horizontal(
             Text.Block("Search"),
             new TextInput<string>(searchThis, placeholder: "search for?")
                 .Variant(TextInputs.Search));
    }
}
```

### Email 
To capture the emails `TextInputs.Email` variant should be used.  

```csharp
new TextInput<string>(email, placeholder: "user@domain.com")
                 .Variant(TextInputs.Email)
```
see it in action here.

```csharp demo-tabs
public class EmailEnterDemo: ViewBase
{
    public override object? Build()
    {
        var email = UseState("");
        return Layout.Horizontal(
             Text.Block("Email"),
             new TextInput<string>(email, placeholder: "user@domain.com")
                 .Variant(TextInputs.Email));
    }
}
```
### Telephone 
To capture the phone numbers `TextInputs.Tel` variant needs to be used.  

```csharp
new TextInput<string>(tel, placeholder: "+1-123-3456")
                 .Variant(TextInputs.Tel)
```
see it in action here.

```csharp demo-tabs
public class PhoneEnterDemo: ViewBase
{
    public override object? Build()
    {
        var tel = UseState("");
        return Layout.Horizontal(
             Text.Block("Phone"),
             new TextInput<string>(tel, placeholder: "+1-123-3456")
                 .Variant(TextInputs.Tel));
    }
}
```

### URL 
To capture the URLs/Links  `TextInputs.Url` variant needs to be used. 

```csharp
 new TextInput<string>(url, placeholder: "https://ivy.app/")
                 .Variant(TextInputs.Url)
```
see it in action here.

```csharp demo-tabs
public class URLEnterDemo: ViewBase
{
    public override object? Build()
    {
        var url = UseState("");
        return Layout.Horizontal(
             Text.Block("Website"),
             new TextInput<string>(url, placeholder: "https://ivy.app/")
                 .Variant(TextInputs.Url));
    }
}
```

## Event Handling
We can get the value of the text entered into any of the `TextInput` variant using the `OnChange` event. 

```csharp
new TextInput<string>(onChangedState.Value, e => onChangedState.Set(e.Value))
```
In this code example shown, the value of the text input will be stored in `onChangedState` variable. 
The following demo shows how to use it in a small application, where users are greeted as they enter their name.

```csharp demo-tabs
public class EventsDemoApp : ViewBase
{
     public override object? Build()
     {
        var onChangedState = UseState("");
        var onChangeLabel = UseState("");
        return Layout.Horizontal(
                new TextBlock("Name "),
                new TextInput<string>(onChangedState.Value, e =>
                    {
                       onChangedState.Set(e.Value);
                       if(e.Value.Length == 0)
                            onChangeLabel.Set("");   //Clean the text
                       else 
                            onChangeLabel.Set("Hello! " + e.Value); 
                    }),
                     onChangeLabel);
     }
}
```

## Styling
`TextInput` variants can be customized with various styling options to offer visual clues to the users.   

### Invalid
When something goes wrong capturing the inputs, `Invalid` style is recommended to be used to signal an error. 
The following code shows how to make an `TextInput` use the `Invalid` style. 

```csharp
new TextInput<string>(withoutValue, placeholder: "Styled Input")
    .Invalid("Invalid input")
```
This renders like this, an invalid text input.

```csharp demo-tabs

public class InvalidInputDemo: ViewBase
{
    public override object? Build()
    {
        var withoutValue = UseState("");
        return new TextInput<string>(withoutValue, placeholder: "a@")
                   .Invalid("Invalid email!");
    }
}
```
Whatever text is provided to the `Invalid` function, shows up when mouse is hovered on the little `i` icon in the box.

### Disabled
When it is needed to disable a `TextInput` variant, `Disabled` style is needed. 
The following code shows how to disable a `TextInput`. 

```csharp
new TextInput<string>(withoutValue, placeholder: "Styled Input")
    .Disabled()
```
This renders as shown below as a disabled text input.

```csharp demo-tabs
public class DisabledInputDemo : ViewBase
{
    public override object? Build()
    {
        return Layout.Horizontal()
            | new TextInput<string>(UseState(""), placeholder: "Disabled Input").Disabled();
    }
}
```

### Real life usages of styles
The following demo shows how to use style like `Invalid` in form validations. 

#### Validate Email Demo
In this example, if the email format is wrong, the input is invalidated and a message is shown.

```csharp demo-tabs
public class EmailValidationDemo : ViewBase 
{      
    // Email regex pattern
    private static readonly System.Text.RegularExpressions.Regex EmailRegex = new 
        System.Text.RegularExpressions.Regex(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        System.Text.RegularExpressions.RegexOptions.Compiled | 
        System.Text.RegularExpressions.RegexOptions.IgnoreCase
    );

    public override object? Build()
    {         
        var onChangedState = UseState("");         
        var invalidState = UseState("");         
        
        return Layout.Horizontal(       
            new TextBlock("Email"),
            new TextInput<string>(onChangedState.Value, e =>                    
            {                        
                onChangedState.Set(e.Value);
                if (string.IsNullOrWhiteSpace(e.Value))
                {
                    invalidState.Set("");
                }
                else if (!EmailRegex.IsMatch(e.Value))                        
                {                             
                    invalidState.Set("Invalid email address");
                }                        
                else                        
                {                         
                    // Clear the invalid state
                    invalidState.Set(""); 
                }                    
            })
            .Invalid(invalidState.Value)
        ); 
    }     
}
```
#### Conditional Enabling/Disabling of Text Inputs
In this demo, password field is enabled only when the username field has a value. 

```csharp demo-tabs

public class LoginForm : ViewBase 
{      
    public override object Build()
    {         
        var usernameState = UseState("");         
        var passwordState = UseState("");         
        
        return Layout.Vertical()
            | Text.H2("Login")
            | Layout.Vertical()
                | Text.Label("Username")
                | usernameState.ToTextInput()
                    .Placeholder("Enter your username")
                | Text.Label("Password") 
                | passwordState.ToPasswordInput()
                    .Placeholder("Enter your password")
                     // Disabled when username is empty
                    .Disabled(string.IsNullOrWhiteSpace(usernameState.Value))
                | Layout.Horizontal()
                    | new Button("Login")
                        .Disabled(string.IsNullOrWhiteSpace(usernameState.Value) || 
                             string.IsNullOrWhiteSpace(passwordState.Value)); 
                            
    }     
}

```
Notice, how extension functions `ToTextInput`, and `ToPasswordInput` are used  to generate `TextInput` variants
needed for the form. 

## Shortcuts
We can use associate keyboard shortcuts to text inputs the following way. 

```csharp
 new TextInput<string>(name, placeholder: "Name (Ctrl+N)")
                     .ShortcutKey("Ctrl+N")   
```

The following demo shows this in action with multiple text inputs each 
with different shortcut keys.

```csharp demo-tabs
public class ShortCutDemo : ViewBase
{
    public override object? Build()
    { 
        var name = UseState("");
        var email = UseState("");
        var message = UseState("");
        return Layout.Vertical()
                | new TextBlock("Keyboard Shortcuts Demo")
                | new TextBlock("Ctrl+N - Focus Name, Ctrl+E - Focus Email, Ctrl+M - Focus Message")  
                | new TextInput<string>(name, placeholder: "Name (Ctrl+N)")
                   .ShortcutKey("Ctrl+N")    
                | new TextInput<string>(email, placeholder: "Email (Ctrl+E)")
                   .ShortcutKey("Ctrl+E")
                    .Variant(TextInputs.Email)    
                | new TextInput<string>(message, placeholder: "Message (Ctrl+M)")
                    .ShortcutKey("Ctrl+M")
                    .Variant(TextInputs.Textarea);
    }
}
```




<WidgetDocs Type="Ivy.TextInput" ExtensionTypes="Ivy.TextInputExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Inputs/TextInput.cs"/>
