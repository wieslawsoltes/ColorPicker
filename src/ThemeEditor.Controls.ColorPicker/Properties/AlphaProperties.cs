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
    public class AlphaProperties : ColorPickerProperties
    {
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

        public AlphaProperties() : base()
        {
            this.GetObservable(AlphaProperty).Subscribe(x => UpdateColorPickerValues());
        }

        public double Alpha
        {
            get { return GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        public override void UpdateColorPickerValues()
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                ColorPicker.Value4 = Alpha;
                _updating = false;
            }
        }

        public override void UpdatePropertyValues()
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                Alpha = ColorPicker.Value4;
                _updating = false;
            }
        }
    }
}
