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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double h && targetType == typeof(Color))
            {
                return ColorHelpers.ToColor(h, 100, 100, 100);
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && targetType == typeof(double))
            {
                try
                {
                    if (ColorHelpers.IsValidHexColor(s))
                    {
                        var c = Color.Parse(s);
                        return new RGB(c.R, c.G, c.B).ToHSV().H;
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
