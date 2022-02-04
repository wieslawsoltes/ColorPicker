using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

namespace ThemeEditor.Converters;

public class ThicknessViewModelToThicknessConverter : IMultiValueConverter
{
    public object Convert(IList<object?>? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values != null && values.Count() == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (values[i]?.GetType() != typeof(double))
                {
                    return AvaloniaProperty.UnsetValue;
                }
            }
            
            var l = (double?)values[0];
            var t = (double?)values[1];
            var r = (double?)values[2];
            var b = (double?)values[3];
            if (l is { } && t is { } && r is { } && b is { })
            {
                return new Thickness(l.Value, t.Value, r.Value, b.Value);
            }
        }
        return AvaloniaProperty.UnsetValue;
    }
}
