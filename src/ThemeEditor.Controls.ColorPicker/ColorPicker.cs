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
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Hue), 0.0);

        public static readonly StyledProperty<double> SaturationProperty =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Saturation), 100.0);

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Value), 100.0);

        public static readonly StyledProperty<double> AlphaProperty =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Alpha), 100.0);

        public static readonly StyledProperty<Color> HueColorProperty =
            AvaloniaProperty.Register<ColorPicker, Color>(nameof(HueColor), new Color(0xFF, 0xFF, 0x00, 0x00));

        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<ColorPicker, Color>(nameof(Color), new Color(0xFF, 0xFF, 0x00, 0x00));

        private Canvas _colorCanvas;
        private Thumb _colorThumb;
        private Canvas _hueCanvas;
        private Thumb _hueThumb;
        private Canvas _alphaCanvas;
        private Thumb _alphaThumb;

        public ColorPicker()
        {
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

        public Color HueColor
        {
            get { return GetValue(HueColorProperty); }
            set { SetValue(HueColorProperty, value); }
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

        private bool IsTemplateValid()
        {
            return _colorCanvas != null
                && _colorThumb != null
                && _hueCanvas != null
                && _hueThumb != null
                && _alphaCanvas != null
                && _alphaThumb != null;
            ;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            if (IsTemplateValid())
            {
                UpdateThumbs();
                UpdateHueColor();
                UpdateColor();
            }
            return size;
        }

        private Color ColorFromHue(double hue)
        {
            var rgb = new HSV(hue, 100, 100).ToRGB();
            var color = new Color(255, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
            return color;
        }

        private Color ColorFromHSV(byte a, double h, double s, double v)
        {
            var rgb = new HSV(h, s, v).ToRGB();
            var color = new Color(a, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
            return color;
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
            double hueX = 0;
            double hueY = (Hue * _hueCanvas.Bounds.Height) / 359.0;

            double colorX = (Saturation * _colorCanvas.Bounds.Width) / 100.0;
            double colorY = _colorCanvas.Bounds.Height - ((Value * _colorCanvas.Bounds.Height) / 100.0);

            double alphaX = ((Alpha * _alphaCanvas.Bounds.Width) / 100.0);
            double alphaY = 0;

            MoveThumb(_hueCanvas, _hueThumb, hueX, hueY);
            MoveThumb(_colorCanvas, _colorThumb, colorX, colorY);
            MoveThumb(_alphaCanvas, _alphaThumb, alphaX, alphaY);
        }

        private void UpdateHueColor()
        {
            double topHue = Canvas.GetTop(_hueThumb);
            double hue = (topHue * 359.0) / _hueCanvas.Bounds.Height;

            Hue = hue;
            HueColor = ColorFromHue(hue);
        }

        private void UpdateColor()
        {
            double topHue = Canvas.GetTop(_hueThumb);
            double leftColor = Canvas.GetLeft(_colorThumb);
            double topColor = Canvas.GetTop(_colorThumb);
            double leftAlpha = Canvas.GetLeft(_alphaThumb);

            double h = (topHue * 359.0) / _hueCanvas.Bounds.Height;
            double s = (leftColor * 100.0) / _colorCanvas.Bounds.Width;
            double v = 100.0 - ((topColor * 100.0) / _colorCanvas.Bounds.Height);
            double a = (leftAlpha * 100.0) / _alphaCanvas.Bounds.Width;

            Saturation = s;
            Value = v;
            Alpha = a;
            Color = ColorFromHSV((byte)((a * 255.0) / 100.0), h, s, v);
        }

        private void ColorCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_colorCanvas);

                MoveThumb(_colorCanvas, _colorThumb, position.X, position.Y);

                UpdateColor();
                UpdateHueColor();

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

                UpdateColor();
                UpdateHueColor();
            }
        }

        private void ColorThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_colorThumb);
            double top = Canvas.GetTop(_colorThumb);

            MoveThumb(_colorCanvas, _colorThumb, left + e.Vector.X, top + e.Vector.Y);

            UpdateColor();
            UpdateHueColor();
        }

        private void HueCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_hueCanvas);

                MoveThumb(_hueCanvas, _hueThumb, 0, position.Y);

                UpdateColor();
                UpdateHueColor();

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

                UpdateColor();
                UpdateHueColor();
            }
        }

        private void HueThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_hueThumb);
            double top = Canvas.GetTop(_hueThumb);

            MoveThumb(_hueCanvas, _hueThumb, 0, top + e.Vector.Y);

            UpdateColor();
            UpdateHueColor();
        }

        private void AlphaCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_alphaCanvas);

                MoveThumb(_alphaCanvas, _alphaThumb, position.X, 0);

                UpdateColor();
                UpdateHueColor();

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

                UpdateColor();
                UpdateHueColor();
            }
        }

        private void AlphaThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_alphaThumb);
            double top = Canvas.GetTop(_alphaThumb);

            MoveThumb(_alphaCanvas, _alphaThumb, left + e.Vector.X, 0);

            UpdateColor();
            UpdateHueColor();
        }
    }
}
