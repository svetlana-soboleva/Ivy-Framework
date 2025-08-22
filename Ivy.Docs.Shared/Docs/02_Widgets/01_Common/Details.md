# Details

<Ingress>
Display structured label-value pairs from models with automatic formatting using the ToDetails() extension method.
</Ingress>

`Detail` widgets display label and value pairs. They are usually generated from a model using `ToDetails()`.

## Basic Usage

The simplest way to create details is by calling `ToDetails()` on any object:

```csharp demo-tabs
new { Name = "John Doe", Email = "john@example.com", Age = 30 }
    .ToDetails()
```

## Automatic Field Removal

Remove empty or null fields using the `RemoveEmpty()` method:

```csharp demo-tabs
new { FirstName = "John", LastName = "Doe", Age = 30, MiddleName = "" }
    .ToDetails()
    .RemoveEmpty()
```

## Custom Field Removal

Selectively remove specific fields using the `Remove()` method:

```csharp demo-tabs
new { Id = 123, Name = "John Doe", Email = "john@example.com" }
    .ToDetails()
    .Remove(u => u.Id)
```

## Multi-Line Fields

Mark specific fields as multi-line for better text display:

```csharp demo-tabs
new { Name = "Widget", Description = "Long description text" }
    .ToDetails()
    .MultiLine(p => p.Description)
```

## Nested Objects

Details automatically handle nested objects by converting them to their own detail views:

```csharp demo-tabs
new { Name = "John", Address = new { Street = "123 Main St", City = "Anytown" }
    .ToDetails() }
    .ToDetails()
```

## Working with State

Details work seamlessly with reactive state:

```csharp demo-tabs
UseState(() => new { Name = "John Doe", Age = 30 })
    .ToDetails()
```

<WidgetDocs Type="Ivy.Details" ExtensionTypes="Ivy.Builders.DetailsBuilderExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Builders/DetailsBuilder.cs"/>

## Examples

### Product Details

```csharp demo-tabs
new { Name = "Widget", Price = 99.99m, IsAvailable = true, Description = "High-quality widget with advanced features" }.ToDetails().MultiLine(p => p.Description).RemoveEmpty()
```

### User Profile

```csharp demo-tabs
new { Username = "johndoe", FullName = "John Doe", IsVerified = true, Email = "john@example.com" }.ToDetails().MultiLine(u => u.FullName).RemoveEmpty()
```
