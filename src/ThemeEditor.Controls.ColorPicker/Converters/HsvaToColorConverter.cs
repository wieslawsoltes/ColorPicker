using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ThemeEditor.Colors;

namespace ThemeEditor.Controls.ColorPicker.Converters
{
    public class HsvaToColorConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            double[] v = values.OfType<double>().ToArray();
            if (v.Length == values.Count)
            {
                RGB rgb = new HSV(v[0], v[1], v[2]).ToRGB();
                byte A = (byte)((v[3] * 255.0) / 100.0);
                return new Color(A, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}
