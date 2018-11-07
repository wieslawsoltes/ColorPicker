using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ThemeEditor.Converters
{
    public class ColorToHexStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color && targetType == typeof(string))
            {
                return $"#{color.A.ToString("X2")}{color.R.ToString("X2")}{color.G.ToString("X2")}{color.B.ToString("X2")}";
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && targetType == typeof(Color))
            {
                return Color.Parse(s);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}
