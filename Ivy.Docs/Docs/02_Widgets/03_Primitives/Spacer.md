# Spacer

The Spacer widget creates empty space between elements in your layout. It's useful for fine-tuning spacing and alignment without adding additional markup or CSS.

```csharp demo-tabs
public class HeaderWithSpacerView : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical().Gap(4)
            | Text.H1("Using Spacers for Layout Control")
            
            // Basic horizontal spacer example
            | Text.H2("Navigation Bar")
            | Layout.Horizontal()
                | new Button("Home").Variant(ButtonVariant.Ghost)
                | new Button("Products").Variant(ButtonVariant.Ghost)
                | new Button("Services").Variant(ButtonVariant.Ghost)
                | new Spacer() // Push remaining items to the right
                | new Button("Login").Variant(ButtonVariant.Outline)
                | new Button("Sign Up")
            
            // Vertical spacer example
            | Text.H2("Card with Controlled Spacing")
            | Layout.Vertical().P(4).Width(Size.Units(300))
                | Text.H3("Product Name")
                | Text.P("This is a short description of the product that tells you about its features.")
                | new Spacer().Height(Size.Units(16)) // Add specific amount of space
                | Text.H4("$19.99")
                | new Button("Add to Cart").Width(Size.Full())
            
            // Multiple spacers for alignment
            | Text.H2("Footer With Multiple Sections")
            | Layout.Horizontal()
                | Layout.Vertical()
                    | Text.Strong("Company")
                    | Text.Small("About Us")
                    | Text.Small("Careers")
                    | Text.Small("Contact")
                | new Spacer().Width(Size.Units(24))
                | Layout.Vertical()
                    | Text.Strong("Products")
                    | Text.Small("Features")
                    | Text.Small("Pricing")
                    | Text.Small("Documentation")
                | new Spacer() // Flexible space
                | Layout.Vertical()
                    | Text.Strong("Connect")
                    | Text.Small("Twitter")
                    | Text.Small("Facebook")
                    | Text.Small("LinkedIn");
    }
}
```

<WidgetDocs Type="Ivy.Spacer" ExtensionsType="Ivy.SpacerExtensions"/> 