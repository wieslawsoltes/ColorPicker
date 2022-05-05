using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace ThemeEditor.Controls.ColorPicker;

public class ColorPickerPropertiesPresenter : TemplatedControl
{
    public static readonly StyledProperty<IEnumerable<ColorPickerProperties>> PropertiesProperty = 
        AvaloniaProperty.Register<ColorPicker, IEnumerable<ColorPickerProperties>>(nameof(Properties));

    public IEnumerable<ColorPickerProperties> Properties
    {
        get => GetValue(PropertiesProperty);
        set => SetValue(PropertiesProperty, value);
    }
}
