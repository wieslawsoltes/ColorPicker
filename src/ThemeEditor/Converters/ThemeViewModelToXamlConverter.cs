using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using ThemeEditor.ViewModels;

namespace ThemeEditor.Converters
{
    public class ThemeViewModelToXamlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ThemeViewModel theme && targetType == typeof(string))
            {
                return theme.ToXaml();
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
