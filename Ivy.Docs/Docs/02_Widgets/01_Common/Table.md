# Table

The `Table` widget is a layout container designed to render data in a tabular format. It accepts rows composed of `TableRow` elements, making it suitable for structured display of content like data listings, reports, or grids.

## Basic Usage

A simple way to create a table with a type of data.

```csharp
new TableBuilder<Person>(people);
```

```csharp demo-tabs
public class BasicRowTable : ViewBase
{
    public override object? Build()
    {
        public class Product
{
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public required double Price { get; set; }
    public required string Url { get; set; }
}

        var products = new[] {
            new {Sku = "1234", Name = "T-shirt", Price = 10.0, Url = "http://example.com/tshirt"},
            new {Sku = "1235", Name = "Jeans", Price = 20.0, Url = "http://example.com/jeans"},
            new {Sku = "1236", Name = "Sneakers", Price = 30.0, Url = "http://example.com/sneakers"},
            new {Sku = "1237", Name = "Hat", Price = 5.0, Url = "http://example.com/hat"},
            new {Sku = "1238", Name = "Socks", Price = 2.0, Url = "http://example.com/socks"}
        };

        return products.ToTable()
                .RemoveHeader()
                .Width(Size.Full())
    }
}
```

Here's a basic example of creating a `Table` using rows and cells:

```csharp demo-below
public class BasicTableDemo : ViewBase
{
    public override object? Build()
    {
        return new Table(
            new TableRow(
                new TableCell("Name"),
                new TableCell("Age")
            ),
            new TableRow(
                new TableCell("Alice"),
                new TableCell("30")
            ),
            new TableRow(
                new TableCell("Bob"),
                new TableCell("25")
            )
        );
    }
}
```

# TableRow

The `TableRow` widget represents a single row within a `Table`. It contains one or more `TableCell` elements and supports features such as marking the row as a header and composable syntax with the `|` operator.

Use `TableRow` to define a standard row of cells inside a `Table`.

```csharp demo-below
public class BasicRowTable : ViewBase
{
    public override object? Build()
    {
        return new Table(
            new TableRow(
                new TableCell("Name"),
                new TableCell("Age")
            ),
            new TableRow(
                new TableCell("Alice"),
                new TableCell("30")
            )
        );
    }
}
```

<WidgetDocs Type="Ivy.Table" ExtensionTypes="Ivy.Views.Tables.TableExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Tables/Table.cs"/>
