using System.Text.RegularExpressions;
using Avalonia.Media;
using ThemeEditor.Controls.ColorPicker.Colors;

namespace ThemeEditor.Controls.ColorPicker;

public static class ColorPickerHelpers
{
    private static readonly Regex s_hexRegex = new Regex("^#[a-fA-F0-9]{8}$");

    public static bool IsValidHexColor(string hex)
    {
        return !string.IsNullOrWhiteSpace(hex) && s_hexRegex.Match(hex).Success;
    }

    public static string ToHexColor(Color color)
    {
        return $"#{color.ToUint32():X8}";
    }

    public static Color FromHexColor(string hex)
    {
        return Color.Parse(hex);
    }

    public static void FromColor(Color color, out double h, out double s, out double v, out double a)
    {
        var hsv = new RGB(color.R, color.G, color.B).ToHSV();
        h = hsv.H;
        s = hsv.S;
        v = hsv.V;
        a = color.A * 100.0 / 255.0;
    }

    // ReSharper disable once InconsistentNaming
    public static Color FromHSVA(double h, double s, double v, double a)
    {
        var rgb = new HSV(h, s, v).ToRGB();
        var A = (byte)(a * 255.0 / 100.0);
        return new Color(A, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
    }

    // ReSharper disable once InconsistentNaming
    public static Color FromRGBA(byte r, byte g, byte b, double a)
    {
        var A = (byte)(a * 255.0 / 100.0);
        return new Color(A, r, g, b);
    }
}
