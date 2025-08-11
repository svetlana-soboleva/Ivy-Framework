# Sheet

`Sheets` slide in from the side of the screen and display additional content while
allowing the user to dismiss them. They provide a non-intrusive way to show additional information or forms without navigating away from the current page.

## Basic Usage

The `WithSheet` extension on a `Button` provides an easy way to open a sheet:

```csharp demo-tabs ivy-bg
public class BasicSheetExample : ViewBase
{
    public override object? Build()
    {
        return new Button("Open Sheet").WithSheet(
            () => new SheetView(),
            title: "This is a sheet",
            description: "Lorem ipsum dolor sit amet",
            width: Size.Fraction(1/2f)
        );
    }
}

public class SheetView : ViewBase
{
    public override object? Build()
    {
        return new Card(
            "Welcome to the sheet!",
            "This is the content inside the sheet"
        );
    }
}
```

## Properties

- **`Title`** - Optional title displayed at the top of the sheet
- **`Description`** - Optional description text below the title
- **`Width`** - Controls the width of the sheet (defaults to `Size.Rem(24)`)
- **`OnClose`** - Event handler called when the sheet is closed

## Width Options

Sheets support various width configurations:

- **`Size.Fraction(1/2f)`** - Half the screen width
- **`Size.Fraction(1/3f)`** - One-third screen width  
- **`Size.Rem(24)`** - 24rem (default)
- **`Size.Units(150)`** - 150 units
- **`Size.Px(400)`** - 400 pixels
- **`Size.Full()`** - Full screen width

## Examples

### Basic Sheet with Custom Content

```csharp demo-tabs ivy-bg
public class BasicSheetWithContent : ViewBase
{
    public override object? Build()
    {
        return new Button("Open Basic Sheet").WithSheet(
            () => new Fragment(
                new Card(
                    "Welcome to the sheet!",
                    new Button("Action Button", onClick: _ => { })
                ).Title("Sheet Content").Description("This is a simple sheet with custom content")
            ),
            title: "Basic Sheet",
            description: "A simple example of sheet usage",
            width: Size.Fraction(1/3f)
        );
    }
}
```

### Sheet with Footer Actions

```csharp demo-tabs ivy-bg
public class SheetWithFooterActions : ViewBase
{
    public override object? Build()
    {
        return new Button("Open Sheet with Actions").WithSheet(
            () => new FooterLayout(
                Layout.Horizontal().Gap(2)
                    | new Button("Save").Variant(ButtonVariant.Primary).HandleClick(_ => { })
                    | new Button("Cancel").Variant(ButtonVariant.Outline).HandleClick(_ => { }),
                new Card(
                    "This sheet has action buttons in the footer"
                ).Title("Content")
            ),
            title: "Actions Sheet",
            width: Size.Fraction(1/2f)
        );
    }
}
```

### Sheet with Complex Layout

```csharp demo-tabs ivy-bg
public class ComplexSheetLayout : ViewBase
{
    public override object? Build()
    {
        return new Button("Open Complex Sheet").WithSheet(
            () => Layout.Vertical()
                | new Card(
                    Layout.Horizontal()
                        | new Avatar("JD").Size(64)
                        | Layout.Vertical()
                            | Text.H3("John Doe")
                            | Text.Small("john.doe@example.com")
                ).Title("User Information")
                | new Card(
                    Layout.Vertical()
                        | new BoolInput("Dark Mode", true)
                        | new BoolInput("Notifications", false)
                        | new SelectInput<string>(options: new[] { "English", "Spanish", "French" }.ToOptions())
                ).Title("Preferences")
                | new Card(
                    Layout.Horizontal().Gap(2)
                        | new Button("Update Profile")
                        | new Button("Change Password")
                        | new Button("Delete Account").Variant(ButtonVariant.Destructive)
                ).Title("Actions"),
            title: "User Profile",
            description: "Manage your account settings and preferences",
            width: Size.Fraction(2/3f)
        );
    }
}
```

### Sheet with Different Widths

```csharp demo-tabs ivy-bg
public class SheetWidthExamples : ViewBase
{
    public override object? Build()
    {
        return Layout.Horizontal().Gap(2)
            | new Button("Small Sheet").WithSheet(
                () => new Card("This is a small sheet").Title("Small Content"),
                title: "Small",
                width: Size.Rem(20)
            )
            | new Button("Medium Sheet").WithSheet(
                () => new Card("This is a medium sheet").Title("Medium Content"),
                title: "Medium",
                width: Size.Fraction(1/2f)
            )
            | new Button("Large Sheet").WithSheet(
                () => new Card("This is a large sheet").Title("Large Content"),
                title: "Large",
                width: Size.Fraction(2/3f)
            )
            | new Button("Full Sheet").WithSheet(
                () => new Card("This is a full-width sheet").Title("Full Content"),
                title: "Full Width",
                width: Size.Full()
            );
    }
}
```

### Sheet with Navigation

```csharp demo-tabs ivy-bg
public class NavigationSheet : ViewBase
{
    public override object? Build()
    {
        return new Button("Open Navigation Sheet").WithSheet(
            () => new NavigationSheetContent(),
            title: "Navigation Sheet",
            width: Size.Fraction(1/2f)
        );
    }
}

public class NavigationSheetContent : ViewBase
{
    public override object? Build()
    {
        var currentPage = UseState(0);
        var pages = new[] { "Home", "Profile", "Settings", "Help" };
        
        return Layout.Vertical()
            | Layout.Horizontal().Gap(2)
                | pages.Select((page, index) => 
                    new Button(page)
                        .Variant(currentPage.Value == index ? ButtonVariant.Primary : ButtonVariant.Outline)
                        .HandleClick(_ => currentPage.Value = index)
                ).ToArray()
            | new Card(
                $"This is the {pages[currentPage.Value]} page content"
            ).Title("Page Content");
    }
}
```

## Best Practices

1. **Use descriptive titles** - Make it clear what the sheet contains
2. **Keep content focused** - Don't overload sheets with too much information
3. **Provide clear actions** - Include save/cancel buttons when appropriate
4. **Consider width carefully** - Use appropriate sizing for the content
5. **Handle state properly** - Ensure sheet state is managed correctly in your views

## Integration with Other Widgets

Sheets work seamlessly with other Ivy widgets:

- **Forms** - Use `ToSheet()` for form-based sheets
- **Cards** - Organize content within sheets using cards
- **Layouts** - Use `Layout.Vertical()` and `Layout.Horizontal()` for structured content
- **Buttons** - Include action buttons in sheet footers
- **Inputs** - Display form inputs and controls within sheets

<WidgetDocs Type="Ivy.Sheet" ExtensionTypes="Ivy.SheetExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Sheet.cs"/>
