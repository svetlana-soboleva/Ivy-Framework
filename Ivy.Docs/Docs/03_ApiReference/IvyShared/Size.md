# Size

`Size` represents width or height values used throughout the framework. You can
create sizes in pixels, rems, fractions or special values such as `Full` or
`Auto`.

```csharp
Layout.Horizontal()
    | new Box().Width(Size.Px(100)).Height(Size.Rem(4))
    | new Box().Width(Size.Fraction(1/2f)).Height(Size.Auto());
```
