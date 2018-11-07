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
        public static readonly StyledProperty<Color> HueColorProperty =
            AvaloniaProperty.Register<ColorPicker, Color>(nameof(HueColor), new Color(0xFF, 0xFF, 0x00, 0x00));

        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<ColorPicker, Color>(nameof(Color), new Color(0xFF, 0xFF, 0x00, 0x00));

        private Canvas _colorCanvas;
        private Thumb _colorThumb;
        private Canvas _hueCanvas;
        private Thumb _hueThumb;

        public ColorPicker()
        {
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

            _colorCanvas = e.NameScope.Find<Canvas>("PART_ColorCanvas");
            _colorThumb = e.NameScope.Find<Thumb>("PART_ColorThumb");
            _hueCanvas = e.NameScope.Find<Canvas>("PART_HueCanvas");
            _hueThumb = e.NameScope.Find<Thumb>("PART_HueThumb");

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
        }

        private bool IsTemplateValid()
        {
            return _colorCanvas != null
                && _colorThumb != null
                && _hueCanvas != null
                && _hueThumb != null;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            if (IsTemplateValid())
            {
                UpdateThumbs();
            }
            return size;
        }

        private Color ColorFromHue(double hue)
        {
            var rgb = new HSV(hue, 100, 100).ToRGB();
            var color = new Color(255, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
            return color;
        }

        private Color ColorFromHSV(double h, double s, double v)
        {
            var rgb = new HSV(h, s, v).ToRGB();
            var color = new Color(255, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
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
            HSV hsvHueColor = new RGB(HueColor.R, HueColor.G, HueColor.B).ToHSV();
            double hueX = 0;
            double hueY = (hsvHueColor.H * _hueCanvas.Bounds.Height) / 359.0;

            HSV hsvColor = new RGB(Color.R, Color.G, Color.B).ToHSV();
            double colorX = (hsvColor.S * _colorCanvas.Bounds.Width) / 100.0;
            double colorY = _colorCanvas.Bounds.Height - ((hsvColor.V * _colorCanvas.Bounds.Height) / 100.0);

            MoveThumb(_hueCanvas, _hueThumb, hueX, hueY);
            MoveThumb(_colorCanvas, _colorThumb, colorX, colorY);
        }

        private void UpdateHueColor()
        {
            double topHue = Canvas.GetTop(_hueThumb);
            double hue = (topHue * 359.0) / _hueCanvas.Bounds.Height;

            HueColor = ColorFromHue(hue);
        }

        private void UpdateColor()
        {
            double topHue = Canvas.GetTop(_hueThumb);
            double leftColor = Canvas.GetLeft(_colorThumb);
            double topColor = Canvas.GetTop(_colorThumb);

            double h = (topHue * 359.0) / _hueCanvas.Bounds.Height;
            double s = (leftColor * 100.0) / _colorCanvas.Bounds.Width;
            double v = 100.0 - ((topColor * 100.0) / _colorCanvas.Bounds.Height);

            Color = ColorFromHSV(h, s, v);
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

            MoveThumb(_hueCanvas, _hueThumb, left + e.Vector.X, top + e.Vector.Y);

            UpdateColor();
            UpdateHueColor();
        }
    }
}
