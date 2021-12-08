using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Media;
using ThemeEditor.ViewModels;

namespace ThemeEditor;

public static class Extensions
{
    private static string Invariant(double value)
    {
        return FormattableString.Invariant($"{value}");
    }

    public static IBrush ToBursh(this Color color)
    {
        return new SolidColorBrush(color);
    }

    public static Color ToColor(this IColor color)
    {
        switch (color)
        {
            case RgbColorViewModel rgb:
                return new Color(255, rgb.R, rgb.G, rgb.B);
            case ArgbColorViewModel argb:
                return new Color(argb.A, argb.R, argb.G, argb.B);
            default:
                throw new NotSupportedException($"Not supported color type {color.GetType()}.");
        }
    }

    public static RgbColorViewModel RgbFromColor(this Color color)
    {
        return new RgbColorViewModel()
        {
            R = color.R,
            G = color.G,
            B = color.B
        };
    }

    public static ArgbColorViewModel ArgbFromColor(this Color color)
    {
        return new ArgbColorViewModel()
        {
            A = color.A,
            R = color.R,
            G = color.G,
            B = color.B
        };
    }

    public static IBrush ToBrush(this IColor color)
    {
        switch (color)
        {
            case RgbColorViewModel rgb:
                return new SolidColorBrush(ToColor(color));
            case ArgbColorViewModel argb:
                return new SolidColorBrush(ToColor(color));
            default:
                throw new NotSupportedException($"Not supported color type {color.GetType()}.");
        }
    }

    public static void FromTextString(this string value, out double left, out double top, out double right, out double bottom)
    {
        var thickness = Thickness.Parse(value);
        left = thickness.Left;
        top = thickness.Top;
        right = thickness.Right;
        bottom = thickness.Bottom;
    }

    public static string ToTextString(this ThicknessViewModel thickness)
    {
        var t = thickness.ToThickness();
        if (t.IsUniform)
            return Invariant(t.Left);
        else if (t.Left.Equals(t.Right) && t.Top.Equals(t.Bottom))
            return $"{Invariant(t.Left)} {Invariant(t.Top)}";
        return $"{Invariant(t.Left)},{Invariant(t.Top)},{Invariant(t.Right)},{Invariant(t.Bottom)}";
    }

    public static RgbColorViewModel RgbFromHexString(this string value)
    {
        return Color.Parse(value).RgbFromColor();
    }

    public static ArgbColorViewModel ArgbFromHexString(this string value)
    {
        return Color.Parse(value).ArgbFromColor();
    }

    public static void FromHexString(this string value, out byte a, out byte r, out byte g, out byte b)
    {
        var color = Color.Parse(value);
        a = color.A;
        r = color.R;
        g = color.G;
        b = color.B;
    }

    public static uint ToUint32(this RgbColorViewModel color)
    {
        return ((uint)255 << 24) | ((uint)color.R << 16) | ((uint)color.G << 8) | (uint)color.B;
    }

    public static uint ToUint32(this ArgbColorViewModel color)
    {
        return ((uint)color.A << 24) | ((uint)color.R << 16) | ((uint)color.G << 8) | (uint)color.B;
    }

    public static string ToHexString(this RgbColorViewModel color)
    {
        return $"#{color.ToUint32():X8}";
    }

    public static string ToHexString(this ArgbColorViewModel color)
    {
        return $"#{color.ToUint32():X8}";
    }

    public static string ToHexString3(this IList<object> values)
    {
        uint argb = ((uint)255 << 24) | ((uint)(byte)values[0] << 16) | ((uint)(byte)values[1] << 8) | (uint)(byte)values[2];
        return $"#{argb:X8}";
    }

    public static string ToHexString4(this IList<object> values)
    {
        uint argb = ((uint)(byte)values[0] << 24) | ((uint)(byte)values[1] << 16) | ((uint)(byte)values[2] << 8) | (uint)(byte)values[3];
        return $"#{argb:X8}";
    }

    public static string ToHexString(this IColor color)
    {
        switch (color)
        {
            case RgbColorViewModel rgb:
                return $"#{rgb.ToUint32():X8}";
            case ArgbColorViewModel argb:
                return $"#{argb.ToUint32():X8}";
            default:
                throw new NotSupportedException($"Not supported color type {color.GetType()}.");
        }
    }

    public static Thickness ToThickness(this ThicknessViewModel thickness)
    {
        return new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
    }

    public static ThicknessViewModel FromThickness(this Thickness thickness)
    {
        return new ThicknessViewModel()
        {
            Left = thickness.Left,
            Top = thickness.Top,
            Right = thickness.Right,
            Bottom = thickness.Bottom
        };
    }

