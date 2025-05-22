# Table

Tables organize data into rows and columns. You can build them manually or generate them from a collection using `ToTable()`.

## Basic Usage

```csharp
new Table(
    new TableRow(new TableCell("Name"), new TableCell("Age")).IsHeader(),
    new TableRow(new TableCell("Niels"), new TableCell("25")),
    new TableRow(new TableCell("John"), new TableCell("30"))
)
```

High level builders can create tables from records:

```csharp
data.ToTable();
```

<WidgetDocs Type="Ivy.Table" ExtensionTypes="Ivy.Tables.TableExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Tables/Table.cs"/>
