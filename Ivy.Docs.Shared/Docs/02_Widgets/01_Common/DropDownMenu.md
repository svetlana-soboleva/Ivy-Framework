---
prepare: |
  var client = this.UseService<IClientProvider>();
---

# DropDownMenu

<Ingress>
Create interactive dropdown menus with customizable options, actions, and styling for navigation and user choices. DropDownMenu provides a flexible way to display hierarchical menus with various item types including checkboxes, radio buttons, separators, and nested submenus.
</Ingress>

## Basic Usage

Here's a simple example of a `DropDownMenu` that shows a toast message when an item is selected:

```csharp demo-tabs
Layout.Horizontal()
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Basic Menu"),
        MenuItem.Default("Profile"), 
        MenuItem.Default("Settings"), 
        MenuItem.Default("Logout"))
```

## Menu Item Types

### Default Menu Items

Basic menu items that trigger actions when selected:

```csharp demo-tabs
Layout.Horizontal().Gap(2)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Default Items"),
        MenuItem.Default("Copy"),
        MenuItem.Default("Paste"),
        MenuItem.Default("Cut"),
        MenuItem.Default("Delete"))
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("With Tags"),
        MenuItem.Default("Save").Tag("save-action"),
        MenuItem.Default("Export").Tag("export-action"),
        MenuItem.Default("Import").Tag("import-action"))
```

### Checkbox Menu Items

Menu items with checkbox functionality:

```csharp demo-tabs
Layout.Horizontal().Gap(2)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Checkboxes"),
        MenuItem.Checkbox("Dark Theme").Checked(),
        MenuItem.Checkbox("Notifications"),
        MenuItem.Checkbox("Auto-save").Checked(),
        MenuItem.Checkbox("Debug Mode"))
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Mixed Types"),
        MenuItem.Default("Profile"),
        MenuItem.Separator(),
        MenuItem.Checkbox("Email Notifications").Checked(),
        MenuItem.Checkbox("SMS Notifications"),
        MenuItem.Checkbox("Push Notifications").Checked())
```

### Separators

Add visual separation between menu sections:

```csharp demo-tabs
new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
    new Button("With Separators"))
    | MenuItem.Default("New File")
    | MenuItem.Default("Open File")
    | MenuItem.Separator()
    | MenuItem.Default("Save")
    | MenuItem.Default("Save As")
    | MenuItem.Separator()
    | MenuItem.Default("Print")
    | MenuItem.Default("Export")
```

### Nested Menu Items

Create hierarchical menu structures using the Children method:

```csharp demo-tabs
new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
    new Button("Nested Items"))
    | MenuItem.Default("File")
        .Children(
            MenuItem.Default("New"),
            MenuItem.Default("Open"),
            MenuItem.Default("Save")
        )
    | MenuItem.Default("Edit")
        .Children(
            MenuItem.Default("Undo"),
            MenuItem.Default("Redo"),
            MenuItem.Default("Cut")
        )
```

## Positioning Options

### Side Positioning

Control which side of the trigger the menu appears on:

```csharp demo-tabs
Layout.Grid().Columns(2).Gap(4)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Top"), MenuItem.Default("Item 1"), MenuItem.Default("Item 2"))
        .Top()
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Right"), MenuItem.Default("Item 1"), MenuItem.Default("Item 2"))
        .Right()
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Bottom"), MenuItem.Default("Item 1"), MenuItem.Default("Item 2"))
        .Bottom()
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Left"), MenuItem.Default("Item 1"), MenuItem.Default("Item 2"))
        .Left()
```

### Alignment Options

Control the alignment of the menu relative to the trigger:

```csharp demo-tabs
Layout.Horizontal().Gap(2)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Start"), MenuItem.Default("Item 1"), MenuItem.Default("Item 2"))
        .Align(DropDownMenu.AlignOptions.Start)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Center"), MenuItem.Default("Item 1"), MenuItem.Default("Item 2"))
        .Align(DropDownMenu.AlignOptions.Center)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("End"), MenuItem.Default("Item 1"), MenuItem.Default("Item 2"))
        .Align(DropDownMenu.AlignOptions.End)
```

### Offset Control

Fine-tune the positioning with offset values:

```csharp demo-tabs
Layout.Horizontal().Gap(2)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Small Offset"), MenuItem.Default("Item 1"), MenuItem.Default("Item 2"))
        .SideOffset(0)
        .AlignOffset(0)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Large Offset"), MenuItem.Default("Item 1"), MenuItem.Default("Item 2"))
        .SideOffset(15)
        .AlignOffset(15)
```

## Advanced Features

### Headers

