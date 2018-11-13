using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ThemeEditor.Controls.ColorPicker.Converters
{
    public class HueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double h && targetType == typeof(Color))
            {
                return ColorHelpers.FromHSVA(h, 100, 100, 100);
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
                        ColorHelpers.FromColor(ColorHelpers.FromHexColor(s), out double h, out _, out _, out _);
                        return h;
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
