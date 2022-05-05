using Avalonia.Data.Converters;

namespace ThemeEditor.Controls.ColorPicker;

public interface IValueConverters
{
    IValueConverter Value1Converter { get; }
    IValueConverter Value2Converter { get; }
    IValueConverter Value3Converter { get; }
    IValueConverter Value4Converter { get; }
}
