using System;
using Avalonia;

namespace ThemeEditor.Controls.ColorPicker;

public abstract class ColorPickerProperties : AvaloniaObject
{
    public static readonly StyledProperty<ColorPicker?> ColorPickerProperty =
        AvaloniaProperty.Register<ColorPickerProperties, ColorPicker?>(nameof(ColorPicker));

    public ColorPickerProperties()
    {
        this.GetObservable(ColorPickerProperty).Subscribe(_ => OnColorPickerChange());
    }

    public ColorPicker? ColorPicker
    {
        get { return GetValue(ColorPickerProperty); }
        set { SetValue(ColorPickerProperty, value); }
    }

    public abstract void UpdateColorPickerValues();

    public abstract void UpdatePropertyValues();

    public virtual void OnColorPickerChange()
    {
        ColorPicker?.GetObservable(ColorPicker.Value1Property).Subscribe(_ => UpdatePropertyValues());
        ColorPicker?.GetObservable(ColorPicker.Value2Property).Subscribe(_ => UpdatePropertyValues());
        ColorPicker?.GetObservable(ColorPicker.Value3Property).Subscribe(_ => UpdatePropertyValues());
        ColorPicker?.GetObservable(ColorPicker.Value4Property).Subscribe(_ => UpdatePropertyValues());
    }
}
