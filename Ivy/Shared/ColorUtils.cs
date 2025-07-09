using System.Text.RegularExpressions;

namespace Ivy.Shared;

public static class ColorUtils
{
    private static readonly Dictionary<Colors, string> ColorToHexMap = new()
    {
        { Colors.Black, "#000000" },
        { Colors.White, "#ffffff" },
        { Colors.Slate, "#6a7489" },
        { Colors.Gray, "#6e727f" },
        { Colors.Zinc, "#717179" },
        { Colors.Neutral, "#737373" },
        { Colors.Stone, "#76716d" },
        { Colors.Red, "#dd5860" },
        { Colors.Orange, "#dc824d" },
        { Colors.Amber, "#deb145" },
        { Colors.Yellow, "#e5e04c" },
        { Colors.Lime, "#afd953" },
        { Colors.Green, "#86d26f" },
        { Colors.Emerald, "#76cd94" },
        { Colors.Teal, "#5b9ba8" },
        { Colors.Cyan, "#4469c0" },
        { Colors.Sky, "#373bda" },
        { Colors.Blue, "#381ff4" },
        { Colors.Indigo, "#4b28e2" },
        { Colors.Violet, "#6637d1" },
        { Colors.Purple, "#844cc0" },
        { Colors.Fuchsia, "#a361af" },
        { Colors.Pink, "#c377a0" },
        { Colors.Rose, "#e48e91" },
        { Colors.Primary, "#381ff4" },
        { Colors.Secondary, "#6e727f" },
        { Colors.Destructive, "#dd5860" }
    };

    private static readonly Dictionary<string, Colors> HexToColorMap = ColorToHexMap
        .ToDictionary(kvp => kvp.Value.ToLowerInvariant(), kvp => kvp.Key);

    public static bool IsValidColor(string? colorString)
    {
        if (string.IsNullOrWhiteSpace(colorString))
            return false;

        return IsValidHexColor(colorString) || 
               IsValidRgbColor(colorString) || 
               IsValidOklchColor(colorString) ||
               IsValidColorsEnum(colorString);
    }

    public static bool IsValidHexColor(string colorString)
    {
        return Regex.IsMatch(colorString, @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
    }

    public static bool IsValidRgbColor(string colorString)
    {
        return Regex.IsMatch(colorString, @"^rgb\(\s*\d+\s*,\s*\d+\s*,\s*\d+\s*\)$");
    }

    public static bool IsValidOklchColor(string colorString)
    {
        return Regex.IsMatch(colorString, @"^oklch\(\s*[\d.]+\s+[\d.]+\s+[\d.]+\s*\)$");
    }

    public static bool IsValidColorsEnum(string colorString)
    {
        return Enum.TryParse<Colors>(colorString, true, out _);
    }

    public static string? ConvertToHex(string? colorString)
    {
        if (string.IsNullOrWhiteSpace(colorString))
            return null;

        // If it's already a valid hex color, return as is
        if (IsValidHexColor(colorString))
            return colorString.ToLowerInvariant();

        // If it's a Colors enum, convert to hex
        if (Enum.TryParse<Colors>(colorString, true, out var colorEnum))
        {
            return ColorToHexMap.TryGetValue(colorEnum, out var hex) ? hex : null;
        }

        // For now, return the original string for rgb and oklch
        // In a real implementation, you would convert these to hex
        return colorString;
    }

    public static Colors? ConvertToColorsEnum(string? colorString)
    {
        if (string.IsNullOrWhiteSpace(colorString))
            return null;

        // If it's already a Colors enum, return as is
        if (Enum.TryParse<Colors>(colorString, true, out var colorEnum))
            return colorEnum;

        // If it's a hex color, try to find matching enum
        var hexColor = ConvertToHex(colorString);
        if (hexColor != null && HexToColorMap.TryGetValue(hexColor, out var enumColor))
            return enumColor;

        return null;
    }

    public static string? ConvertToRgb(string? colorString)
    {
        if (string.IsNullOrWhiteSpace(colorString))
            return null;

        // If it's already a valid rgb color, return as is
        if (IsValidRgbColor(colorString))
            return colorString;

        // For now, return the original string
        // In a real implementation, you would convert hex/oklch to rgb
        return colorString;
    }

    public static string? ConvertToOklch(string? colorString)
    {
        if (string.IsNullOrWhiteSpace(colorString))
            return null;

        // If it's already a valid oklch color, return as is
        if (IsValidOklchColor(colorString))
            return colorString;

        // For now, return the original string
        // In a real implementation, you would convert hex/rgb to oklch
        return colorString;
    }

    public static string GetDisplayValue(string? colorString)
    {
        if (string.IsNullOrWhiteSpace(colorString))
            return string.Empty;

        // If it's a Colors enum, return the enum name
        if (Enum.TryParse<Colors>(colorString, true, out var colorEnum))
            return colorEnum.ToString();

        // Otherwise return the original string
        return colorString;
    }
} 