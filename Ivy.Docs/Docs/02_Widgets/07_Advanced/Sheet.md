# Sheet

`Sheets` slide in from the side of the screen and display additional content while
allowing the user to dismiss them. The `WithSheet` extension on a `Button`
provides an easy way to open a sheet.

```csharp
new Button("Open Sheet").WithSheet(
    () => new SheetView(),
    title: "This is a sheet",
    description: "Lorem ipsum dolor sit amet",
    width: Size.Fraction(1/2f)
);
```

<WidgetDocs Type="Ivy.Sheet" ExtensionTypes="Ivy.SheetExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Sheet.cs"/>
