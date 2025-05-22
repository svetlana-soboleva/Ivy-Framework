# Details

Detail widgets display label and value pairs. They are usually generated from a model using `ToDetails()`.

## Basic Usage

```csharp
data record User(string Name, string Email);
var user = new User("Niels", "niels@example.com");
return user.ToDetails();
```

Remove empty entries or customize fields via the builder:

```csharp
data.ToDetails().RemoveEmpty();
```

<WidgetDocs Type="Ivy.Details" ExtensionTypes="Ivy.Builders.DetailsBuilderExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Builders/DetailsBuilder.cs"/>
