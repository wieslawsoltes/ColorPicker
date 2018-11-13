using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using ThemeEditor.Colors;

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

        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<ColorPicker, Color>(nameof(Color), new Color(0xFF, 0xFF, 0x00, 0x00));

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

        private Canvas _colorCanvas;
        private Thumb _colorThumb;
        private Canvas _hueCanvas;
        private Thumb _hueThumb;
        private Canvas _alphaCanvas;
        private Thumb _alphaThumb;
        private bool _updating = false;

        public ColorPicker()
        {
            this.GetObservable(HueProperty).Subscribe(x => UpdateOnHsvaChange());
            this.GetObservable(SaturationProperty).Subscribe(x => UpdateOnHsvaChange());
            this.GetObservable(ValueProperty).Subscribe(x => UpdateOnHsvaChange());
            this.GetObservable(AlphaProperty).Subscribe(x => UpdateOnHsvaChange());
            this.GetObservable(ColorProperty).Subscribe(x => UpdateOnColorChange());
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

        public Color Color
        {
            get { return GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
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
                UpdateThumbs();
                UpdateProperties();
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

        private void UpdateThumbs()
        {
            _updating = true;
            double hueX = 0;
            double hueY = (Hue * _hueCanvas.Bounds.Height) / 360.0;
            double colorX = (Saturation * _colorCanvas.Bounds.Width) / 100.0;
            double colorY = _colorCanvas.Bounds.Height - ((Value * _colorCanvas.Bounds.Height) / 100.0);
            double alphaX = ((Alpha * _alphaCanvas.Bounds.Width) / 100.0);
            double alphaY = 0;
            MoveThumb(_hueCanvas, _hueThumb, hueX, hueY);
            MoveThumb(_colorCanvas, _colorThumb, colorX, colorY);
            MoveThumb(_alphaCanvas, _alphaThumb, alphaX, alphaY);
            _updating = false;
        }

        private void UpdateProperties()
        {
            _updating = true;
            double hueY = Canvas.GetTop(_hueThumb);
            double colorX = Canvas.GetLeft(_colorThumb);
            double colorY = Canvas.GetTop(_colorThumb);
            double alphaX = Canvas.GetLeft(_alphaThumb);
            double h = (hueY * 360.0) / _hueCanvas.Bounds.Height;
            double s = (colorX * 100.0) / _colorCanvas.Bounds.Width;
            double v = 100.0 - ((colorY * 100.0) / _colorCanvas.Bounds.Height);
            double a = (alphaX * 100.0) / _alphaCanvas.Bounds.Width;
            Hue = h;
            Saturation = s;
            Value = v;
            Alpha = a;
            RGB rgb = new HSV(h, s, v).ToRGB();
            byte A = (byte)((a * 255.0) / 100.0);
            Color = new Color(A, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
            _updating = false;
        }

        private void UpdateOnHsvaChange()
        {
            if (IsTemplateValid() && !_updating)
            {
                UpdateThumbs();
                UpdateProperties();
            }
        }

        private void UpdateOnColorChange()
        {
            if (IsTemplateValid() && !_updating)
            {
                _updating = true;
                HSV hsv = new RGB(Color.R, Color.G, Color.B).ToHSV();
                Hue = hsv.H;
                Saturation = hsv.S;
                Value = hsv.V;
                Alpha = (Color.A * 100.0) / 255.0;
                _updating = false;
                UpdateThumbs();
            }
        }

        private void ColorCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_colorCanvas);
                MoveThumb(_colorCanvas, _colorThumb, position.X, position.Y);
                UpdateProperties();
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
                UpdateProperties();
            }
        }

        private void ColorThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_colorThumb);
            double top = Canvas.GetTop(_colorThumb);
            MoveThumb(_colorCanvas, _colorThumb, left + e.Vector.X, top + e.Vector.Y);
            UpdateProperties();
        }

        private void HueCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_hueCanvas);
                MoveThumb(_hueCanvas, _hueThumb, 0, position.Y);
                UpdateProperties();
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
                UpdateProperties();
            }
        }

        private void HueThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double top = Canvas.GetTop(_hueThumb);
            MoveThumb(_hueCanvas, _hueThumb, 0, top + e.Vector.Y);
            UpdateProperties();
        }

        private void AlphaCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_alphaCanvas);
                MoveThumb(_alphaCanvas, _alphaThumb, position.X, 0);
                UpdateProperties();
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
                UpdateProperties();
            }
        }

        private void AlphaThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_alphaThumb);
            MoveThumb(_alphaCanvas, _alphaThumb, left + e.Vector.X, 0);
            UpdateProperties();
        }
    }
}
