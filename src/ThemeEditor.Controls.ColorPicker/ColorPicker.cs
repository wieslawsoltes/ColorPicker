using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using ThemeEditor.Controls.ColorPicker.Props;

namespace ThemeEditor.Controls.ColorPicker;

public class ColorPicker : TemplatedControl
{
    public static readonly StyledProperty<Color> ColorProperty =
        AvaloniaProperty.Register<ColorPicker, Color>(nameof(Color));

    private ColorPickerPropertiesPresenter? _propertiesPresenter;
    private ColorPickerValuesPresenter? _valuesPresenter;
    private CompositeDisposable? _disposable;

    public Color Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _disposable?.Dispose();
        _disposable = new CompositeDisposable();

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
            _valuesPresenter.GetObservable(ColorPickerValuesPresenter.Value1Property)
                .Subscribe(_ => _valuesPresenter.OnValueChange())
                .DisposeWith(_disposable);

            _valuesPresenter.GetObservable(ColorPickerValuesPresenter.Value2Property)
                .Subscribe(_ => _valuesPresenter.OnValueChange())
                .DisposeWith(_disposable);

            _valuesPresenter.GetObservable(ColorPickerValuesPresenter.Value3Property)
                .Subscribe(_ => _valuesPresenter.OnValueChange())
                .DisposeWith(_disposable);

            _valuesPresenter.GetObservable(ColorPickerValuesPresenter.Value4Property)
                .Subscribe(_ => _valuesPresenter.OnValueChange())
                .DisposeWith(_disposable);

            _valuesPresenter._colorPicker = this;

            this.GetObservable(ColorProperty)
                .Subscribe(_ => _valuesPresenter.OnColorChange())
                .DisposeWith(_disposable);
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var size = base.ArrangeOverride(finalSize);

        _valuesPresenter?.OnColorChange();

        return size;
    }
}
