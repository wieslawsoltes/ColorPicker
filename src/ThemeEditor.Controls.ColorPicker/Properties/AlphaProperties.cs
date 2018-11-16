using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using ThemeEditor.Colors;

namespace ThemeEditor.Controls.ColorPicker
{
    public class AlphaProperties : AvaloniaObject
    {
        public static readonly StyledProperty<ColorPicker> ColorPickerProperty =
            AvaloniaProperty.Register<AlphaProperties, ColorPicker>(nameof(ColorPicker));

        public static readonly StyledProperty<double> AlphaProperty =
            AvaloniaProperty.Register<AlphaProperties, double>(nameof(Alpha), 100.0, validate: ValidateAlpha);

        private static double ValidateAlpha(AlphaProperties cp, double alpha)
        {
            if (alpha < 0.0 || alpha > 100.0)
            {
                throw new ArgumentException("Invalid Alpha value.");
            }
            return alpha;
        }

        private bool _updating = false;

        public AlphaProperties()
        {
            this.GetObservable(ColorPickerProperty).Subscribe(x => OnColorPickerChange());
            this.GetObservable(AlphaProperty).Subscribe(x => UpdateColorPickerValues());
        }

        public ColorPicker ColorPicker
        {
            get { return GetValue(ColorPickerProperty); }
            set { SetValue(ColorPickerProperty, value); }
        }

        public double Alpha
        {
            get { return GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        private void UpdateColorPickerValues()
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                ColorPicker.Value4 = Alpha;
                _updating = false;
            }
        }

        private void UpdatePropertyValues()
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                Alpha = ColorPicker.Value4;
                _updating = false;
            }
        }

        private void OnColorPickerChange()
        {
            if (ColorPicker != null)
            {
                ColorPicker.GetObservable(ColorPicker.Value4Property).Subscribe(x => UpdatePropertyValues());
            }
        }
    }
}
