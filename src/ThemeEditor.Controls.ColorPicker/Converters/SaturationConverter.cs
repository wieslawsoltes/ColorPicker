using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ThemeEditor.Controls.ColorPicker.Converters;

public class SaturationConverter : IValueConverter
{
    public static readonly SaturationConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double v && parameter is double range && (targetType == typeof(double) || targetType == typeof(double?)))
        {
            return v * range / 100.0;
        }
        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double v && parameter is double range && targetType == typeof(double))
        {
            return v * 100.0 / range;
        }
        return AvaloniaProperty.UnsetValue;
    }
}
