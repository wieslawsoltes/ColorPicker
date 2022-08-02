using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using ThemeEditor.Controls.ColorPicker.Converters;

namespace ThemeEditor.Controls.ColorPicker;

public class ColorPickerVerticalSlider : TemplatedControl
{
    public static readonly StyledProperty<double?> Value1Property =
        AvaloniaProperty.Register<ColorPickerVerticalSlider, double?>(nameof(Value1));

    private Canvas? _hueCanvas;
    private Thumb? _hueThumb;
    private bool _updating;
    private bool _captured;
    private readonly IValueConverter _value1Converter = HueConverter.Instance;

    public double? Value1
    {
        get => GetValue(Value1Property);
        set => SetValue(Value1Property, value);
    }

    public double GetValue1Range() => _hueCanvas?.Bounds.Height ?? 0.0;

    public bool IsTemplateValid()
    {
        return _hueCanvas != null
               && _hueThumb != null;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

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
        
        _hueCanvas = e.NameScope.Find<Canvas>("PART_HueCanvas");
        _hueThumb = e.NameScope.Find<Thumb>("PART_HueThumb");
        
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

    private void HueCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var position = e.GetPosition(_hueCanvas);
        _updating = true;
        MoveThumb(_hueCanvas, _hueThumb, 0, position.Y);
        UpdateValuesFromThumbs();
        //UpdateColorFromThumbs();
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
            //UpdateColorFromThumbs();
            _updating = false;
        }
    }

    private void HueThumb_DragDelta(object? sender, VectorEventArgs e)
    {
        var top = Canvas.GetTop(_hueThumb);
        _updating = true;
        MoveThumb(_hueCanvas, _hueThumb, 0, top + e.Vector.Y);
        UpdateValuesFromThumbs();
        //UpdateColorFromThumbs();
        _updating = false;
    }

    private void UpdateThumbsFromValues()
    {
        var hueY = Convert(_value1Converter, Value1, GetValue1Range());
        MoveThumb(_hueCanvas, _hueThumb, 0, hueY ?? 0.0);
    }

    private T? Convert<T>(IValueConverter converter, T? value, T? range)
    {
        return (T?)converter.Convert(value, typeof(T), range, CultureInfo.CurrentCulture);
    }

    private T? ConvertBack<T>(IValueConverter converter, T? value, T? range)
    {
        return (T?)converter.ConvertBack(value, typeof(T), range, CultureInfo.CurrentCulture);
    }

    private void UpdateValuesFromThumbs()
    {
        var hueY = Canvas.GetTop(_hueThumb);
        Value1 = ConvertBack(_value1Converter, hueY, GetValue1Range());
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == Value1Property || change.Property == BoundsProperty)
        {
            if (_updating == false && IsTemplateValid())
            {
                _updating = true;
                UpdateThumbsFromValues();
                //UpdateValuesFromThumbs();
                //UpdateColorFromThumbs();
                _updating = false;
            }
        }
    }
}
