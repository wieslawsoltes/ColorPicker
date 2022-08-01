using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ThemeEditor.Controls.ColorPicker.Converters;

namespace ThemeEditor.Controls.ColorPicker;

public class ColorPickerValuesPresenter : TemplatedControl
{
    public static readonly StyledProperty<double?> Value1Property =
        AvaloniaProperty.Register<ColorPickerValuesPresenter, double?>(nameof(Value1));

    public static readonly StyledProperty<double?> Value2Property =
        AvaloniaProperty.Register<ColorPickerValuesPresenter, double?>(nameof(Value2));

    public static readonly StyledProperty<double?> Value3Property =
        AvaloniaProperty.Register<ColorPickerValuesPresenter, double?>(nameof(Value3));

    public static readonly StyledProperty<double?> Value4Property =
        AvaloniaProperty.Register<ColorPickerValuesPresenter, double?>(nameof(Value4));

    private bool _updating;
    private readonly IValueConverter _value1Converter = HueConverter.Instance;
    private readonly IValueConverter _value2Converter = SaturationConverter.Instance;
    private readonly IValueConverter _value3Converter = ValueConverter.Instance;
    private readonly IValueConverter _value4Converter = AlphaConverter.Instance;
    internal ColorPicker? _colorPicker;
    private ColorPickerAreaSlider? _colorPickerAreaSlider;
    private ColorPickerVerticalSlider? _colorPickerVerticalSlider;
    private ColorPickerHorizontalSlider? _colorPickerHorizontalSlider;

    public double? Value1
    {
        get => GetValue(Value1Property);
        set => SetValue(Value1Property, value);
    }

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

    public double? Value4
    {
        get => GetValue(Value4Property);
        set => SetValue(Value4Property, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _colorPickerAreaSlider = e.NameScope.Find<ColorPickerAreaSlider>("PART_ColorPickerAreaSlider");
        _colorPickerVerticalSlider = e.NameScope.Find<ColorPickerVerticalSlider>("PART_ColorPickerVerticalSlider");
        _colorPickerHorizontalSlider = e.NameScope.Find<ColorPickerHorizontalSlider>("PART_ColorPickerHorizontalSlider");
    }

    private bool IsTemplateValid()
    {
        return _colorPickerAreaSlider != null
               && _colorPickerVerticalSlider != null
               && _colorPickerHorizontalSlider != null;
    }

    private T? Convert<T>(IValueConverter converter, T? value, T? range)
    {
        return (T?)converter.Convert(value, typeof(T), range, CultureInfo.CurrentCulture);
    }

    private T? ConvertBack<T>(IValueConverter converter, T? value, T? range)
    {
        return (T?)converter.ConvertBack(value, typeof(T), range, CultureInfo.CurrentCulture);
    }

    private void UpdateThumbsFromColor()
    {
        ColorPickerHelpers.FromColor(GetColor(), out var h, out var s, out var v, out var a);
        var hueY = Convert(_value1Converter, h, _colorPickerVerticalSlider?.GetValue1Range());
        var colorX = Convert(_value2Converter, s, _colorPickerAreaSlider?.GetValue2Range());
        var colorY = Convert(_value3Converter, v, _colorPickerAreaSlider?.GetValue3Range());
        var alphaX = Convert(_value4Converter, a, _colorPickerHorizontalSlider?.GetValue4Range());
        Value1 = hueY;
        Value2 = colorX;
        Value3 = colorY;
        Value4 = alphaX;
    }

    private void UpdateColorFromThumbs()
    {
        var hueY = Value1;
        var colorX = Value2;
        var colorY = Value3;
        var alphaX = Value4;
        var h = ConvertBack(_value1Converter, hueY, _colorPickerVerticalSlider?.GetValue1Range());
        var s = ConvertBack(_value2Converter, colorX, _colorPickerAreaSlider?.GetValue2Range());
        var v = ConvertBack(_value3Converter, colorY, _colorPickerAreaSlider?.GetValue3Range());
        var a = ConvertBack(_value4Converter, alphaX, _colorPickerHorizontalSlider?.GetValue4Range());
  
        if (h is not null && s is not null && v is not null && a is not null)
        {
            SetColor(h.Value, s.Value, v.Value, a.Value);
        }
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
            //UpdateThumbsFromValues();
            //UpdateValuesFromThumbs();
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
            //UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            _updating = false;
        }
    }
}
