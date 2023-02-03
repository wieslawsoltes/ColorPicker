using System;
using Avalonia;
using Avalonia.Reactive;

namespace ThemeEditor.Controls.ColorPicker;

public abstract class ColorPickerProperties : AvaloniaObject
{
    public static readonly StyledProperty<object?> HeaderProperty =
        AvaloniaProperty.Register<ColorPickerProperties, object?>(nameof(Header));

    public static readonly StyledProperty<ColorPickerValuesPresenter?> PresenterProperty =
        AvaloniaProperty.Register<ColorPickerProperties, ColorPickerValuesPresenter?>(nameof(Presenter));

    private IDisposable? _value1Disposable;
    private IDisposable? _value2Disposable;
    private IDisposable? _value3Disposable;
    private IDisposable? _value4Disposable;
    
    public ColorPickerProperties()
    {
        this.GetObservable(PresenterProperty).Subscribe(
            new AnonymousObserver<ColorPickerValuesPresenter?>(_ => OnColorPickerChange()));
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

    protected abstract void UpdatePropertyValues();

    protected virtual void OnColorPickerChange()
    {
        _value1Disposable?.Dispose();
        _value2Disposable?.Dispose();
        _value3Disposable?.Dispose();
        _value4Disposable?.Dispose();

        if (Presenter is { })
        {
            _value1Disposable = Presenter
                .GetObservable(ColorPickerValuesPresenter.Value1Property)
                .Subscribe(new AnonymousObserver<double?>(_ => UpdatePropertyValues()));

            _value2Disposable = Presenter
                .GetObservable(ColorPickerValuesPresenter.Value2Property)
                .Subscribe(new AnonymousObserver<double?>(_ => UpdatePropertyValues()));

            _value3Disposable = Presenter
                .GetObservable(ColorPickerValuesPresenter.Value3Property)
                .Subscribe(new AnonymousObserver<double?>(_ => UpdatePropertyValues()));

            _value4Disposable = Presenter
                .GetObservable(ColorPickerValuesPresenter.Value4Property)
                .Subscribe(new AnonymousObserver<double?>(_ => UpdatePropertyValues()));
        }
    }
}
