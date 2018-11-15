using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using ThemeEditor.Controls.ColorPicker.Converters;

namespace ThemeEditor.Controls.ColorPicker
{
    public class ColorPicker : TemplatedControl
    {
        public static readonly StyledProperty<double> HueProperty =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Hue), 0.0, validate: ValidateHue);

        public static readonly StyledProperty<double> SaturationProperty =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Saturation), 100.0, validate: ValidateSaturation);

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Value), 100.0, validate: ValidateValue);

        public static readonly StyledProperty<double> AlphaProperty =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Alpha), 100.0, validate: ValidateAlpha);

        public static readonly StyledProperty<byte> RedProperty =
            AvaloniaProperty.Register<ColorPicker, byte>(nameof(Red), 0xFF, validate: ValidateRed);

        public static readonly StyledProperty<byte> GreenProperty =
            AvaloniaProperty.Register<ColorPicker, byte>(nameof(Green), 0x00, validate: ValidateGreen);

        public static readonly StyledProperty<byte> BlueProperty =
            AvaloniaProperty.Register<ColorPicker, byte>(nameof(Blue), 0x00, validate: ValidateBlue);

        public static readonly StyledProperty<string> HexProperty =
            AvaloniaProperty.Register<ColorPicker, string>(nameof(Hex), "#FFFF0000", validate: ValidateHex);

        private static double ValidateHue(ColorPicker cp, double hue)
        {
            if (hue < 0.0 || hue > 360.0)
            {
                throw new ArgumentException("Invalid Hue value.");
            }
            return hue;
        }

        private static double ValidateSaturation(ColorPicker cp, double saturation)
        {
            if (saturation < 0.0 || saturation > 100.0)
            {
                throw new ArgumentException("Invalid Saturation value.");
            }
            return saturation;
        }

        private static double ValidateValue(ColorPicker cp, double value)
        {
            if (value < 0.0 || value > 100.0)
            {
                throw new ArgumentException("Invalid Value value.");
            }
            return value;
        }

        private static double ValidateAlpha(ColorPicker cp, double alpha)
        {
            if (alpha < 0.0 || alpha > 100.0)
            {
                throw new ArgumentException("Invalid Alpha value.");
            }
            return alpha;
        }

        private static byte ValidateRed(ColorPicker cp, byte red)
        {
            if (red < 0 || red > 255)
            {
                throw new ArgumentException("Invalid Red value.");
            }
            return red;
        }

        private static byte ValidateGreen(ColorPicker cp, byte green)
        {
            if (green < 0 || green > 255)
            {
                throw new ArgumentException("Invalid Green value.");
            }
            return green;
        }

        private static byte ValidateBlue(ColorPicker cp, byte blue)
        {
            if (blue < 0 || blue > 255)
            {
                throw new ArgumentException("Invalid Blue value.");
            }
            return blue;
        }

        private static string ValidateHex(ColorPicker cp, string hex)
        {
            if (!ColorHelpers.IsValidHexColor(hex))
            {
                throw new ArgumentException("Invalid Hex value.");
            }
            return hex;
        }

        private Canvas _colorCanvas;
        private Thumb _colorThumb;
        private Canvas _hueCanvas;
        private Thumb _hueThumb;
        private Canvas _alphaCanvas;
        private Thumb _alphaThumb;
        private bool _updating = false;

        public ColorPicker()
        {
            this.GetObservable(HueProperty).Subscribe(x => OnHsvaChange());
            this.GetObservable(SaturationProperty).Subscribe(x => OnHsvaChange());
            this.GetObservable(ValueProperty).Subscribe(x => OnHsvaChange());
            this.GetObservable(AlphaProperty).Subscribe(x => OnHsvaChange());
            this.GetObservable(RedProperty).Subscribe(x => OnRgbChange());
            this.GetObservable(GreenProperty).Subscribe(x => OnRgbChange());
            this.GetObservable(BlueProperty).Subscribe(x => OnRgbChange());
            this.GetObservable(HexProperty).Subscribe(x => OnHexChange());
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

        public double Alpha
        {
            get { return GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
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

        public string Hex
        {
            get { return GetValue(HexProperty); }
            set { SetValue(HexProperty, value); }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            if (_colorCanvas != null)
            {
                _colorCanvas.PointerPressed -= ColorCanvas_PointerPressed;
                _colorCanvas.PointerReleased -= ColorCanvas_PointerReleased;
                _colorCanvas.PointerMoved -= ColorCanvas_PointerMoved;
            }

            if (_colorThumb != null)
            {
                _colorThumb.DragDelta -= ColorThumb_DragDelta;
            }

            if (_hueCanvas != null)
            {
                _hueCanvas.PointerPressed -= HueCanvas_PointerPressed;
                _hueCanvas.PointerReleased -= HueCanvas_PointerReleased;
                _hueCanvas.PointerMoved -= HueCanvas_PointerMoved;
            }

            if (_hueThumb != null)
            {
                _hueThumb.DragDelta -= HueThumb_DragDelta;
            }

            if (_alphaCanvas != null)
            {
                _alphaCanvas.PointerPressed -= AlphaCanvas_PointerPressed;
                _alphaCanvas.PointerReleased -= AlphaCanvas_PointerReleased;
                _alphaCanvas.PointerMoved -= AlphaCanvas_PointerMoved;
            }

            if (_alphaThumb != null)
            {
                _alphaThumb.DragDelta -= AlphaThumb_DragDelta;
            }

            _colorCanvas = e.NameScope.Find<Canvas>("PART_ColorCanvas");
            _colorThumb = e.NameScope.Find<Thumb>("PART_ColorThumb");
            _hueCanvas = e.NameScope.Find<Canvas>("PART_HueCanvas");
            _hueThumb = e.NameScope.Find<Thumb>("PART_HueThumb");
            _alphaCanvas = e.NameScope.Find<Canvas>("PART_AlphaCanvas");
            _alphaThumb = e.NameScope.Find<Thumb>("PART_AlphaThumb");

            if (_colorCanvas != null)
            {
                _colorCanvas.PointerPressed += ColorCanvas_PointerPressed;
                _colorCanvas.PointerReleased += ColorCanvas_PointerReleased;
                _colorCanvas.PointerMoved += ColorCanvas_PointerMoved;
            }

            if (_colorThumb != null)
            {
                _colorThumb.DragDelta += ColorThumb_DragDelta;
            }

            if (_hueCanvas != null)
            {
                _hueCanvas.PointerPressed += HueCanvas_PointerPressed;
                _hueCanvas.PointerReleased += HueCanvas_PointerReleased;
                _hueCanvas.PointerMoved += HueCanvas_PointerMoved;
            }

            if (_hueThumb != null)
            {
                _hueThumb.DragDelta += HueThumb_DragDelta;
            }

            if (_alphaCanvas != null)
            {
                _alphaCanvas.PointerPressed += AlphaCanvas_PointerPressed;
                _alphaCanvas.PointerReleased += AlphaCanvas_PointerReleased;
                _alphaCanvas.PointerMoved += AlphaCanvas_PointerMoved;
            }

            if (_alphaThumb != null)
            {
                _alphaThumb.DragDelta += AlphaThumb_DragDelta;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            if (IsTemplateValid() && !_updating)
            {
                _updating = true;
                UpdateThumbsFromHsva();
                UpdateHsvaFromThumbs();
                UpdateRgbFromHsva();
                UpdateHexFromHsva();
                _updating = false;
            }
            return size;
        }

        private bool IsTemplateValid()
        {
            return _colorCanvas != null
                && _colorThumb != null
                && _hueCanvas != null
                && _hueThumb != null
                && _alphaCanvas != null
                && _alphaThumb != null;
        }

        private double Clamp(double val, double min, double max)
        {
            return Math.Min(Math.Max(val, min), max);
        }

        private void MoveThumb(Canvas canvas, Thumb thumb, double x, double y)
        {
            double left = Clamp(x, 0, canvas.Bounds.Width);
            double top = Clamp(y, 0, canvas.Bounds.Height);
            Canvas.SetLeft(thumb, left);
            Canvas.SetTop(thumb, top);
        }

        private void UpdateThumbsFromHsva()
        {
            double hueX = 0;
            double hueY = Hue * _hueCanvas.Bounds.Height / 360.0;
            double colorX = Saturation * _colorCanvas.Bounds.Width / 100.0;
            double colorY = _colorCanvas.Bounds.Height - (Value * _colorCanvas.Bounds.Height / 100.0);
            double alphaX = Alpha * _alphaCanvas.Bounds.Width / 100.0;
            double alphaY = 0;
            MoveThumb(_hueCanvas, _hueThumb, hueX, hueY);
            MoveThumb(_colorCanvas, _colorThumb, colorX, colorY);
            MoveThumb(_alphaCanvas, _alphaThumb, alphaX, alphaY);
        }

        private void UpdateHsvaFromThumbs()
        {
            double hueY = Canvas.GetTop(_hueThumb);
            double colorX = Canvas.GetLeft(_colorThumb);
            double colorY = Canvas.GetTop(_colorThumb);
            double alphaX = Canvas.GetLeft(_alphaThumb);
            double h = hueY * 360.0 / _hueCanvas.Bounds.Height;
            double s = colorX * 100.0 / _colorCanvas.Bounds.Width;
            double v = 100.0 - (colorY * 100.0 / _colorCanvas.Bounds.Height);
            double a = alphaX * 100.0 / _alphaCanvas.Bounds.Width;
            Hue = h;
            Saturation = s;
            Value = v;
            Alpha = a;
        }

        private void UpdateHsvaFromHex()
        {
            if (ColorHelpers.IsValidHexColor(Hex))
            {
                Color color = Color.Parse(Hex);
                ColorHelpers.FromColor(color, out double h, out double s, out double v, out double a);
                Hue = h;
                Saturation = s;
                Value = v;
                Alpha = a;
            }
        }

        private void UpdateRgbFromHsva()
        {
            Color color = ColorHelpers.FromHSVA(Hue, Saturation, Value, Alpha);
            Red = color.R;
            Green = color.G;
            Blue = color.B;
        }

        private void UpdateHexFromHsva()
        {
            Color color = ColorHelpers.FromHSVA(Hue, Saturation, Value, Alpha);
            Hex = ColorHelpers.ToHexColor(color);
        }

        private void OnHsvaChange()
        {
            if (IsTemplateValid() && !_updating)
            {
                _updating = true;
                UpdateThumbsFromHsva();
                UpdateHsvaFromThumbs();
                UpdateRgbFromHsva();
                UpdateHexFromHsva();
                _updating = false;
            }
        }

        private void OnRgbChange()
        {
            if (_updating == false)
            {
                _updating = true;
                Color color = ColorHelpers.FromRGBA(Red, Green, Blue, Alpha);
                Hex = ColorHelpers.ToHexColor(color);
                UpdateHsvaFromHex();
                _updating = false;
            }
        }

        private void OnHexChange()
        {
            if (_updating == false)
            {
                _updating = true;
                UpdateHsvaFromHex();
                UpdateRgbFromHsva();
                _updating = false;
            }
        }

        private void ColorCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_colorCanvas);
                MoveThumb(_colorCanvas, _colorThumb, position.X, position.Y);
                _updating = true;
                UpdateHsvaFromThumbs();
                UpdateRgbFromHsva();
                UpdateHexFromHsva();
                _updating = false;
                e.Device.Capture(_colorCanvas);
            }
        }

        private void ColorCanvas_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (e.Device.Captured == _colorCanvas)
            {
                e.Device.Capture(null);
            }
        }

        private void ColorCanvas_PointerMoved(object sender, PointerEventArgs e)
        {
            if (e.Device.Captured == _colorCanvas)
            {
                var position = e.GetPosition(_colorCanvas);
                MoveThumb(_colorCanvas, _colorThumb, position.X, position.Y);
                _updating = true;
                UpdateHsvaFromThumbs();
                UpdateRgbFromHsva();
                UpdateHexFromHsva();
                _updating = false;
            }
        }

        private void ColorThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_colorThumb);
            double top = Canvas.GetTop(_colorThumb);
            MoveThumb(_colorCanvas, _colorThumb, left + e.Vector.X, top + e.Vector.Y);
            _updating = true;
            UpdateHsvaFromThumbs();
            UpdateRgbFromHsva();
            UpdateHexFromHsva();
            _updating = false;
        }

        private void HueCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_hueCanvas);
                MoveThumb(_hueCanvas, _hueThumb, 0, position.Y);
                _updating = true;
                UpdateHsvaFromThumbs();
                UpdateRgbFromHsva();
                UpdateHexFromHsva();
                _updating = false;
                e.Device.Capture(_hueCanvas);
            }
        }

        private void HueCanvas_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (e.Device.Captured == _hueCanvas)
            {
                e.Device.Capture(null);
            }
        }

        private void HueCanvas_PointerMoved(object sender, PointerEventArgs e)
        {
            if (e.Device.Captured == _hueCanvas)
            {
                var position = e.GetPosition(_hueCanvas);
                MoveThumb(_hueCanvas, _hueThumb, 0, position.Y);
                _updating = true;
                UpdateHsvaFromThumbs();
                UpdateRgbFromHsva();
                UpdateHexFromHsva();
                _updating = false;
            }
        }

        private void HueThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double top = Canvas.GetTop(_hueThumb);
            MoveThumb(_hueCanvas, _hueThumb, 0, top + e.Vector.Y);
            _updating = true;
            UpdateHsvaFromThumbs();
            UpdateRgbFromHsva();
            UpdateHexFromHsva();
            _updating = false;
        }

        private void AlphaCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_alphaCanvas);
                MoveThumb(_alphaCanvas, _alphaThumb, position.X, 0);
                _updating = true;
                UpdateHsvaFromThumbs();
                UpdateRgbFromHsva();
                UpdateHexFromHsva();
                _updating = false;
                e.Device.Capture(_alphaCanvas);
            }
        }

        private void AlphaCanvas_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (e.Device.Captured == _alphaCanvas)
            {
                e.Device.Capture(null);
            }
        }

        private void AlphaCanvas_PointerMoved(object sender, PointerEventArgs e)
        {
            if (e.Device.Captured == _alphaCanvas)
            {
                var position = e.GetPosition(_alphaCanvas);
                MoveThumb(_alphaCanvas, _alphaThumb, position.X, 0);
                _updating = true;
                UpdateHsvaFromThumbs();
                UpdateRgbFromHsva();
                UpdateHexFromHsva();
                _updating = false;
            }
        }

        private void AlphaThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_alphaThumb);
            MoveThumb(_alphaCanvas, _alphaThumb, left + e.Vector.X, 0);
            _updating = true;
            UpdateHsvaFromThumbs();
            UpdateRgbFromHsva();
            UpdateHexFromHsva();
            _updating = false;
        }
    }
}