Add informational headers to your dropdown menus:

```csharp demo-tabs
Layout.Horizontal().Gap(2)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("With Header"),
        MenuItem.Separator(),
        MenuItem.Default("Profile"),
        MenuItem.Default("Settings"),
        MenuItem.Default("Logout"))
        .Header(Text.Muted("Signed in as user@example.com"))
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Complex Header"),
        MenuItem.Separator(),
        MenuItem.Default("View Profile"),
        MenuItem.Default("Account Settings"))
        .Header(Text.Label("John Doe - Administrator"))
```

### Nested Submenus

Create hierarchical menu structures:

```csharp demo-tabs
new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
    new Button("Nested Menus"))
    | MenuItem.Default("File")
        .Children(
            MenuItem.Default("New"),
            MenuItem.Default("Open"),
            MenuItem.Default("Save"),
            MenuItem.Separator(),
            MenuItem.Default("Export")
                .Children(
                    MenuItem.Default("PDF"),
                    MenuItem.Default("Word"),
                    MenuItem.Default("Excel")
                )
        )
    | MenuItem.Default("Edit")
        .Children(
            MenuItem.Default("Undo"),
            MenuItem.Default("Redo"),
            MenuItem.Separator(),
            MenuItem.Default("Find"),
            MenuItem.Default("Replace")
        )
```

### Button Integration

Use the convenient `WithDropDown` extension method on buttons:

```csharp demo-tabs
Layout.Horizontal().Gap(2)
    | new Button("Quick Menu")
        .WithDropDown(
            MenuItem.Default("Option 1"),
            MenuItem.Default("Option 2"),
            MenuItem.Default("Option 3")
        )
    | new Button("Settings")
        .Secondary()
        .WithDropDown(
            MenuItem.Default("Preferences"),
            MenuItem.Default("Account"),
            MenuItem.Default("Help")
        )
```

### Custom Event Handling

Implement custom selection logic:

```csharp demo-tabs
new DropDownMenu(@evt => {
        var value = @evt.Value?.ToString();
        if (value == "delete") {
            client.Toast("Deleting item...");
        } else if (value == "export") {
            client.Toast("Exporting data...");
        } else {
            client.Toast($"Selected: {value}");
        }
    }, 
    new Button("Custom Actions"))
    | MenuItem.Default("View").Tag("view")
    | MenuItem.Default("Edit").Tag("edit")
    | MenuItem.Default("Delete").Tag("delete")
    | MenuItem.Separator()
    | MenuItem.Default("Export").Tag("export")
    | MenuItem.Default("Share").Tag("share")
```

## Complete Example

Here's a comprehensive example combining multiple features:

```csharp demo-tabs
Layout.Horizontal().Gap(2)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("User Menu"),
        MenuItem.Separator(),
        MenuItem.Default("View Profile").Tag("profile"),
        MenuItem.Default("Account Settings").Tag("settings"),
        MenuItem.Default("Preferences").Tag("preferences"),
        MenuItem.Separator(),
        MenuItem.Default("Theme")
            .Children(
                MenuItem.Checkbox("Light").Tag("theme-light"),
                MenuItem.Checkbox("Dark").Checked().Tag("theme-dark"),
                MenuItem.Checkbox("System").Tag("theme-system")
            ),
        MenuItem.Default("Notifications")
            .Children(
                MenuItem.Checkbox("Email").Checked().Tag("notify-email"),
                MenuItem.Checkbox("Push").Checked().Tag("notify-push"),
                MenuItem.Checkbox("SMS").Tag("notify-sms")
            ),
        MenuItem.Separator(),
        MenuItem.Default("Help & Support").Tag("help"),
        MenuItem.Default("About").Tag("about"),
        MenuItem.Separator(),
        MenuItem.Default("Logout").Tag("logout"))
        .Header(Text.Muted("Signed in as john.doe@company.com"))
        .Top()
        .Align(DropDownMenu.AlignOptions.End)
    | new DropDownMenu(@evt => client.Toast("Selected: " + @evt.Value), 
        new Button("Settings Menu"),
        MenuItem.Default("General").Tag("general"),
        MenuItem.Default("Appearance").Tag("appearance"),
        MenuItem.Default("Privacy").Tag("privacy"),
        MenuItem.Default("Security").Tag("security"),
        MenuItem.Separator(),
        MenuItem.Default("Updates").Tag("updates"),
        MenuItem.Default("Support").Tag("support"))
        .Header(Text.Muted("Application Settings"))
```

<WidgetDocs Type="Ivy.DropDownMenu" ExtensionTypes="Ivy.DropDownMenuExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/DropDownMenu.cs"/>