    public static string ToXaml(this ThemeViewModel theme)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<Style xmlns=\"https://github.com/avaloniaui\"");
        sb.AppendLine("       xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
        sb.AppendLine("       xmlns:sys=\"using:System;assembly=mscorlib\">");
        sb.AppendLine("    <Style.Resources>");
        sb.AppendLine("");
        sb.AppendLine($"        <Color x:Key=\"ThemeAccentColor\">{theme.ThemeAccentColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeAccentColor2\">{theme.ThemeAccentColor2?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeAccentColor3\">{theme.ThemeAccentColor3?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeAccentColor4\">{theme.ThemeAccentColor4?.ToHexString()}</Color>");
        sb.AppendLine("");
        sb.AppendLine($"        <Color x:Key=\"ThemeBackgroundColor\">{theme.ThemeBackgroundColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeBorderLowColor\">{theme.ThemeBorderLowColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeBorderMidColor\">{theme.ThemeBorderMidColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeBorderHighColor\">{theme.ThemeBorderHighColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeControlLowColor\">{theme.ThemeControlLowColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeControlMidColor\">{theme.ThemeControlMidColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeControlHighColor\">{theme.ThemeControlHighColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeControlHighlightLowColor\">{theme.ThemeControlHighlightLowColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeControlHighlightMidColor\">{theme.ThemeControlHighlightMidColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeControlHighlightHighColor\">{theme.ThemeControlHighlightHighColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeForegroundColor\">{theme.ThemeForegroundColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ThemeForegroundLowColor\">{theme.ThemeForegroundLowColor?.ToHexString()}</Color>");
        sb.AppendLine("");
        sb.AppendLine($"        <Color x:Key=\"HighlightColor\">{theme.HighlightColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ErrorColor\">{theme.ErrorColor?.ToHexString()}</Color>");
        sb.AppendLine($"        <Color x:Key=\"ErrorLowColor\">{theme.ErrorLowColor?.ToHexString()}</Color>");
        sb.AppendLine("");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeBackgroundBrush\" Color=\"{DynamicResource ThemeBackgroundColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeBorderLowBrush\" Color=\"{DynamicResource ThemeBorderLowColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeBorderMidBrush\" Color=\"{DynamicResource ThemeBorderMidColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeBorderHighBrush\" Color=\"{DynamicResource ThemeBorderHighColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeControlLowBrush\" Color=\"{DynamicResource ThemeControlLowColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeControlMidBrush\" Color=\"{DynamicResource ThemeControlMidColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeControlHighBrush\" Color=\"{DynamicResource ThemeControlHighColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeControlHighlightLowBrush\" Color=\"{DynamicResource ThemeControlHighlightLowColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeControlHighlightMidBrush\" Color=\"{DynamicResource ThemeControlHighlightMidColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeControlHighlightHighBrush\" Color=\"{DynamicResource ThemeControlHighlightHighColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeForegroundBrush\" Color=\"{DynamicResource ThemeForegroundColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeForegroundLowBrush\" Color=\"{DynamicResource ThemeForegroundLowColor}\"></SolidColorBrush>");
        sb.AppendLine("");
        sb.AppendLine("        <SolidColorBrush x:Key=\"HighlightBrush\" Color=\"{DynamicResource HighlightColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeAccentBrush\" Color=\"{DynamicResource ThemeAccentColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeAccentBrush2\" Color=\"{DynamicResource ThemeAccentColor2}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeAccentBrush3\" Color=\"{DynamicResource ThemeAccentColor3}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ThemeAccentBrush4\" Color=\"{DynamicResource ThemeAccentColor4}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ErrorBrush\" Color=\"{DynamicResource ErrorColor}\"></SolidColorBrush>");
        sb.AppendLine("        <SolidColorBrush x:Key=\"ErrorLowBrush\" Color=\"{DynamicResource ErrorLowColor}\"></SolidColorBrush>");
        sb.AppendLine("");
        sb.AppendLine($"        <Thickness x:Key=\"ThemeBorderThickness\">{theme.ThemeBorderThickness?.ToTextString()}</Thickness>");
        sb.AppendLine($"        <sys:Double x:Key=\"ThemeDisabledOpacity\">{Invariant(theme.ThemeDisabledOpacity)}</sys:Double>");
        sb.AppendLine("");
        sb.AppendLine($"        <sys:Double x:Key=\"FontSizeSmall\">{Invariant(theme.FontSizeSmall)}</sys:Double>");
        sb.AppendLine($"        <sys:Double x:Key=\"FontSizeNormal\">{Invariant(theme.FontSizeNormal)}</sys:Double>");
        sb.AppendLine($"        <sys:Double x:Key=\"FontSizeLarge\">{Invariant(theme.FontSizeLarge)}</sys:Double>");
        sb.AppendLine("");
        sb.AppendLine($"        <sys:Double x:Key=\"ScrollBarThickness\">{Invariant(theme.ScrollBarThickness)}</sys:Double>");
        sb.AppendLine("    </Style.Resources>");
        sb.AppendLine("</Style>");

        return sb.ToString();
    }
}