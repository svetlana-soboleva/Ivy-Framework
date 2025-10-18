using Ivy.Shared;

namespace Ivy.Samples.Shared.Apps.Widgets.Primitives;

[App(icon: Icons.CodeXml, path: ["Widgets", "Primitives"], searchHints: ["vector", "graphics", "svg", "image", "scalable", "illustration"])]
public class SvgApp : SampleBase
{
    protected override object? BuildSample()
    {
        var content =
            """
            <svg width="100" height="100">
                <circle cx="50" cy="50" r="40" stroke="black" stroke-width="3" fill="red" />
            </svg>
            """;

        return new Svg(content);
    }
}