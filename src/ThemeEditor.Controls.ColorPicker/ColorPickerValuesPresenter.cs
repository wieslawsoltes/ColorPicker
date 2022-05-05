using System;
using System.Globalization;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Media;
using ThemeEditor.Controls.ColorPicker.Converters;

namespace ThemeEditor.Controls.ColorPicker;

public class ColorPickerValuesPresenter : TemplatedControl
{
    public static readonly StyledProperty<double> Value1Property =
        AvaloniaProperty.Register<ColorPickerValuesPresenter, double>(nameof(Value1));

    public static readonly StyledProperty<double> Value2Property =
        AvaloniaProperty.Register<ColorPickerValuesPresenter, double>(nameof(Value2));

    public static readonly StyledProperty<double> Value3Property =
        AvaloniaProperty.Register<ColorPickerValuesPresenter, double>(nameof(Value3));

    public static readonly StyledProperty<double> Value4Property =
        AvaloniaProperty.Register<ColorPickerValuesPresenter, double>(nameof(Value4));

    private CompositeDisposable? _disposable;
    private Canvas? _colorCanvas;
    private Thumb? _colorThumb;
    private Canvas? _hueCanvas;
    private Thumb? _hueThumb;
    private Canvas? _alphaCanvas;
    private Thumb? _alphaThumb;
    private bool _updating;
    private bool _captured;
    private readonly IValueConverter _value1Converter = new HueConverter();
    private readonly IValueConverter _value2Converter = new SaturationConverter();
    private readonly IValueConverter _value3Converter = new ValueConverter();
    private readonly IValueConverter _value4Converter = new AlphaConverter();
    internal ColorPicker? _colorPicker;

    public double Value1
    {
        get => GetValue(Value1Property);
        set => SetValue(Value1Property, value);
    }

    public double Value2
    {
        get => GetValue(Value2Property);
        set => SetValue(Value2Property, value);
    }

    public double Value3
    {
        get => GetValue(Value3Property);
        set => SetValue(Value3Property, value);
    }

    public double Value4
    {
        get => GetValue(Value4Property);
        set => SetValue(Value4Property, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _disposable?.Dispose();

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

        _disposable = new CompositeDisposable();

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
    }

    private void MoveThumb(Canvas? canvas, Thumb? thumb, double x, double y)
    {
        if (canvas != null && thumb != null)
        {
            var left = ColorPickerHelpers.Clamp(x, 0, canvas.Bounds.Width);
            var top = ColorPickerHelpers.Clamp(y, 0, canvas.Bounds.Height);
            Canvas.SetLeft(thumb, left);
            Canvas.SetTop(thumb, top); 
        }
    }

    private T? Convert<T>(IValueConverter converter, T? value, T? range)
    {
        return (T?)converter.Convert(value, typeof(T), range, CultureInfo.CurrentCulture);
    }

    private T? ConvertBack<T>(IValueConverter converter, T? value, T? range)
    {
        return (T?)converter.ConvertBack(value, typeof(T), range, CultureInfo.CurrentCulture);
    }

    private double GetValue1Range() => _hueCanvas?.Bounds.Height ?? 0.0;

    private double GetValue2Range() => _colorCanvas?.Bounds.Width ?? 0.0;

    private double GetValue3Range() => _colorCanvas?.Bounds.Height ?? 0.0;

    private double GetValue4Range() => _alphaCanvas?.Bounds.Width ?? 0.0;

    private void UpdateThumbsFromColor()
    {
        ColorPickerHelpers.FromColor(GetColor(), out var h, out var s, out var v, out var a);
        var hueY = Convert(_value1Converter, h, GetValue1Range());
        var colorX = Convert(_value2Converter, s, GetValue2Range());
        var colorY = Convert(_value3Converter, v, GetValue3Range());
        var alphaX = Convert(_value4Converter, a, GetValue4Range());
        MoveThumb(_hueCanvas, _hueThumb, 0, hueY);
        MoveThumb(_colorCanvas, _colorThumb, colorX, colorY);
        MoveThumb(_alphaCanvas, _alphaThumb, alphaX, 0);
    }

    private void UpdateThumbsFromValues()
    {
        var hueY = Convert(_value1Converter, Value1, GetValue1Range());
        var colorX = Convert(_value2Converter, Value2, GetValue2Range());
        var colorY = Convert(_value3Converter, Value3, GetValue3Range());
        var alphaX = Convert(_value4Converter, Value4, GetValue4Range());

        MoveThumb(_hueCanvas, _hueThumb, 0, hueY);
        MoveThumb(_colorCanvas, _colorThumb, colorX, colorY);
        MoveThumb(_alphaCanvas, _alphaThumb, alphaX, 0);
    }

    private void UpdateValuesFromThumbs()
    {
        var hueY = Canvas.GetTop(_hueThumb);
        var colorX = Canvas.GetLeft(_colorThumb);
        var colorY = Canvas.GetTop(_colorThumb);
        var alphaX = Canvas.GetLeft(_alphaThumb);
        Value1 = ConvertBack(_value1Converter, hueY, GetValue1Range());
        Value2 = ConvertBack(_value2Converter, colorX, GetValue2Range());
        Value3 = ConvertBack(_value3Converter, colorY, GetValue3Range());
        Value4 = ConvertBack(_value4Converter, alphaX, GetValue4Range());
        SetColor(Value1, Value2, Value3, Value4);
    }
 
    private void UpdateColorFromThumbs()
    {
        var hueY = Canvas.GetTop(_hueThumb);
        var colorX = Canvas.GetLeft(_colorThumb);
        var colorY = Canvas.GetTop(_colorThumb);
        var alphaX = Canvas.GetLeft(_alphaThumb);
        var h = ConvertBack(_value1Converter, hueY, GetValue1Range());
        var s = ConvertBack(_value2Converter, colorX, GetValue2Range());
        var v = ConvertBack(_value3Converter, colorY, GetValue3Range());
        var a = ConvertBack(_value4Converter, alphaX, GetValue4Range());
        SetColor(h, s, v, a);
    }

    private Color GetColor()
    {
        return _colorPicker?.Color ?? Avalonia.Media.Colors.Black;
    }

    private void SetColor(double h, double s, double v, double a)
    {
        if (_colorPicker is { })
        {
            _colorPicker.Color = ColorPickerHelpers.FromHSVA(h, s, v, a);
        }
    }

    internal void OnValueChange()
    {
        if (_updating == false && IsTemplateValid())
        {
            _updating = true;
            UpdateThumbsFromValues();
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            _updating = false;
        }
    }

    internal void OnColorChange()
    {
        if (_updating == false && IsTemplateValid())
        {
            _updating = true;
            UpdateThumbsFromColor();
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            _updating = false;
        }
    }

    private void ColorCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var position = e.GetPosition(_colorCanvas);
        _updating = true;
        MoveThumb(_colorCanvas, _colorThumb, position.X, position.Y);
        UpdateValuesFromThumbs();
        UpdateColorFromThumbs();
        if (_colorCanvas is { } && _colorThumb is { })
        {
            _colorCanvas.Cursor = new Cursor(StandardCursorType.None);
            _colorThumb.Cursor = new Cursor(StandardCursorType.None);
        }
        _updating = false;
        _captured = true;
    }

