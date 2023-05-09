﻿using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ColorPickerDemo;

public class HexToColorConverter : IValueConverter
{
    public static HexToColorConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string s && (targetType == typeof(Color?) || targetType == typeof(Color)))
        {
            try
            {
                if (Color.TryParse(s, out var color))
                {
                    return color;
                }
            }
            catch (Exception)
            {
                return AvaloniaProperty.UnsetValue;
            }
        }
        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color c && targetType == typeof(string))
        {
            try
            {
                return $"#{c.ToUInt32():x8}";
            }
            catch (Exception)
            {
                return AvaloniaProperty.UnsetValue;
            }
        }
        return AvaloniaProperty.UnsetValue;
    }
}
