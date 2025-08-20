# Callout

<Ingress>
Create attention-grabbing components to highlight important information, warnings, tips, and success messages. Callouts come in different variants including info, warning, error, and success, with customizable icons and styling.
</Ingress>

The `Callout` widget displays prominent, styled information boxes that draw attention to important content. They're perfect for user guidance, system messages, warnings, and success confirmations. Each variant has distinct visual styling and appropriate icons to help users quickly understand the message type.

## Basic Usage

The simplest way to create a callout is using the static factory methods:

```csharp demo-tabs
public class BasicCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Callout.Info("This is an informational message that provides helpful context.")
            | Callout.Success("Operation completed successfully!")
            | Callout.Warning("Please review your input before proceeding.")
            | Callout.Error("An error occurred while processing your request.");
    }
}
```

### With Titles

Add descriptive titles to make your callouts more informative:

```csharp demo-tabs
public class TitledCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Callout.Info("This feature requires admin privileges.", "Access Note")
            | Callout.Success("Your settings have been saved successfully!", "Success")
            | Callout.Warning("Changes made here cannot be automatically undone.", "Caution")
            | Callout.Error("API connection failed. Check your network settings.", "Connection Error");
    }
}
```

### Constructor Syntax

You can also use the constructor directly for more control:

```csharp demo-tabs
public class ConstructorCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | new Callout("This is a basic info callout")
            | new Callout("Success message with title", "Operation Complete", CalloutVariant.Success)
            | new Callout("Warning with custom icon", "Important Notice", CalloutVariant.Warning, Icons.TriangleAlert)
            | new Callout("Error details here", "System Error", CalloutVariant.Error, Icons.Bug);
    }
}
```

<Callout Type="Tip">
The `Callout` widget has the following properties:
- Title (string): Optional title displayed above the description
- Variant (CalloutVariant): The visual style variant (Info, Success, Warning, Error)
- Icon (Icons): Optional custom icon to override the default variant icon
- Children: The main content/description of the callout
</Callout>

## Variants

### Info Callouts

Info callouts are perfect for general information, tips, and helpful guidance:

```csharp demo-tabs
public class InfoCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Callout.Info("Welcome to the new dashboard! Here you can monitor all your system metrics in real-time.")
            | Callout.Info("Pro tip: Use the search bar to quickly find what you're looking for.", "Quick Tip")
            | Callout.Info("This feature is currently in beta. Please report any issues you encounter.", "Beta Feature")
            | Callout.Info("Remember to save your work frequently. Auto-save is enabled every 5 minutes.", "Workflow Reminder");
    }
}
```

### Success Callouts

Success callouts confirm completed actions and positive outcomes:

```csharp demo-tabs
public class SuccessCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Callout.Success("Your profile has been updated successfully!")
            | Callout.Success("File uploaded successfully. You can now share it with your team.", "Upload Complete")
            | Callout.Success("Payment processed successfully. A confirmation email has been sent.", "Payment Confirmed")
            | Callout.Success("All changes have been saved and synchronized across devices.", "Sync Complete");
    }
}
```

### Warning Callouts

Warning callouts alert users to potential issues or important considerations:

```csharp demo-tabs
public class WarningCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Callout.Warning("This action cannot be undone. Please confirm before proceeding.")
            | Callout.Warning("Your session will expire in 5 minutes. Please save your work.", "Session Expiry")
            | Callout.Warning("Some features may not work properly in older browsers.", "Browser Compatibility")
            | Callout.Warning("Large file uploads may take several minutes to complete.", "Upload Time");
    }
}
```

### Error Callouts

Error callouts communicate problems and guide users toward solutions:

```csharp demo-tabs
public class ErrorCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Callout.Error("Failed to connect to the server. Please check your internet connection.")
            | Callout.Error("Invalid email format. Please enter a valid email address.", "Validation Error")
            | Callout.Error("Access denied. You don't have permission to perform this action.", "Permission Error")
            | Callout.Error("The requested resource was not found. It may have been moved or deleted.", "Resource Not Found");
    }
}
```

## Custom Icons

Override the default variant icons with custom ones for more specific messaging:

```csharp demo-tabs
public class CustomIconCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Callout.Info("New features available!", "What's New").Icon(Icons.Sparkles)
            | Callout.Success("Backup completed successfully!", "Backup Status").Icon(Icons.Database)
            | Callout.Warning("Maintenance scheduled for tonight", "System Notice").Icon(Icons.Wrench)
            | Callout.Error("Security alert detected", "Security Warning").Icon(Icons.Shield)
            | Callout.Info("Download in progress...", "File Transfer").Icon(Icons.Download)
            | Callout.Success("Integration connected successfully!", "Connection Status").Icon(Icons.Link);
    }
}
```

## Using Extensions

The Callout widget provides fluent extension methods for easy customization:

