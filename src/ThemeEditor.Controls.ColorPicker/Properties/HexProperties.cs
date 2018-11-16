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
    public class HexProperties : AvaloniaObject
    {
        public static readonly StyledProperty<ColorPicker> ColorPickerProperty =
            AvaloniaProperty.Register<HexProperties, ColorPicker>(nameof(ColorPicker));

        public static readonly StyledProperty<string> HexProperty =
            AvaloniaProperty.Register<HexProperties, string>(nameof(Hex), "#FFFF0000", validate: ValidateHex);

        private static string ValidateHex(HexProperties cp, string hex)
        {
            if (!ColorHelpers.IsValidHexColor(hex))
            {
                throw new ArgumentException("Invalid Hex value.");
            }
            return hex;
        }

        private bool _updating = false;

        public HexProperties()
        {
            this.GetObservable(ColorPickerProperty).Subscribe(x => OnColorPickerChange());
            this.GetObservable(HexProperty).Subscribe(x => UpdateColorPickerValues());
        }

        public ColorPicker ColorPicker
        {
            get { return GetValue(ColorPickerProperty); }
            set { SetValue(ColorPickerProperty, value); }
        }

        public string Hex
        {
            get { return GetValue(HexProperty); }
            set { SetValue(HexProperty, value); }
        }

        private void UpdateColorPickerValues()
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                Color color = Color.Parse(Hex);
                ColorHelpers.FromColor(color, out double h, out double s, out double v, out double a);
                ColorPicker.Value1 = h;
                ColorPicker.Value2 = s;
                ColorPicker.Value3 = v;
                ColorPicker.Value4 = a;
                _updating = false;
            }
        }

        private void UpdatePropertyValues()
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                Color color = ColorHelpers.FromHSVA(ColorPicker.Value1, ColorPicker.Value2, ColorPicker.Value3, ColorPicker.Value4);
                Hex = ColorHelpers.ToHexColor(color);
                _updating = false;
            }
        }

        private void OnColorPickerChange()
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
