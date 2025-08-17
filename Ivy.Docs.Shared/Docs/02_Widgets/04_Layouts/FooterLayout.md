# FooterLayout

<Ingress>
FooterLayout creates a layout with a fixed footer at the bottom and scrollable content above it. It's perfect for forms, sheets, and any interface where you need persistent action buttons or information while allowing the main content to scroll independently.
</Ingress>

The `FooterLayout` widget is designed to keep important actions or information visible at the bottom of the view while allowing the main content to scroll freely above it. This pattern is commonly used in forms, modal dialogs, and sheet interfaces where users need constant access to primary actions.

## Basic Usage

Create a simple footer layout with content and footer elements:

```csharp demo-tabs
public class BasicFooterExample : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        
        return Layout.Vertical()
            | new Card("Header Section")
                    | Text.H2("Welcome to Ivy Framework")
            | new FooterLayout(
                footer: new Button("Save", _ => client.Toast("Content saved!"))
                    .Variant(ButtonVariant.Primary),
                content: Layout.Vertical().Gap(3)
                    | Text.P("This is the main content area that demonstrates how content can scroll independently above the footer.")
        );
    }
}
```

## Form with Footer Actions

A common use case is creating forms with persistent action buttons:

```csharp demo-tabs
public class FormWithFooterExample : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        var firstName = UseState("John");
        var lastName = UseState("Doe");
        var email = UseState("john.doe@example.com");
        var bio = UseState("Software developer with 5+ years of experience...");
        
        return Layout.Vertical().Gap(4)
            | new Card("Form Header")
            | new FooterLayout(
                footer: Layout.Horizontal().Gap(2).Align(Align.Right)
                    | new Button("Cancel", _ => client.Toast("Cancelled"))
                    | new Button("Submit", _ => client.Toast("Form submitted"))
                        .Variant(ButtonVariant.Primary),
                content: Layout.Vertical().Gap(4)
                    | new Card(Layout.Vertical().Gap(3)
                        | new TextInput(firstName, "First Name")
                        | new TextInput(lastName, "Last Name")
                        | new TextInput(email, "Email Address")
                    ).Title("Personal Information")
            );
    }
}
```

## Sheet Interface

FooterLayout is commonly used in sheet interfaces for consistent action placement:

```csharp demo-tabs
public class SheetWithFooterExample : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        var title = UseState("Getting Started with Ivy Framework");
        var content = UseState("Write your article content here...");
        
        return Layout.Vertical().Gap(4)
            | new Card("Sheet Header")
                    | Layout.Vertical()
                        | Text.H1("Article Editor")
                        | Text.Small("Create and edit your articles with ease").Color(Colors.Gray)
            | new FooterLayout(
                footer: Layout.Horizontal().Gap(2).Align(Align.Right)
                    | new Button("Save Draft", _ => client.Toast("Draft saved"))
                    | new Button("Publish", _ => client.Toast("Published!"))
                        .Variant(ButtonVariant.Primary),
                content: Layout.Vertical().Gap(4)
                    | new Card(Layout.Vertical().Gap(3)
                        | new TextInput(title, "Article Title")
                        | new TextInput(content, "Article Content").Variant(TextInputs.Textarea)
                    ).Title("Article Details")
                    | new Card("Article preview will appear here as you type...")
                        .Title("Preview")
                    | new Card("Meta description and keywords for search engines...")
                        .Title("SEO Information")
            );
    }
}
```

## Complex Footer with Multiple Elements

Create sophisticated footers with various components:

```csharp demo-tabs
public class ComplexFooterExample : ViewBase
{
    public override object? Build()
    {
        var client = UseService<IClientProvider>();
        var docTitle = UseState("Project Proposal");
        var summary = UseState("This project aims to...");
        var details = UseState("The implementation will use...");
        
        return Layout.Vertical().Gap(4)
            | new Card("Project Header")
                    | Layout.Vertical()
                        | Text.H1("Document Editor")
                        | Text.Small("Comprehensive project management tool").Color(Colors.Gray)
            | new FooterLayout(
                footer: Layout.Horizontal().Gap(2).Align(Align.Right)
                    | new Badge("Draft").Variant(BadgeVariant.Secondary)
                    | new Button("Save Draft", _ => client.Toast("Draft saved"))
                    | new Button("Submit", _ => client.Toast("Submitted for review"))
                        .Variant(ButtonVariant.Primary),
                content: Layout.Vertical().Gap(4)
                    | new Card(Layout.Vertical().Gap(3)
                        | new TextInput(docTitle, "Document Title")
                        | new TextInput(summary, "Executive Summary").Variant(TextInputs.Textarea)
                    ).Title("Document Information")
                    | new Card(Layout.Vertical().Gap(3)
                        | new TextInput(details, "Technical Details").Variant(TextInputs.Textarea)
                    ).Title("Technical Specifications")
                    | new Card("• All required fields are completed\n• Technical specifications are accurate\n• Timeline is realistic and achievable")
                        .Title("Review Checklist")
            );
    }
}
```

