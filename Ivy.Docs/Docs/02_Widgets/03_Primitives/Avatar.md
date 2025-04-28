# Avatar

Avatars are graphical representations of users or entities. They display an image if available or fall back to initials or a placeholder when no image is provided.

```csharp demo-below
Layout.Horizontal()
    | new Avatar("Niels Bosma", "https://api.images.cat/150/150")            
    | new Avatar("Renco Smeding")
```

<WidgetDocs Type="Ivy.Avatar" ExtensionsType="Ivy.AvatarExtensions"/> 