using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ThemeEditor.Converters;

public class ColorViewModelToBrushConverter : IMultiValueConverter
{
    public static readonly ColorViewModelToBrushConverter Instance = new();

    public object Convert(IList<object?>? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values != null && values.Count() == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (values[i]?.GetType() != typeof(byte))
                {
                    return AvaloniaProperty.UnsetValue;
                }
            }

            var a = (byte?)values[0];
            var r = (byte?)values[1];
            var g = (byte?)values[2];
            var b = (byte?)values[3];
            if (a is { } && r is { } && g is { } && b is { })
            {
                var color = Color.FromArgb(a.Value, r.Value, g.Value , b.Value);

                return new SolidColorBrush(color);
            }
        }
        return AvaloniaProperty.UnsetValue;
    }
}
