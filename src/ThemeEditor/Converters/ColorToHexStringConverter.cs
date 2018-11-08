using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ThemeEditor.Converters
{
    public class ColorToHexStringConverter : IValueConverter
    {
        private static Regex s_hexRegex = new Regex("^#[a-fA-F0-9]{8}$");

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
                try
                {
                    if (s_hexRegex.Match(s).Success)
                    {
                        return Color.Parse(s);
                    }
                }
                catch(Exception)
                {
                    return AvaloniaProperty.UnsetValue;
                }
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}
