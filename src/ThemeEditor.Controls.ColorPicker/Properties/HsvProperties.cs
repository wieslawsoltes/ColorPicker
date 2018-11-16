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
    public class HsvProperties : AvaloniaObject
    {
        public static readonly StyledProperty<ColorPicker> ColorPickerProperty =
            AvaloniaProperty.Register<HsvProperties, ColorPicker>(nameof(ColorPicker));

        public static readonly StyledProperty<double> HueProperty =
            AvaloniaProperty.Register<HsvProperties, double>(nameof(Hue), 0.0, validate: ValidateHue);

        public static readonly StyledProperty<double> SaturationProperty =
            AvaloniaProperty.Register<HsvProperties, double>(nameof(Saturation), 100.0, validate: ValidateSaturation);

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<HsvProperties, double>(nameof(Value), 100.0, validate: ValidateValue);

        private static double ValidateHue(HsvProperties cp, double hue)
        {
            if (hue < 0.0 || hue > 360.0)
            {
                throw new ArgumentException("Invalid Hue value.");
            }
            return hue;
        }

        private static double ValidateSaturation(HsvProperties cp, double saturation)
        {
            if (saturation < 0.0 || saturation > 100.0)
            {
                throw new ArgumentException("Invalid Saturation value.");
            }
            return saturation;
        }

        private static double ValidateValue(HsvProperties cp, double value)
        {
            if (value < 0.0 || value > 100.0)
            {
                throw new ArgumentException("Invalid Value value.");
            }
            return value;
        }

        private bool _updating = false;

        public HsvProperties()
        {
            this.GetObservable(ColorPickerProperty).Subscribe(x => OnColorPickerChange(x));
            this.GetObservable(HueProperty).Subscribe(x => OnHueChange(x));
            this.GetObservable(SaturationProperty).Subscribe(x => OnSaturationChange(x));
            this.GetObservable(ValueProperty).Subscribe(x => OnValueChange(x));
        }

        public ColorPicker ColorPicker
        {
            get { return GetValue(ColorPickerProperty); }
            set { SetValue(ColorPickerProperty, value); }
        }

        public double Hue
        {
            get { return GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }

        public double Saturation
        {
            get { return GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }

        public double Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private void UpdateColorPickerValues()
        {
            ColorPicker.Value1 = Hue;
            ColorPicker.Value2 = Saturation;
            ColorPicker.Value3 = Value;
        }

        private void UpdatePropertyValues()
        {
            Hue = ColorPicker.Value1;
            Saturation = ColorPicker.Value2;
            Value = ColorPicker.Value3;
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
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                UpdatePropertyValues();
                _updating = false;
            }
        }

        private void OnValue2Change(double value2)
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                UpdatePropertyValues();
                _updating = false;
            }
        }

        private void OnValue3Change(double value3)
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                UpdatePropertyValues();
                _updating = false;
            }
        }

        private void OnHueChange(double hue)
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                UpdateColorPickerValues();
                _updating = false;
            }
        }

        private void OnSaturationChange(double saturation)
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                UpdateColorPickerValues();
                _updating = false;
            }
        }

        private void OnValueChange(double value)
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                UpdateColorPickerValues();
                _updating = false;
            }
        }
    }
}
