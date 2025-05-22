# WrapLayout

WrapLayout arranges children in rows that wrap when they reach the end of the
available space.

```csharp
new WrapLayout([
    Text.Literal("One"),
    Text.Literal("Two"),
    Text.Literal("Three")
]).Gap(2).Padding(new Thickness(4));
```
