using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Controls.ColorPicker
{
    public class ColorPicker : UserControl
    {
        private Canvas _canvas;
        private Thumb _thumb;

        public ColorPicker()
        {
            this.InitializeComponent();

            _canvas = this.FindControl<Canvas>("canvas");
            _canvas.PointerPressed += Canvas_PointerPressed;
            _canvas.PointerReleased += Canvas_PointerReleased;
            _canvas.PointerMoved += Canvas_PointerMoved;

            _thumb = this.FindControl<Thumb>("thumb");
            _thumb.DragDelta += Thumb_DragDelta;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private double Clamp(double val, double min, double max)
        {
            return Math.Min(Math.Max(val, min), max);
        }

        private void MoveThumbTo(double x, double y)
        {
            double left = Clamp(x, 0, _canvas.Bounds.Width);
            double top = Clamp(y, 0, _canvas.Bounds.Height);
            Canvas.SetLeft(_thumb, left);
            Canvas.SetTop(_thumb, top);
        }

        private void Canvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_canvas);
                MoveThumbTo(position.X, position.Y);
                e.Device.Capture(_canvas);
            }
        }

        private void Canvas_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (e.Device.Captured == _canvas)
            {
                e.Device.Capture(null);
            }
        }

        private void Canvas_PointerMoved(object sender, PointerEventArgs e)
        {
            if (e.Device.Captured == _canvas)
            {
                var position = e.GetPosition(_canvas);
                MoveThumbTo(position.X, position.Y);
            }
        }

        private void Thumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_thumb);
            double top = Canvas.GetTop(_thumb);
            MoveThumbTo(left + e.Vector.X, top + e.Vector.Y);
        }
    }
}