    private void ColorCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_captured)
        {
            if (_colorCanvas is { } && _colorThumb is { })
            {
                _colorCanvas.Cursor = Cursor.Default;
                _colorThumb.Cursor = Cursor.Default;
            }
            _captured = false;
        }
    }

    private void ColorCanvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (_captured)
        {
            var position = e.GetPosition(_colorCanvas);
            _updating = true;
            MoveThumb(_colorCanvas, _colorThumb, position.X, position.Y);
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            _updating = false;
        }
    }

    private void ColorThumb_DragDelta(object? sender, VectorEventArgs e)
    {
        var left = Canvas.GetLeft(_colorThumb);
        var top = Canvas.GetTop(_colorThumb);
        _updating = true;
        MoveThumb(_colorCanvas, _colorThumb, left + e.Vector.X, top + e.Vector.Y);
        UpdateValuesFromThumbs();
        UpdateColorFromThumbs();
        _updating = false;
    }

    private void HueCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var position = e.GetPosition(_hueCanvas);
        _updating = true;
        MoveThumb(_hueCanvas, _hueThumb, 0, position.Y);
        UpdateValuesFromThumbs();
        UpdateColorFromThumbs();
        if (_hueCanvas is { } && _hueThumb is { })
        {
            _hueCanvas.Cursor = new Cursor(StandardCursorType.SizeNorthSouth);
            _hueThumb.Cursor = new Cursor(StandardCursorType.SizeNorthSouth);
        }
        _updating = false;
        _captured = true;
    }

    private void HueCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_captured)
        {
            if (_hueCanvas is { } && _hueThumb is { })
            {
                _hueCanvas.Cursor = Cursor.Default;
                _hueThumb.Cursor = Cursor.Default;
            }
            _captured = false;
        }
    }

    private void HueCanvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (_captured)
        {
            var position = e.GetPosition(_hueCanvas);
            _updating = true;
            MoveThumb(_hueCanvas, _hueThumb, 0, position.Y);
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            _updating = false;
        }
    }

    private void HueThumb_DragDelta(object? sender, VectorEventArgs e)
    {
        var top = Canvas.GetTop(_hueThumb);
        _updating = true;
        MoveThumb(_hueCanvas, _hueThumb, 0, top + e.Vector.Y);
        UpdateValuesFromThumbs();
        UpdateColorFromThumbs();
        _updating = false;
    }

    private void AlphaCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var position = e.GetPosition(_alphaCanvas);
        _updating = true;
        MoveThumb(_alphaCanvas, _alphaThumb, position.X, 0);
        UpdateValuesFromThumbs();
        UpdateColorFromThumbs();
        if (_alphaCanvas is { } && _alphaThumb is { })
        {
            _alphaCanvas.Cursor = new Cursor(StandardCursorType.SizeWestEast);
            _alphaThumb.Cursor = new Cursor(StandardCursorType.SizeWestEast);
        }
        _updating = false;
        _captured = true;
    }

    private void AlphaCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_captured)
        {
            if (_alphaCanvas is { } && _alphaThumb is { })
            {
                _alphaCanvas.Cursor = Cursor.Default;
                _alphaThumb.Cursor = Cursor.Default;
            }
            _captured = false;
        }
    }

    private void AlphaCanvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (_captured)
        {
            var position = e.GetPosition(_alphaCanvas);
            _updating = true;
            MoveThumb(_alphaCanvas, _alphaThumb, position.X, 0);
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            _updating = false;
        }
    }

    private void AlphaThumb_DragDelta(object? sender, VectorEventArgs e)
    {
        var left = Canvas.GetLeft(_alphaThumb);
        _updating = true;
        MoveThumb(_alphaCanvas, _alphaThumb, left + e.Vector.X, 0);
        UpdateValuesFromThumbs();
        UpdateColorFromThumbs();
        _updating = false;
    }
}
