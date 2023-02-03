using System;
using System.Globalization;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Reactive;
using ThemeEditor.Controls.ColorPicker.Props;

namespace ThemeEditor.Controls.ColorPicker;

public class ColorPicker : TemplatedControl
{
    public static NumberFormatInfo NumberFormat = CultureInfo.InvariantCulture.NumberFormat;

    public static readonly StyledProperty<Color?> ColorProperty =
        AvaloniaProperty.Register<ColorPicker, Color?>(nameof(Color));

    private ColorPickerPropertiesPresenter? _propertiesPresenter;
    private ColorPickerValuesPresenter? _valuesPresenter;
    private IDisposable? _value1Disposable;
    private IDisposable? _value2Disposable;
    private IDisposable? _value3Disposable;
    private IDisposable? _value4Disposable;
    private IDisposable? _colorDisposable;

    public Color? Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _value1Disposable?.Dispose();
        _value2Disposable?.Dispose();
        _value3Disposable?.Dispose();
        _value4Disposable?.Dispose();
        _colorDisposable?.Dispose();

        _propertiesPresenter = e.NameScope.Find<ColorPickerPropertiesPresenter>("PART_ColorPickerPropertiesPresenter");
        _valuesPresenter = e.NameScope.Find<ColorPickerValuesPresenter>("PART_ColorPickerValuesPresenter");

        if (_propertiesPresenter is { } && _valuesPresenter is { })
        {
            _propertiesPresenter.Properties = new AvaloniaList<ColorPickerProperties>
            {
                new HexProperties { Header = "Hex", Presenter = _valuesPresenter },
                new AlphaProperties { Header = "Alpha", Presenter = _valuesPresenter },
                new RgbProperties { Header = "RGB", Presenter = _valuesPresenter },
                new HsvProperties { Header = "HSV", Presenter = _valuesPresenter },
                new CmykProperties { Header = "CMYK", Presenter = _valuesPresenter }
            };
        }

        if (_valuesPresenter is { })
        {
            _value1Disposable = _valuesPresenter
                .GetObservable(ColorPickerValuesPresenter.Value1Property)
                .Subscribe(new AnonymousObserver<double?>(_ => _valuesPresenter.OnValueChange()));

            _value2Disposable = _valuesPresenter
                .GetObservable(ColorPickerValuesPresenter.Value2Property)
                .Subscribe(new AnonymousObserver<double?>(_ => _valuesPresenter.OnValueChange()));

            _value3Disposable = _valuesPresenter
                .GetObservable(ColorPickerValuesPresenter.Value3Property)
                .Subscribe(new AnonymousObserver<double?>(_ => _valuesPresenter.OnValueChange()));

            _value4Disposable = _valuesPresenter
                    .GetObservable(ColorPickerValuesPresenter.Value4Property)
                    .Subscribe(new AnonymousObserver<double?>(_ => _valuesPresenter.OnValueChange()));

            _valuesPresenter._colorPicker = this;

            _colorDisposable = this
                .GetObservable(ColorProperty)
                .Subscribe(new AnonymousObserver<Color?>(_ => _valuesPresenter.OnColorChange()));
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var size = base.ArrangeOverride(finalSize);

        _valuesPresenter?.OnColorChange();

        return size;
    }
}
