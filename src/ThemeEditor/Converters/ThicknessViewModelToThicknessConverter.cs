using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using ThemeEditor.ViewModels;

namespace ThemeEditor.Converters
{
    public class ThicknessViewModelToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ThicknessViewModel thickness && targetType == typeof(Thickness))
            {
                return thickness.ToThickness();
            }
            return AvaloniaProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
