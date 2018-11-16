using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;

namespace ThemeEditor.Controls.ColorPicker
{
    public abstract class ColorPickerProperties : AvaloniaObject
    {
        public static readonly StyledProperty<ColorPicker> ColorPickerProperty =
            AvaloniaProperty.Register<ColorPickerProperties, ColorPicker>(nameof(ColorPicker));

        public ColorPickerProperties()
        {
            this.GetObservable(ColorPickerProperty).Subscribe(x => OnColorPickerChange());
        }

        public ColorPicker ColorPicker
        {
            get { return GetValue(ColorPickerProperty); }
            set { SetValue(ColorPickerProperty, value); }
        }

        public abstract void UpdateColorPickerValues();

        public abstract void UpdatePropertyValues();

        public virtual void OnColorPickerChange()
        {
            if (ColorPicker != null)
            {
                ColorPicker.GetObservable(ColorPicker.Value1Property).Subscribe(x => UpdatePropertyValues());
                ColorPicker.GetObservable(ColorPicker.Value2Property).Subscribe(x => UpdatePropertyValues());
                ColorPicker.GetObservable(ColorPicker.Value3Property).Subscribe(x => UpdatePropertyValues());
                ColorPicker.GetObservable(ColorPicker.Value4Property).Subscribe(x => UpdatePropertyValues());
            }
        }
    }
}
