using System;
using System.Globalization;
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
            if (value is double hue && targetType == typeof(Color))
            {
                var rgb = new HSV(hue, 100, 100).ToRGB();
                var color = new Color(255, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
                return color;
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
