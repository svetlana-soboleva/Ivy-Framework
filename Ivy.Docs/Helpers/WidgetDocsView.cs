using System.Reflection;
using Ivy.Tables;
using Newtonsoft.Json;

namespace Ivy.Docs.Helpers;

public class WidgetDocsView(string typeName, string? extensionsTypeName) : ViewBase
{
    public override object? Build()
    {
        var type = TypeUtils.GetTypeFromName(typeName);
        var extensionsType = TypeUtils.GetTypeFromName(extensionsTypeName);
        
        if(type == null) return Text.Danger($"WidgetDocsView:Unable to find type {typeName}.");
        
        object? defaultValueProvider = TypeUtils.TryToActivate(type);
        
        var properties = type.GetProperties()
            .Where(p => p.GetCustomAttribute<PropAttribute>() != null)
            .Select(e => TypeUtils.GetPropRecord(e, defaultValueProvider, type, extensionsType))
            .OrderBy(e => e.Name);

        var events = type.GetProperties()
            .Where(p => p.GetCustomAttribute<EventAttribute>() != null)
            .Select(e => TypeUtils.GetEventRecord(e, type, extensionsType))
            .OrderBy(e => e.Name)
            .ToList();

        return Layout.Vertical().Gap(8)
               | Text.H2("API")
               | Text.H3("Properties")
               | properties.ToTable()
               | (events.Any() ? Text.H3("Events") : null)
               | events.ToTable()
            ;
    }
}