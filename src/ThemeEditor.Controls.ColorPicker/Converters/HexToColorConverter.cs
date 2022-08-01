using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ThemeEditor.Controls.ColorPicker.Converters;

public class HexToColorConverter : IValueConverter
{
    public static readonly HexToColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string s && targetType == typeof(Color))
        {
            try
            {
                if (ColorPickerHelpers.IsValidHexColor(s))
                {
                    return ColorPickerHelpers.FromHexColor(s);
                }
            }
            catch (Exception)
            {
                return AvaloniaProperty.UnsetValue;
            }
        }
        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color c && targetType == typeof(string))
        {
            try
            {
                return ColorPickerHelpers.ToHexColor(c);
            }
            catch (Exception)
            {
                return AvaloniaProperty.UnsetValue;
            }
        }
        return AvaloniaProperty.UnsetValue;
    }
}
