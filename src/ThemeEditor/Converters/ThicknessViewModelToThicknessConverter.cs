using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

namespace ThemeEditor.Converters;

public class ThicknessViewModelToThicknessConverter : IMultiValueConverter
{
    public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values != null && values.Count() == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (values[i].GetType() != typeof(double))
                {
                    return AvaloniaProperty.UnsetValue;
                }
            }
            return new Thickness(
                (double)values[0],
                (double)values[1],
                (double)values[2],
                (double)values[3]);
        }
        return AvaloniaProperty.UnsetValue;
    }
}