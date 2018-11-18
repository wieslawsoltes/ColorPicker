using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;

namespace ThemeEditor.Controls.ColorPicker
{
    public class HueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is double range && targetType == typeof(double))
            {
                return v * range / 360.0;
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is double range && targetType == typeof(double))
            {
                return v * 360.0 / range;
            }
            return AvaloniaProperty.UnsetValue;
        }
    }

    public class SaturationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is double range && targetType == typeof(double))
            {
                return v * range / 100.0;
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is double range && targetType == typeof(double))
            {
                return v * 100.0 / range;
            }
            return AvaloniaProperty.UnsetValue;
        }
    }

    public class ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is double range && targetType == typeof(double))
            {
                return range - (v * range / 100.0);
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is double range && targetType == typeof(double))
            {
                return 100.0 - (v * 100.0 / range);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }

    public class AlphaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is double range && targetType == typeof(double))
            {
                return v * range / 100.0;
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is double range && targetType == typeof(double))
            {
                return v * 100.0 / range;
            }
            return AvaloniaProperty.UnsetValue;
        }
    }

    public class ColorPicker : TemplatedControl
    {
        public static readonly StyledProperty<double> Value1Property =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Value1));

        public static readonly StyledProperty<double> Value2Property =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Value2));

        public static readonly StyledProperty<double> Value3Property =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Value3));

        public static readonly StyledProperty<double> Value4Property =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Value4));

        private Canvas _colorCanvas;
        private Thumb _colorThumb;
        private Canvas _hueCanvas;
        private Thumb _hueThumb;
        private Canvas _alphaCanvas;
        private Thumb _alphaThumb;
        private bool _updating = false;
        private IValueConverter _value1Converter = new HueConverter();
        private IValueConverter _value2Converter = new SaturationConverter();
        private IValueConverter _value3Converter = new ValueConverter();
        private IValueConverter _value4Converter = new AlphaConverter();

        public ColorPicker()
        {
            this.GetObservable(Value1Property).Subscribe(x => OnValueChange());
            this.GetObservable(Value2Property).Subscribe(x => OnValueChange());
            this.GetObservable(Value3Property).Subscribe(x => OnValueChange());
            this.GetObservable(Value4Property).Subscribe(x => OnValueChange());
        }

        public double Value1
        {
            get { return GetValue(Value1Property); }
            set { SetValue(Value1Property, value); }
        }

        public double Value2
        {
            get { return GetValue(Value2Property); }
            set { SetValue(Value2Property, value); }
        }

        public double Value3
        {
            get { return GetValue(Value3Property); }
            set { SetValue(Value3Property, value); }
        }

        public double Value4
        {
            get { return GetValue(Value4Property); }
            set { SetValue(Value4Property, value); }
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
            OnValueChange();
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

        private T Convert<T>(IValueConverter converter, T value, T param)
        {
            return (T)converter.Convert(value, typeof(T), param, CultureInfo.CurrentCulture);
        }

        private T ConvertBack<T>(IValueConverter converter, T value, T param)
        {
            return (T)converter.ConvertBack(value, typeof(T), param, CultureInfo.CurrentCulture);
        }

        private double GetValue1Range() => _hueCanvas.Bounds.Height;

        private double GetValue2Range() => _colorCanvas.Bounds.Width;

        private double GetValue3Range() => _colorCanvas.Bounds.Height;

        private double GetValue4Range() => _alphaCanvas.Bounds.Width;

        private void UpdateThumbsFromValues()
        {
            double hueY = Convert(_value1Converter, Value1, GetValue1Range());
            double colorX = Convert(_value2Converter, Value2, GetValue2Range());
            double colorY = Convert(_value3Converter, Value3, GetValue3Range());
            double alphaX = Convert(_value4Converter, Value4, GetValue4Range());
            MoveThumb(_hueCanvas, _hueThumb, 0, hueY);
            MoveThumb(_colorCanvas, _colorThumb, colorX, colorY);
            MoveThumb(_alphaCanvas, _alphaThumb, alphaX, 0);
        }

        private void UpdateValuesFromThumbs()
        {
            double hueY = Canvas.GetTop(_hueThumb);
            double colorX = Canvas.GetLeft(_colorThumb);
            double colorY = Canvas.GetTop(_colorThumb);
            double alphaX = Canvas.GetLeft(_alphaThumb);
            Value1 = ConvertBack(_value1Converter, hueY, GetValue1Range());
            Value2 = ConvertBack(_value2Converter, colorX, GetValue2Range());
            Value3 = ConvertBack(_value3Converter, colorY, GetValue3Range());
            Value4 = ConvertBack(_value4Converter, alphaX, GetValue4Range());
        }

        private void OnValueChange()
        {
            if (_updating == false && IsTemplateValid())
            {
                _updating = true;
                UpdateThumbsFromValues();
                UpdateValuesFromThumbs();
                _updating = false;
            }
        }
        private void ColorCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_colorCanvas);
                _updating = true;
                MoveThumb(_colorCanvas, _colorThumb, position.X, position.Y);
                UpdateValuesFromThumbs();
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
                _updating = true;
                MoveThumb(_colorCanvas, _colorThumb, position.X, position.Y);
                UpdateValuesFromThumbs();
                _updating = false;
            }
        }

        private void ColorThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_colorThumb);
            double top = Canvas.GetTop(_colorThumb);
            _updating = true;
            MoveThumb(_colorCanvas, _colorThumb, left + e.Vector.X, top + e.Vector.Y);
            UpdateValuesFromThumbs();
            _updating = false;
        }

        private void HueCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_hueCanvas);
                _updating = true;
                MoveThumb(_hueCanvas, _hueThumb, 0, position.Y);
                UpdateValuesFromThumbs();
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
                _updating = true;
                MoveThumb(_hueCanvas, _hueThumb, 0, position.Y);
                UpdateValuesFromThumbs();
                _updating = false;
            }
        }

        private void HueThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double top = Canvas.GetTop(_hueThumb);
            _updating = true;
            MoveThumb(_hueCanvas, _hueThumb, 0, top + e.Vector.Y);
            UpdateValuesFromThumbs();
            _updating = false;
        }

        private void AlphaCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == MouseButton.Left)
            {
                var position = e.GetPosition(_alphaCanvas);
                _updating = true;
                MoveThumb(_alphaCanvas, _alphaThumb, position.X, 0);
                UpdateValuesFromThumbs();
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
                _updating = true;
                MoveThumb(_alphaCanvas, _alphaThumb, position.X, 0);
                UpdateValuesFromThumbs();
                _updating = false;
            }
        }

        private void AlphaThumb_DragDelta(object sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(_alphaThumb);
            _updating = true;
            MoveThumb(_alphaCanvas, _alphaThumb, left + e.Vector.X, 0);
            UpdateValuesFromThumbs();
            _updating = false;
        }
    }
}
