using System;
using Avalonia;

namespace ThemeEditor.Controls.ColorPicker;

public abstract class ColorPickerProperties : AvaloniaObject
{
    public static readonly StyledProperty<object?> HeaderProperty =
        AvaloniaProperty.Register<ColorPickerProperties, object?>(nameof(Header));

    public static readonly StyledProperty<ColorPickerPresenter?> PresenterProperty =
        AvaloniaProperty.Register<ColorPickerProperties, ColorPickerPresenter?>(nameof(Presenter));

    public ColorPickerProperties()
    {
        this.GetObservable(PresenterProperty).Subscribe(_ => OnColorPickerChange());
    }

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public ColorPickerPresenter? Presenter
    {
        get => GetValue(PresenterProperty);
        set => SetValue(PresenterProperty, value);
    }

    protected abstract void UpdateColorPickerValues();

    public abstract void UpdatePropertyValues();

    protected virtual void OnColorPickerChange()
    {
        Presenter?.GetObservable(ColorPickerPresenter.Value1Property).Subscribe(_ => UpdatePropertyValues());
        Presenter?.GetObservable(ColorPickerPresenter.Value2Property).Subscribe(_ => UpdatePropertyValues());
        Presenter?.GetObservable(ColorPickerPresenter.Value3Property).Subscribe(_ => UpdatePropertyValues());
        Presenter?.GetObservable(ColorPickerPresenter.Value4Property).Subscribe(_ => UpdatePropertyValues());
    }
}
