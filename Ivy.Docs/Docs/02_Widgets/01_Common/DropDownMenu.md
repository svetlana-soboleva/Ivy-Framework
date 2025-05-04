# DropDownMenu

## Basic Usage

Here's a simple example of a DropDownMenu that shows a toast message when an item is selected:

```csharp
var client = this.UseService<IClientProvider>();
return new DropDownMenu(@evt =>
       {
           client.Toast(@evt.Value?.ToString() ?? throw new Exception("Missing value in event."));
       }, new Button("Open Menu")).Header(
            Layout.Vertical().Gap(0) | Text.Muted("Signed in as") | Text.Small("niels@bosmainteractive.se")
           )
           .Left()
            | MenuItem.Separator()
            | MenuItem.Default("Ivy Github").Icon(Icons.Github)
            | MenuItem.Default("Theme")
                .Icon(Icons.SunMoon)
                .Children(
                    MenuItem.Checkbox("Dark").Checked().Icon(Icons.Moon),
                    MenuItem.Checkbox("Light").Icon(Icons.Sun),
                    MenuItem.Checkbox("System").Icon(Icons.SunMoon)
                )
            | MenuItem.Default("Logout").Icon(Icons.LogOut);
```

## Variants

DropDownMenus can be customized with different menu items and icons to suit various use cases.

## Event Handling

DropDownMenus can handle selection events using the event parameter:

```csharp
var client = this.UseService<IClientProvider>();
return new DropDownMenu(@evt =>
{
    client.Toast(@evt.Value?.ToString() ?? "No selection made.");
}, new Button("Open Menu"));
```

## Styling

DropDownMenus can be styled with various options to fit the design of your application.

## Examples

### Advanced Usage

```csharp
return new DropDownMenu(@evt =>
{
    // Handle the event
}, new Button("Advanced Menu"))
    .Header(Layout.Vertical() | Text("Advanced Header"))
    .Left()
    | MenuItem.Default("Advanced Item").Icon(Icons.Advanced);
```

<WidgetDocs Type="Ivy.DropDownMenu" ExtensionTypes="Ivy.DropDownMenuExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/DropDownMenu.cs"/>