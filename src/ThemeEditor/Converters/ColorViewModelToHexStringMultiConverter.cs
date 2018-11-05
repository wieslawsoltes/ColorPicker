using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

namespace ThemeEditor.Converters
{
    public class ColorViewModelToHexStringMultiConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Count() == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (values[i].GetType() != typeof(byte))
                    {
                        return AvaloniaProperty.UnsetValue;
                    }
                }
                return values.ToHexString4();
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}
