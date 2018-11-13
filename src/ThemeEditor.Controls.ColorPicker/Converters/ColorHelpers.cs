using System.Text.RegularExpressions;
using Avalonia.Media;
using ThemeEditor.Colors;

namespace ThemeEditor.Controls.ColorPicker.Converters
{
    public static class ColorHelpers
    {
        private static Regex s_hexRegex = new Regex("^#[a-fA-F0-9]{8}$");

        public static bool IsValidHexColor(string hex)
        {
            return s_hexRegex.Match(hex).Success;
        }

        public static string ToHexColor(Color color)
        {
            return $"#{color.ToUint32():X8}";
        }

        public static void FromColor(Color color, out double h, out double s, out double v, out double a)
        {
            HSV hsv = new RGB(color.R, color.G, color.B).ToHSV();
            h = hsv.H;
            s = hsv.S;
            v = hsv.V;
            a = color.A * 100.0 / 255.0;
        }

        public static Color ToColor(double h, double s, double v, double a)
        {
            RGB rgb = new HSV(h, s, v).ToRGB();
            byte A = (byte)(a * 255.0 / 100.0);
            return new Color(A, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
        }
    }
}
