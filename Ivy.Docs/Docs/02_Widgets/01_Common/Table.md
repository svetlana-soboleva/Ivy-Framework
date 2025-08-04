# Table

The `Table` widget is a layout container designed to render data in a tabular format. It accepts rows composed of `TableRow` elements, making it suitable for structured display of content like data listings, reports, or grids.

## Basic Usage

There is a recommended way to create a table with data:

```csharp demo-tabs
public class BasicRowTable : ViewBase
{
    public class Product
    {
        public required string Sku { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required string Url { get; set; }
    }
    
    public override object? Build()
    {
        var products = new[] {
            new {Sku = "1234", Name = "T-shirt", Price = 10.0, Url = "http://example.com/tshirt"},
            new {Sku = "1235", Name = "Jeans", Price = 20.0, Url = "http://example.com/jeans"},
            new {Sku = "1236", Name = "Sneakers", Price = 30.0, Url = "http://example.com/sneakers"},
        };
        
        return products.ToTable()
            .Width(Size.Full());
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
            )
        );
    }
}
```

## Data-Driven Tables

### Basic Data Table

```csharp demo-tabs
public class ProductTable : ViewBase
{
    public override object? Build()
    {
        var products = new[] {
            new {Sku = "1234", Name = "T-shirt", Price = 10.0},
            new {Sku = "1235", Name = "Jeans", Price = 20.0}
        };

        return products.ToTable()
            .Width(Size.Full())
            .Header(p => p.Price, "Unit Price")
            .Align(p => p.Price, Align.Right);
    }
}
```

## Custom Column Builders

```csharp demo-tabs
public class CustomBuilderTable : ViewBase
{
    public override object? Build()
    {
        var products = new[] {
            new {Sku = "1234", Name = "T-shirt", Price = 10.0, Url = "http://example.com/tshirt"},
            new {Sku = "1235", Name = "Jeans", Price = 20.0, Url = "http://example.com/jeans"}
        };

        return products.ToTable()
            .Width(Size.Full())
            .Width(p => p.Price, Size.Units(100))
            .Header(p => p.Price, "Unit Price")
            .Align(p => p.Price, Align.Right)
            .Order(p => p.Name, p => p.Price, p => p.Sku)
            .Remove(p => p.Url)
            .Totals(p => p.Price)
            .Empty(new Card("No products found").Width(Size.Full()));
    }
}
```

## Manual Table with Headers

```csharp demo-tabs
public class ManualTableDemo : ViewBase
{
    public override object? Build()
    {
        return new Table(
            new TableRow(
                new TableCell("Name").IsHeader().Align(Align.Left),
                new TableCell("Age").IsHeader().Align(Align.Center),
                new TableCell("Email").IsHeader().Align(Align.Left)
            ),
            new TableRow(
                new TableCell("Alice"),
                new TableCell("30").Align(Align.Center),
                new TableCell("alice@example.com")
            )
        );
    }
}
```

<WidgetDocs Type="Ivy.Table" ExtensionTypes="Ivy.Views.Tables.TableExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Tables/Table.cs"/>
