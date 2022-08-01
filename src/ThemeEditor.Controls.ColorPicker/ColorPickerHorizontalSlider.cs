using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using ThemeEditor.Controls.ColorPicker.Converters;

namespace ThemeEditor.Controls.ColorPicker;

public class ColorPickerHorizontalSlider : TemplatedControl
{
    public static readonly StyledProperty<double?> Value4Property =
        AvaloniaProperty.Register<ColorPickerHorizontalSlider, double?>(nameof(Value4));

    private Canvas? _alphaCanvas;
    private Thumb? _alphaThumb;
    private bool _updating;
    private bool _captured;
    private readonly IValueConverter _value4Converter = AlphaConverter.Instance;

    public double? Value4
    {
        get => GetValue(Value4Property);
        set => SetValue(Value4Property, value);
    }

    public double GetValue4Range() => _alphaCanvas?.Bounds.Width ?? 0.0;

    private bool IsTemplateValid()
    {
        return _alphaCanvas != null
               && _alphaThumb != null;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
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

        _alphaCanvas = e.NameScope.Find<Canvas>("PART_AlphaCanvas");
        _alphaThumb = e.NameScope.Find<Thumb>("PART_AlphaThumb");

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

    private void AlphaCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var position = e.GetPosition(_alphaCanvas);
        _updating = true;
        MoveThumb(_alphaCanvas, _alphaThumb, position.X, 0);
        UpdateValuesFromThumbs();
        //UpdateColorFromThumbs();
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
            //UpdateColorFromThumbs();
            _updating = false;
        }
    }

    private void AlphaThumb_DragDelta(object? sender, VectorEventArgs e)
    {
        var left = Canvas.GetLeft(_alphaThumb);
        _updating = true;
        MoveThumb(_alphaCanvas, _alphaThumb, left + e.Vector.X, 0);
        UpdateValuesFromThumbs();
        //UpdateColorFromThumbs();
        _updating = false;
    }

    private void UpdateThumbsFromValues()
    {
        var alphaX = Value4;
        MoveThumb(_alphaCanvas, _alphaThumb, alphaX ?? 0.0, 0.0);
    }

    private T? ConvertBack<T>(IValueConverter converter, T? value, T? range)
    {
        return (T?)converter.ConvertBack(value, typeof(T), range, CultureInfo.CurrentCulture);
    }

    private void UpdateValuesFromThumbs()
    {
        var alphaX = Canvas.GetLeft(_alphaThumb);
        Value4 = ConvertBack(_value4Converter, alphaX, GetValue4Range());
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == Value4Property)
        {
            if (_updating == false && IsTemplateValid())
            {
                _updating = true;
                UpdateThumbsFromValues();
                UpdateValuesFromThumbs();
                //UpdateColorFromThumbs();
                _updating = false;
            }
        }
    }
}
