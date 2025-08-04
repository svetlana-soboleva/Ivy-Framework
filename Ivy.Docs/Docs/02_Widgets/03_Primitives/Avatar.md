# Avatar

`Avatar`s are graphical representations of users or entities. They display an image if available or fall back to initials or a placeholder when no image is provided.

```csharp demo-below
Layout.Horizontal()
    | new Avatar("Niels Bosma", "https://api.images.cat/150/150")            
    | new Avatar("Niels Bosma")
```

<WidgetDocs Type="Ivy.Avatar" ExtensionTypes="Ivy.AvatarExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Avatar.cs"/> 