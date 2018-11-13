using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ThemeEditor.Colors;

namespace ThemeEditor.Controls.ColorPicker.Converters
{
    public class HueToColorConverter : IValueConverter
    {
        private static Regex s_hexRegex = new Regex("^#[a-fA-F0-9]{8}$");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double h && targetType == typeof(Color))
            {
                var rgb = new HSV(h, 100, 100).ToRGB();
                return new Color(0xFF, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && targetType == typeof(double))
            {
                try
                {
                    if (s_hexRegex.Match(s).Success)
                    {
                        var c = Color.Parse(s);
                        HSV hsv = new RGB(c.R, c.G, c.B).ToHSV();
                        return hsv.H;
                    }
                }
                catch (Exception)
                {
                    return AvaloniaProperty.UnsetValue;
                }
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}
