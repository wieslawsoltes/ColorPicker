using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ThemeEditor.Colors;

namespace ThemeEditor.Controls.ColorPicker
{
    public class ColorPicker : UserControl
    {
        private Canvas _colorCanvas;
        private Thumb _colorThumb;
        private Canvas _hueCanvas;
        private Thumb _hueThumb;

        public ColorPicker()
        {
            this.InitializeComponent();

            _colorCanvas = this.FindControl<Canvas>("colorCanvas");
            _colorCanvas.PointerPressed += ColorCanvas_PointerPressed;
            _colorCanvas.PointerReleased += ColorCanvas_PointerReleased;
            _colorCanvas.PointerMoved += ColorCanvas_PointerMoved;

            _colorThumb = this.FindControl<Thumb>("colorThumb");
            _colorThumb.DragDelta += ColorThumb_DragDelta;

            _hueCanvas = this.FindControl<Canvas>("hueCanvas");
            _hueCanvas.PointerPressed += HueCanvas_PointerPressed;
            _hueCanvas.PointerReleased += HueCanvas_PointerReleased;
            _hueCanvas.PointerMoved += HueCanvas_PointerMoved;

            _hueThumb = this.FindControl<Thumb>("hueThumb");
            _hueThumb.DragDelta += HueThumb_DragDelta;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            var color = (Color)Resources["HueColor"];
            var hsv = new RGB(color.R, color.G, color.B).ToHSV();

            MoveHueThumb(0, (hsv.H * _hueCanvas.Bounds.Height) / 359);

            double x = (hsv.S * _colorCanvas.Bounds.Width) / 100.0;
            double y = _colorCanvas.Bounds.Height - ((hsv.V * _colorCanvas.Bounds.Height) / 100.0);

            MoveColorThumb(x, y);

            UpdateColorBrush();
            UpdateHueColor();

            return size;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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

        private void UpdateHueColor()
        {
            double topHue = Canvas.GetTop(_hueThumb);
            double hue = (topHue * 359.0) / _hueCanvas.Bounds.Height;

            Resources["HueColor"] = ColorFromHue(hue);
        }

        private void UpdateColorBrush()
        {
            double topHue = Canvas.GetTop(_hueThumb);
            double hue = (topHue * 359.0) / _hueCanvas.Bounds.Height;

            double leftColor = Canvas.GetLeft(_colorThumb);
            double topColor = Canvas.GetTop(_colorThumb);
            double saturation = (leftColor * 100.0) / _colorCanvas.Bounds.Width;
            double lightness = 100.0 - ((topColor * 100.0) / _colorCanvas.Bounds.Height);

            var color = ColorFromHSV(hue, saturation, lightness);
            var brush = new SolidColorBrush(color);

            Resources["ColorBrush"] = brush;
        }

        private void MoveColorThumb(double x, double y)
        {
            double left = Clamp(x, 0, _colorCanvas.Bounds.Width);
            double top = Clamp(y, 0, _colorCanvas.Bounds.Height);
            Canvas.SetLeft(_colorThumb, left);
            Canvas.SetTop(_colorThumb, top);
        }

        private void MoveHueThumb(double x, double y)
        {
            double top = Clamp(y, 0, _hueCanvas.Bounds.Height);
            Canvas.SetTop(_hueThumb, top);
        }

        private void ColorCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_colorCanvas);

                MoveColorThumb(position.X, position.Y);

                UpdateColorBrush();
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

                MoveColorThumb(position.X, position.Y);

                UpdateColorBrush();
                UpdateHueColor();
            }
        }

        private void ColorThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_colorThumb);
            double top = Canvas.GetTop(_colorThumb);

            MoveColorThumb(left + e.Vector.X, top + e.Vector.Y);

            UpdateColorBrush();
            UpdateHueColor();
        }

        private void HueCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_hueCanvas);

                MoveHueThumb(position.X, position.Y);

                UpdateColorBrush();
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

                MoveHueThumb(position.X, position.Y);

                UpdateColorBrush();
                UpdateHueColor();
            }
        }

        private void HueThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_hueThumb);
            double top = Canvas.GetTop(_hueThumb);
            MoveHueThumb(left + e.Vector.X, top + e.Vector.Y);

            UpdateColorBrush();
            UpdateHueColor();
        }
    }
}
