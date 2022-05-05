using System;
using Avalonia;

namespace ThemeEditor.Controls.ColorPicker;

public abstract class ColorPickerProperties : AvaloniaObject
{
    public static readonly StyledProperty<object?> HeaderProperty =
        AvaloniaProperty.Register<ColorPickerProperties, object?>(nameof(Header));

    public static readonly StyledProperty<ColorPickerValuesPresenter?> PresenterProperty =
        AvaloniaProperty.Register<ColorPickerProperties, ColorPickerValuesPresenter?>(nameof(Presenter));

    public ColorPickerProperties()
    {
        this.GetObservable(PresenterProperty).Subscribe(_ => OnColorPickerChange());
    }

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public ColorPickerValuesPresenter? Presenter
    {
        get => GetValue(PresenterProperty);
        set => SetValue(PresenterProperty, value);
    }

    protected abstract void UpdateColorPickerValues();

    public abstract void UpdatePropertyValues();

    protected virtual void OnColorPickerChange()
    {
        Presenter?.GetObservable(ColorPickerValuesPresenter.Value1Property).Subscribe(_ => UpdatePropertyValues());
        Presenter?.GetObservable(ColorPickerValuesPresenter.Value2Property).Subscribe(_ => UpdatePropertyValues());
        Presenter?.GetObservable(ColorPickerValuesPresenter.Value3Property).Subscribe(_ => UpdatePropertyValues());
        Presenter?.GetObservable(ColorPickerValuesPresenter.Value4Property).Subscribe(_ => UpdatePropertyValues());
    }
}
