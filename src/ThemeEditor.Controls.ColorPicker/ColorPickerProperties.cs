using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls.Mixins;

namespace ThemeEditor.Controls.ColorPicker;

public abstract class ColorPickerProperties : AvaloniaObject
{
    public static readonly StyledProperty<object?> HeaderProperty =
        AvaloniaProperty.Register<ColorPickerProperties, object?>(nameof(Header));

    public static readonly StyledProperty<ColorPickerValuesPresenter?> PresenterProperty =
        AvaloniaProperty.Register<ColorPickerProperties, ColorPickerValuesPresenter?>(nameof(Presenter));

    private CompositeDisposable? _disposable;
    
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

    protected abstract void UpdatePropertyValues();

    protected virtual void OnColorPickerChange()
    {
        _disposable?.Dispose();
        _disposable = new CompositeDisposable();

        if (Presenter is { })
        {
            Presenter.GetObservable(ColorPickerValuesPresenter.Value1Property)
                .Subscribe(_ => UpdatePropertyValues())
                .DisposeWith(_disposable);

            Presenter.GetObservable(ColorPickerValuesPresenter.Value2Property)
                .Subscribe(_ => UpdatePropertyValues())
                .DisposeWith(_disposable);

            Presenter.GetObservable(ColorPickerValuesPresenter.Value3Property)
                .Subscribe(_ => UpdatePropertyValues())
                .DisposeWith(_disposable);

            Presenter.GetObservable(ColorPickerValuesPresenter.Value4Property)
                .Subscribe(_ => UpdatePropertyValues())
                .DisposeWith(_disposable);
        }
    }
}
