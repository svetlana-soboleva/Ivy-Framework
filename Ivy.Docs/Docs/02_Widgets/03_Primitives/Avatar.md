# Avatar

`Avatar`s are graphical representations of users or entities. They display an image if available or fall back to initials or a placeholder when no image is provided.

```csharp demo-below
Layout.Horizontal()
    | new Avatar("Niels Bosma", "https://api.images.cat/150/150")            
    | new Avatar("Niels Bosma")
```


<WidgetDocs Type="Ivy.Avatar" ExtensionTypes="Ivy.AvatarExtensions" SourceUrl="https://github.com/Ivy-Interactive/Ivy-Framework/blob/main/Ivy/Widgets/Primitives/Avatar.cs"/>

## Practical Usage

`Avatar`s can be used to showcase Teams like this. 

```csharp demo-tabs
public class IvyTeamDemo : ViewBase
{
    public override object? Build()
    {
        var team = new Dictionary<string,string>()
        {
             {"Neils Bosma", 
                 "https://ca.slack-edge.com/T08GFP8DV5F-U08GFP8DV6V-c4f29d91e630-72"},
             {"Mikael Rinne", 
                 "https://ca.slack-edge.com/T08GFP8DV5F-U091ZQY4LLS-0b7df2f0bdc1-72"},
             {"Renco Smeding", 
                 "https://ca.slack-edge.com/T08GFP8DV5F-U08GMFBS8CW-19c489f08a2e-192"},
             {"Jesper", 
                 "https://ca.slack-edge.com/T08GFP8DV5F-U0970GDECAH-c3871d963505-192"},
             {"Frida Bosma", 
                 "https://ca.slack-edge.com/T08GFP8DV5F-U08MV9BQG3W-625070267114-192"},
             {"Viktor Bolin", 
                 "https://ca.slack-edge.com/T08GFP8DV5F-U08P9HK52LV-608a7c8601d4-192"},
        };
        
        var layout = Layout.Grid()
                           .Columns(3)
                           .Rows(2);
                          
                           
        foreach(var key in team.Keys)
        {
            layout = layout 
                      |new Card(new Avatar(key, team[key]).Height(200).Width(100))
                            .Title(key);
        }
        return Layout.Vertical()
                      | H3("Ivy Team")
                      | layout;
    }    
}
```

### Food Menu

```csharp demo-tabs
public class AvatarAsFoodIcon : ViewBase
{
    public override object? Build()
    {
        return Layout.Vertical()
                | 
                new Card(
                new Avatar("Köttbullar","https://www.campervansweden.com/assets/img/blog/461/traditional-swedish-dishes-kotbullar.jpg"),
                new Button("Add to order")).Title("Köttbullar")
                    .Description("The quintessential Swedish food, these meatballs are more than just a dish; they're a cultural icon.")
                    .Width(Size.Units(100))
                | 
                new Card(
                new Avatar("Pytt i Panna","https://www.campervansweden.com/assets/img/blog/461/traditional-swedish-dishes-pytt-panna.jpg"),
                new Button("Add to order")).Title("Pytt i Panna")
                    .Description("Translating to small pieces in a pan, this hearty hash of potatoes, onions, and meat is a beloved comfort food. It's a brilliant way to use leftovers and is often crowned with a fried egg.")
                    .Width(Size.Units(100));
    }    
}

```