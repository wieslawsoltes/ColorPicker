using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ThemeEditor.Controls.ColorPicker.Converters;

public class ValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double v && parameter is double range && targetType == typeof(double))
        {
            return range - (v * range / 100.0);
        }
        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double v && parameter is double range && targetType == typeof(double))
        {
            return 100.0 - (v * 100.0 / range);
        }
        return AvaloniaProperty.UnsetValue;
    }
}
