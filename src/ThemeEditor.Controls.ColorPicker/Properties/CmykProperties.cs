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
    public class CmykProperties : AvaloniaObject
    {
        public static readonly StyledProperty<ColorPicker> ColorPickerProperty =
            AvaloniaProperty.Register<CmykProperties, ColorPicker>(nameof(ColorPicker));

        public static readonly StyledProperty<double> CyanProperty =
            AvaloniaProperty.Register<CmykProperties, double>(nameof(Cyan), 0.0, validate: ValidateCyan);

        public static readonly StyledProperty<double> MagentaProperty =
            AvaloniaProperty.Register<CmykProperties, double>(nameof(Magenta), 100.0, validate: ValidateMagenta);

        public static readonly StyledProperty<double> YellowProperty =
            AvaloniaProperty.Register<CmykProperties, double>(nameof(Yellow), 100.0, validate: ValidateYellow);

        public static readonly StyledProperty<double> BlackKeyProperty =
            AvaloniaProperty.Register<CmykProperties, double>(nameof(BlackKey), 0.0, validate: ValidateBlackKey);

        private static double ValidateCyan(CmykProperties cp, double cyan)
        {
            if (cyan < 0.0 || cyan > 100.0)
            {
                throw new ArgumentException("Invalid Cyan value.");
            }
            return cyan;
        }

        private static double ValidateMagenta(CmykProperties cp, double magenta)
        {
            if (magenta < 0.0 || magenta > 100.0)
            {
                throw new ArgumentException("Invalid Magenta value.");
            }
            return magenta;
        }

        private static double ValidateYellow(CmykProperties cp, double yellow)
        {
            if (yellow < 0.0 || yellow > 100.0)
            {
                throw new ArgumentException("Invalid Yellow value.");
            }
            return yellow;
        }

        private static double ValidateBlackKey(CmykProperties cp, double blackKey)
        {
            if (blackKey < 0.0 || blackKey > 100.0)
            {
                throw new ArgumentException("Invalid BlackKey value.");
            }
            return blackKey;
        }

        private bool _updating = false;

        public CmykProperties()
        {
            this.GetObservable(ColorPickerProperty).Subscribe(x => OnColorPickerChange(x));
            this.GetObservable(CyanProperty).Subscribe(x => OnCyanChange(x));
            this.GetObservable(MagentaProperty).Subscribe(x => OnMagentaChange(x));
            this.GetObservable(YellowProperty).Subscribe(x => OnYellowChange(x));
            this.GetObservable(BlackKeyProperty).Subscribe(x => OnBlackKeyChange(x));
        }

        public ColorPicker ColorPicker
        {
            get { return GetValue(ColorPickerProperty); }
            set { SetValue(ColorPickerProperty, value); }
        }

        public double Cyan
        {
            get { return GetValue(CyanProperty); }
            set { SetValue(CyanProperty, value); }
        }

        public double Magenta
        {
            get { return GetValue(MagentaProperty); }
            set { SetValue(MagentaProperty, value); }
        }

        public double Yellow
        {
            get { return GetValue(YellowProperty); }
            set { SetValue(YellowProperty, value); }
        }

        public double BlackKey
        {
            get { return GetValue(BlackKeyProperty); }
            set { SetValue(BlackKeyProperty, value); }
        }

        private void UpdateColorPickerValues()
        {
            CMYK cmyk = new CMYK(Cyan, Magenta, Yellow, BlackKey);
            HSV hsv = cmyk.ToHSV();
            ColorPicker.Value1 = hsv.H;
            ColorPicker.Value2 = hsv.S;
            ColorPicker.Value3 = hsv.V;
        }

        private void UpdatePropertyValues()
        {
            HSV hsv = new HSV(ColorPicker.Value1, ColorPicker.Value2, ColorPicker.Value3);
            CMYK cmyk = hsv.ToCMYK();
            Cyan = cmyk.C;
            Magenta = cmyk.M;
            Yellow = cmyk.Y;
            BlackKey = cmyk.K;
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

        private void OnCyanChange(double cyan)
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                UpdateColorPickerValues();
                _updating = false;
            }
        }

        private void OnMagentaChange(double magenta)
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                UpdateColorPickerValues();
                _updating = false;
            }
        }

        private void OnYellowChange(double yellow)
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                UpdateColorPickerValues();
                _updating = false;
            }
        }

        private void OnBlackKeyChange(double blackKey)
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
