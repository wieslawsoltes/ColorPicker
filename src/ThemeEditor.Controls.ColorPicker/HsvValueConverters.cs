using Avalonia.Data.Converters;
using ThemeEditor.Controls.ColorPicker.Converters;

namespace ThemeEditor.Controls.ColorPicker;

public class HsvValueConverters : IValueConverters
{
    public IValueConverter Value1Converter { get; } = new HueConverter();

    public IValueConverter Value2Converter { get; } = new SaturationConverter();

    public IValueConverter Value3Converter { get; } = new ValueConverter();

    public IValueConverter Value4Converter { get; } = new AlphaConverter();
}