```csharp demo-tabs
public class ExtensionCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Callout.Info("Basic message")
                .Title("Custom Title")
                .Icon(Icons.Star)
            | Callout.Warning("Warning message")
                .Description("This is a detailed warning description that explains the issue.")
                .Variant(CalloutVariant.Error)
                .Icon(Icons.TriangleAlert)
            | Callout.Success("Success message")
                .Title("Operation Complete")
                .Description("Your request has been processed successfully.")
                .Icon(Icons.Check);
    }
}
```

## Complex Content

Callouts can contain rich content beyond simple text:

```csharp demo-tabs
public class ComplexContentCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | new Callout(
                Layout.Vertical().Gap(2)
                    | Text.P("This callout contains multiple elements including badges and rich content. You can include any widgets as children!")
                    | Layout.Horizontal().Gap(2)
                        | new Badge("Feature", BadgeVariant.Secondary)
                        | new Badge("New", BadgeVariant.Primary)
                    | Text.P("Additional content can be added as children"),
                "Rich Content Example",
                CalloutVariant.Info)
            | new Callout(
                Layout.Vertical().Gap(1)
                    | Text.P("Please review the following items before proceeding:")
                    | Text.P("• Check your email settings")
                    | Text.P("• Verify your phone number")
                    | Text.P("• Update your profile picture"),
                "Action Required",
                CalloutVariant.Warning)
            | new Callout(
                Layout.Vertical().Gap(2)
                    | Text.P("System error details are shown below. Please contact support if this issue persists.")
                    | new Code("Error Code: E-1001\nTimestamp: 2024-01-15 14:30:00")
                    | Text.P("Technical details for debugging"),
                "System Error",
                CalloutVariant.Error);
    }
}
```

## Form Integration

Use callouts within forms to provide context and validation feedback:

```csharp demo-tabs
public class FormCalloutView : ViewBase
{
    public override object? Build()
    {
        var emailState = UseState("");
        var passwordState = UseState("");
        
        return Layout.Vertical().Gap(4)
            | Callout.Info("All fields marked with * are required. Your information will be kept secure.", "Form Guidelines")
            | Layout.Vertical().Gap(2)
                | Text.Label("Email Address *")
                | new TextInput(emailState, placeholder: "Enter your email")
                | Callout.Warning("Please use your work email address for business communications.", "Email Policy")
            | Layout.Vertical().Gap(2)
                | Text.Label("Password *")
                | new TextInput(passwordState, placeholder: "Enter your password", variant: TextInputs.Password)
                | Callout.Info("Password must be at least 8 characters with uppercase, lowercase, and numbers.", "Password Requirements")
            | Layout.Horizontal().Gap(2)
                | new Button("Submit", variant: ButtonVariant.Primary)
                | new Button("Cancel", variant: ButtonVariant.Secondary);
    }
}
```

## Dashboard Notifications

Create informative dashboard notifications with callouts:

```csharp demo-tabs
public class DashboardCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Layout.Horizontal().Gap(4)
                | Layout.Vertical().Gap(2)
                    | Callout.Success("Revenue increased by 15% this month", "Financial Update").Icon(Icons.TrendingUp)
                    | Callout.Info("3 new team members joined this week", "Team Update").Icon(Icons.Users)
                | Layout.Vertical().Gap(2)
                    | Callout.Warning("Server maintenance scheduled for 2:00 AM", "System Notice").Icon(Icons.Server)
                    | Callout.Error("2 failed login attempts detected", "Security Alert").Icon(Icons.Shield)
            | Layout.Horizontal().Gap(4)
                | Callout.Info("Your daily backup is running in the background", "Background Process").Icon(Icons.Clock)
                | Callout.Success("All systems operational", "System Status").Icon(Icons.Check);
    }
}
```

## Responsive Layouts

Callouts work well in various layout configurations:

```csharp demo-tabs
public class ResponsiveCalloutView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Text.H3("Horizontal Layout")
            | Layout.Horizontal().Gap(4)
                | Callout.Info("Left column info", "Column 1")
                | Callout.Success("Right column success", "Column 2")
            | Text.H3("Grid Layout")
            | Layout.Grid(2).Gap(4)
                | Callout.Warning("Grid cell 1", "Grid Layout")
                | Callout.Error("Grid cell 2", "Grid Layout")
            | Text.H3("Compact Horizontal Layout")
            | Layout.Horizontal().Gap(2)
                | Callout.Info("Compact", "Small").Icon(Icons.Info)
                | Callout.Success("Compact", "Small").Icon(Icons.Check)
                | Callout.Warning("Compact", "Small").Icon(Icons.TriangleAlert)
                | Callout.Error("Compact", "Small").Icon(Icons.X);
    }
}
```

<WidgetDocs Type="Ivy.Callout" ExtensionTypes="Ivy.CalloutExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Callout.cs"/>
