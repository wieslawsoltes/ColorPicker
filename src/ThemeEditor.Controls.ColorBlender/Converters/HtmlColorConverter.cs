using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

namespace ThemeEditor.Controls.ColorBlender.Converters
{
    public class HtmlColorConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Count() == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (values[i].GetType() != typeof(byte))
                    {
                        return AvaloniaProperty.UnsetValue;
                    }
                }
                return $"#FF{((byte)values[0]).ToString("X2")}{((byte)values[1]).ToString("X2")}{((byte)values[2]).ToString("X2")}";
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}
