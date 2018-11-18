using System;
using Avalonia;
using Avalonia.Media;
using ThemeEditor.Controls.ColorPicker.Converters;

namespace ThemeEditor.Controls.ColorPicker
{
    public class HexProperties : ColorPickerProperties
    {
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

        public HexProperties() : base()
        {
            this.GetObservable(HexProperty).Subscribe(x => UpdateColorPickerValues());
        }

        public string Hex
        {
            get { return GetValue(HexProperty); }
            set { SetValue(HexProperty, value); }
        }

        public override void UpdateColorPickerValues()
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

        public override void UpdatePropertyValues()
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                Color color = ColorHelpers.FromHSVA(ColorPicker.Value1, ColorPicker.Value2, ColorPicker.Value3, ColorPicker.Value4);
                Hex = ColorHelpers.ToHexColor(color);
                _updating = false;
            }
        }
    }
}
