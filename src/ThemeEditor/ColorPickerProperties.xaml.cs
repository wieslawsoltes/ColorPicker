using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ThemeEditor.Controls.ColorPicker;
using ThemeEditor.Controls.ColorPicker.Converters;

namespace ThemeEditor
{
    public class ColorPickerProperties : UserControl
    {
        public static readonly StyledProperty<ColorPicker> ColorPickerProperty =
            AvaloniaProperty.Register<ColorPickerProperties, ColorPicker>(nameof(ColorPicker));

        public static readonly StyledProperty<string> HexProperty =
            AvaloniaProperty.Register<ColorPickerProperties, string>(nameof(Hex), "", validate: ValidateHex);

        public static readonly StyledProperty<byte> RedProperty =
            AvaloniaProperty.Register<ColorPickerProperties, byte>(nameof(Red), 0xFF, validate: ValidateRed);

        public static readonly StyledProperty<byte> GreenProperty =
            AvaloniaProperty.Register<ColorPickerProperties, byte>(nameof(Green), 0xFF, validate: ValidateGreen);

        public static readonly StyledProperty<byte> BlueProperty =
            AvaloniaProperty.Register<ColorPickerProperties, byte>(nameof(Blue), 0xFF, validate: ValidateBlue);

        private static string ValidateHex(ColorPickerProperties view, string hex)
        {
            if (!ColorHelpers.IsValidHexColor(hex))
            {
                throw new ArgumentException("Invalid Hex value.");
            }
            return hex;
        }

        private static byte ValidateRed(ColorPickerProperties view, byte red)
        {
            if (red < 0 || red > 255)
            {
                throw new ArgumentException("Invalid Red value.");
            }
            return red;
        }

        private static byte ValidateGreen(ColorPickerProperties view, byte green)
        {
            if (green < 0 || green > 255)
            {
                throw new ArgumentException("Invalid Green value.");
            }
            return green;
        }

        private static byte ValidateBlue(ColorPickerProperties view, byte blue)
        {
            if (blue < 0 || blue > 255)
            {
                throw new ArgumentException("Invalid Blue value.");
            }
            return blue;
        }

        private CompositeDisposable _disposables = null;
        private bool _updating = false;

        public ColorPickerProperties()
        {
            this.InitializeComponent();
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

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (ColorPicker != null)
            {
                _disposables = new CompositeDisposable(
                    ColorPicker.GetObservable(ColorPicker.HueProperty).Subscribe(x => UpdateOnHsvaChange()),
                    ColorPicker.GetObservable(ColorPicker.SaturationProperty).Subscribe(x => UpdateOnHsvaChange()),
                    ColorPicker.GetObservable(ColorPicker.ValueProperty).Subscribe(x => UpdateOnHsvaChange()),
                    ColorPicker.GetObservable(ColorPicker.AlphaProperty).Subscribe(x => UpdateOnHsvaChange()),
                    this.GetObservable(HexProperty).Subscribe(x => UpdateOnHexChange()),
                    this.GetObservable(RedProperty).Subscribe(x => UpdateOnRGBChange()),
                    this.GetObservable(GreenProperty).Subscribe(x => UpdateOnRGBChange()),
                    this.GetObservable(BlueProperty).Subscribe(x => UpdateOnRGBChange()));
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            if (_disposables != null)
            {
                _disposables.Dispose();
                _disposables = null;
            }
        }

        private Color GetColor()
        {
            return ColorHelpers.FromHSVA(
                ColorPicker.Hue,
                ColorPicker.Saturation,
                ColorPicker.Value,
                ColorPicker.Alpha);
        }

        private void UpdateRGB()
        {
            Color color = GetColor();
            Red = color.R;
            Green = color.G;
            Blue = color.B;
        }

        private void UpdateHex()
        {
            Color color = GetColor();
            Hex = ColorHelpers.ToHexColor(color);
        }

        private void UpdatePicker()
        {
            if (ColorHelpers.IsValidHexColor(Hex))
            {
                Color color = Color.Parse(Hex);
                ColorHelpers.FromColor(color,
                    out double h,
                    out double s,
                    out double v,
                    out double a);
                ColorPicker.Hue = h;
                ColorPicker.Saturation = s;
                ColorPicker.Value = v;
                ColorPicker.Alpha = a;
            }
        }

        private void UpdateOnHsvaChange()
        {
            if (_updating == false)
            {
                _updating = true;
                UpdateHex();
                UpdateRGB();
                _updating = false;
            }
        }

        private void UpdateOnHexChange()
        {
            if (_updating == false)
            {
                _updating = true;
                UpdatePicker();
                UpdateRGB();
                _updating = false;
            }
        }

        private void UpdateOnRGBChange()
        {
            if (_updating == false)
            {
                _updating = true;
                Color color = ColorHelpers.FromRGBA(Red, Green, Blue, ColorPicker.Alpha);
                Hex = ColorHelpers.ToHexColor(color);
                UpdatePicker();
                _updating = false;
            }
        }
    }
}
