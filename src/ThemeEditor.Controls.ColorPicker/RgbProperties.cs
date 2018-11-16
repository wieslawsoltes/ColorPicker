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
    public class RgbProperties : AvaloniaObject
    {
        public static readonly StyledProperty<ColorPicker> ColorPickerProperty =
            AvaloniaProperty.Register<RgbProperties, ColorPicker>(nameof(ColorPicker));

        public static readonly StyledProperty<byte> RedProperty =
            AvaloniaProperty.Register<RgbProperties, byte>(nameof(Red), 0xFF, validate: ValidateRed);

        public static readonly StyledProperty<byte> GreenProperty =
            AvaloniaProperty.Register<RgbProperties, byte>(nameof(Green), 0x00, validate: ValidateGreen);

        public static readonly StyledProperty<byte> BlueProperty =
            AvaloniaProperty.Register<RgbProperties, byte>(nameof(Blue), 0x00, validate: ValidateBlue);

        //public static readonly StyledProperty<double> AlphaProperty =
        //    AvaloniaProperty.Register<RgbProperties, double>(nameof(Alpha), 100.0, validate: ValidateAlpha);

        private static byte ValidateRed(RgbProperties cp, byte red)
        {
            if (red < 0 || red > 255)
            {
                throw new ArgumentException("Invalid Red value.");
            }
            return red;
        }

        private static byte ValidateGreen(RgbProperties cp, byte green)
        {
            if (green < 0 || green > 255)
            {
                throw new ArgumentException("Invalid Green value.");
            }
            return green;
        }

        private static byte ValidateBlue(RgbProperties cp, byte blue)
        {
            if (blue < 0 || blue > 255)
            {
                throw new ArgumentException("Invalid Blue value.");
            }
            return blue;
        }

        //private static double ValidateAlpha(RgbProperties cp, double alpha)
        //{
        //    if (alpha < 0.0 || alpha > 100.0)
        //    {
        //        throw new ArgumentException("Invalid Alpha value.");
        //    }
        //    return alpha;
        //}

        private bool _updating = false;

        public RgbProperties()
        {
            this.GetObservable(ColorPickerProperty).Subscribe(x => OnColorPickerChange(x));
            this.GetObservable(RedProperty).Subscribe(x => OnRedChange(x));
            this.GetObservable(GreenProperty).Subscribe(x => OnGreenChange(x));
            this.GetObservable(BlueProperty).Subscribe(x => OnBlueChange(x));
            //this.GetObservable(AlphaProperty).Subscribe(x => OnHsvaChange());
        }

        public ColorPicker ColorPicker
        {
            get { return GetValue(ColorPickerProperty); }
            set { SetValue(ColorPickerProperty, value); }
        }

        public byte Red
        {
            get { return GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public byte Green
        {
            get { return GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public byte Blue
        {
            get { return GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        //public double Alpha
        //{
        //    get { return GetValue(AlphaProperty); }
        //    set { SetValue(AlphaProperty, value); }
        //}

        private void UpdateColorPickerValues()
        {
            RGB rgb = new RGB(Red, Green, Blue);
            HSV hsv = rgb.ToHSV();
            ColorPicker.Value1 = hsv.H;
            ColorPicker.Value2 = hsv.S;
            ColorPicker.Value3 = hsv.V;
        }

        private void UpdatePropertyValues()
        {
            HSV hsv = new HSV(ColorPicker.Value1, ColorPicker.Value2, ColorPicker.Value3);
            RGB rgb = hsv.ToRGB();
            Red = (byte)rgb.R;
            Green = (byte)rgb.G;
            Blue = (byte)rgb.B;
        }

        private void OnColorPickerChange(ColorPicker colorPicker)
        {
            if (ColorPicker != null)
            {
                ColorPicker.GetObservable(ColorPicker.Value1Property).Subscribe(x => OnValue1Change(x));
                ColorPicker.GetObservable(ColorPicker.Value2Property).Subscribe(x => OnValue2Change(x));
                ColorPicker.GetObservable(ColorPicker.Value3Property).Subscribe(x => OnValue3Change(x));
            }
        }

        private void OnValue1Change(double value1)
        {
            if (ColorPicker != null)
            {
                _updating = true;
                UpdatePropertyValues();
                _updating = false;
            }
        }

        private void OnValue2Change(double value2)
        {
            if (ColorPicker != null)
            {
                _updating = true;
                UpdatePropertyValues();
                _updating = false;
            }
        }

        private void OnValue3Change(double value3)
        {
            if (ColorPicker != null)
            {
                _updating = true;
                UpdatePropertyValues();
                _updating = false;
            }
        }

        private void OnRedChange(byte red)
        {
            if (ColorPicker != null)
            {
                _updating = true;
                UpdateColorPickerValues();
                _updating = false;
            }
        }

        private void OnGreenChange(byte green)
        {
            if (ColorPicker != null)
            {
                _updating = true;
                UpdateColorPickerValues();
                _updating = false;
            }
        }

        private void OnBlueChange(byte value)
        {
            if (ColorPicker != null)
            {
                _updating = true;
                UpdateColorPickerValues();
                _updating = false;
            }
        }
    }
}
