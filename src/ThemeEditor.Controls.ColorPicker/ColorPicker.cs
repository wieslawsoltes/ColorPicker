using System;
using System.Collections.Generic;
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

    public static readonly StyledProperty<IEnumerable<ColorPickerProperties>> PropertiesProperty = 
        AvaloniaProperty.Register<ColorPicker, IEnumerable<ColorPickerProperties>>(nameof(Properties));

    private ColorPickerPresenter? _colorPickerPresenter;
    private CompositeDisposable? _disposable;

    public Color Color
    {
        get { return GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

    public IEnumerable<ColorPickerProperties> Properties
    {
        get => GetValue(PropertiesProperty);
        set => SetValue(PropertiesProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _disposable?.Dispose();

        _colorPickerPresenter = e.NameScope.Find<ColorPickerPresenter>("PART_ColorPickerPresenter");

        if (_colorPickerPresenter is { })
        {
            _disposable = new CompositeDisposable();

            Properties = new AvaloniaList<ColorPickerProperties>
            {
                new HexProperties { Header = "Hex", Presenter = _colorPickerPresenter },
                new AlphaProperties { Header = "Alpha", Presenter = _colorPickerPresenter },
                new RgbProperties { Header = "RGB", Presenter = _colorPickerPresenter },
                new HsvProperties { Header = "HSV", Presenter = _colorPickerPresenter },
                new CmykProperties { Header = "CMYK", Presenter = _colorPickerPresenter }
            };

            _colorPickerPresenter.GetObservable(ColorPickerPresenter.Value1Property)
                .Subscribe(_ => _colorPickerPresenter.OnValueChange())
                .DisposeWith(_disposable);

            _colorPickerPresenter.GetObservable(ColorPickerPresenter.Value2Property)
                .Subscribe(_ => _colorPickerPresenter.OnValueChange())
                .DisposeWith(_disposable);

            _colorPickerPresenter.GetObservable(ColorPickerPresenter.Value3Property)
                .Subscribe(_ => _colorPickerPresenter.OnValueChange())
                .DisposeWith(_disposable);

            _colorPickerPresenter.GetObservable(ColorPickerPresenter.Value4Property)
                .Subscribe(_ => _colorPickerPresenter.OnValueChange())
                .DisposeWith(_disposable);

            _colorPickerPresenter._colorPicker = this;

            this.GetObservable(ColorProperty)
                .Subscribe(_ => _colorPickerPresenter.OnColorChange())
                .DisposeWith(_disposable);
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var size = base.ArrangeOverride(finalSize);

        _colorPickerPresenter?.OnColorChange();

        return size;
    }
}
