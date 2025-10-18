﻿---
searchHints:
  - icons
  - lucide
  - symbols
  - graphics
  - glyphs
  - svg
---

# Icons

Ivy use the [Lucide](https://lucide.dev/icons/) icon library.

```csharp demo-tabs
public class SearchIconsView : ViewBase
{
    public override object? Build()
    {
        var client = this.UseService<IClientProvider>();
        var searchState = this.UseState("code");
        var iconsState = this.UseState<Icons[]>(Array.Empty<Icons>());
        
        UseEffect(() =>
        {
            var allIcons = Enum.GetValues<Icons>().Where(e => e != Icons.None);
            iconsState.Set(string.IsNullOrEmpty(searchState.Value)
                ? []
                : allIcons.Where(e => e.ToString().Contains(searchState.Value, StringComparison.OrdinalIgnoreCase)).Take(10).ToArray());
        }, [ EffectTrigger.AfterInit(), searchState.Throttle(TimeSpan.FromMilliseconds(500)).ToTrigger() ]);
        
        var searchInput = searchState.ToSearchInput().Placeholder("Type a icon name");
        
        var icons  = iconsState.Value.Select(e => Layout.Horizontal().Gap(2)
            | new Button(null, @event =>
            {
                var iconCode = "Icons." + e.ToString();
                client.CopyToClipboard(iconCode);
                client.Toast($"Copied '{iconCode}' to clipboard", "Icon Code Copied");
            }, ButtonVariant.Ghost, e).Small().WithTooltip($"Click to copy {e.ToString()}")
            | Text.Label("Icons." + e.ToString())
            );

        return Layout.Vertical()
               | searchInput
               | icons
            ; 
    }
}
```
