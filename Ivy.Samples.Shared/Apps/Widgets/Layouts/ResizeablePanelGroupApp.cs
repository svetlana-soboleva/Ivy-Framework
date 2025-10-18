﻿using Ivy.Shared;

namespace Ivy.Samples.Shared.Apps.Widgets.Layouts;

[App(icon: Icons.LayoutPanelTop, searchHints: ["split", "resizable", "panels", "divider", "adjustable", "layout"])]
public class ResizeablePanelGroupApp : ViewBase
{
    public override object? Build()
    {
        return new ResizeablePanelGroup(
            new ResizeablePanel(25, "Left"),
            new ResizeablePanel(75,
                new ResizeablePanelGroup(
                    new ResizeablePanel(50, "Top"),
                    new ResizeablePanel(50, "Bottom")
            ).Vertical())
        ).Horizontal().Height(Size.Screen());
    }
}