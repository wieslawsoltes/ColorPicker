﻿using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using ThemeEditor.Controls.ColorPicker.Converters;

namespace ThemeEditor.Controls.ColorPicker;

public class ColorPickerAreaSlider : TemplatedControl
{
    public static readonly StyledProperty<double?> Value2Property =
        AvaloniaProperty.Register<ColorPickerAreaSlider, double?>(nameof(Value2));

    public static readonly StyledProperty<double?> Value3Property =
        AvaloniaProperty.Register<ColorPickerAreaSlider, double?>(nameof(Value3));

    private Canvas? _colorCanvas;
    private Thumb? _colorThumb;
    private bool _updating;
    private bool _captured;
    private readonly IValueConverter _value2Converter = SaturationConverter.Instance;
    private readonly IValueConverter _value3Converter = ValueConverter.Instance;

    public double? Value2
    {
        get => GetValue(Value2Property);
        set => SetValue(Value2Property, value);
    }

    public double? Value3
    {
        get => GetValue(Value3Property);
        set => SetValue(Value3Property, value);
    }

    public double GetValue2Range() => _colorCanvas?.Bounds.Width ?? 0.0;

    public double GetValue3Range() => _colorCanvas?.Bounds.Height ?? 0.0;

    public bool IsTemplateValid()
    {
        return _colorCanvas != null
               && _colorThumb != null;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

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

        _colorCanvas = e.NameScope.Find<Canvas>("PART_ColorCanvas");
        _colorThumb = e.NameScope.Find<Thumb>("PART_ColorThumb");

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

    private void ColorCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var position = e.GetPosition(_colorCanvas);
        _updating = true;
        MoveThumb(_colorCanvas, _colorThumb, position.X, position.Y);
        UpdateValuesFromThumbs();
        //UpdateColorFromThumbs();
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
            //UpdateColorFromThumbs();
            _updating = false;
        }
    }

    private void ColorThumb_DragDelta(object? sender, VectorEventArgs e)
    {
        if (_colorThumb is null)
        {
            return;
        }
        var left = Canvas.GetLeft(_colorThumb);
        var top = Canvas.GetTop(_colorThumb);
        _updating = true;
        MoveThumb(_colorCanvas, _colorThumb, left + e.Vector.X, top + e.Vector.Y);
        UpdateValuesFromThumbs();
        //UpdateColorFromThumbs();
        _updating = false;
    }
    
    private void UpdateThumbsFromValues()
    {
        var colorX = Convert(_value2Converter, Value2, GetValue2Range());
        var colorY = Convert(_value3Converter, Value3, GetValue3Range());
        MoveThumb(_colorCanvas, _colorThumb, colorX ?? 0.0, colorY ?? 0.0);
    }

    private T? Convert<T>(IValueConverter converter, T? value, T? range)
    {
        var converted = converter.Convert(value, typeof(T), range, CultureInfo.CurrentCulture);
        if (converted == AvaloniaProperty.UnsetValue)
        {
            return default;
        }
        var result = (T?)converted;
        return result;
    }

    private T? ConvertBack<T>(IValueConverter converter, T? value, T? range)
    {
        var converted = converter.ConvertBack(value, typeof(T), range, CultureInfo.CurrentCulture);
        if (converted == AvaloniaProperty.UnsetValue)
        {
            return default;
        }
        var result = (T?)converted;
        return result;
    }

    private void UpdateValuesFromThumbs()
    {
        if (_colorThumb is null)
        {
            return;
        }
        var colorX = Canvas.GetLeft(_colorThumb);
        var colorY = Canvas.GetTop(_colorThumb);
        Value2 = ConvertBack(_value2Converter, colorX, GetValue2Range());
        Value3 = ConvertBack(_value3Converter, colorY, GetValue3Range());
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == Value2Property || change.Property == Value3Property || change.Property == BoundsProperty)
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
