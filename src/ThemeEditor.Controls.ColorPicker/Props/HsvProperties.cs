using Avalonia;
using Avalonia.Reactive;

namespace ThemeEditor.Controls.ColorPicker.Props;

public class HsvProperties : ColorPickerProperties
{
    public static readonly StyledProperty<double?> HueProperty =
        AvaloniaProperty.Register<HsvProperties, double?>(nameof(Hue), 0.0);

    public static readonly StyledProperty<double?> SaturationProperty =
        AvaloniaProperty.Register<HsvProperties, double?>(nameof(Saturation), 100.0);

    public static readonly StyledProperty<double?> ValueProperty =
        AvaloniaProperty.Register<HsvProperties, double?>(nameof(Value), 100.0);

    private bool _updating;

    public HsvProperties()
    {
        this.GetObservable(HueProperty).Subscribe(new AnonymousObserver<double?>(_ => UpdateColorPickerValues()));
        this.GetObservable(SaturationProperty).Subscribe(new AnonymousObserver<double?>(_ => UpdateColorPickerValues()));
        this.GetObservable(ValueProperty).Subscribe(new AnonymousObserver<double?>(_ => UpdateColorPickerValues()));
    }

    public double? Hue
    {
        get => GetValue(HueProperty);
        set => SetValue(HueProperty, value);
    }

    public double? Saturation
    {
        get => GetValue(SaturationProperty);
        set => SetValue(SaturationProperty, value);
    }

    public double? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    protected override void UpdateColorPickerValues()
    {
        if (_updating == false && Presenter != null)
        {
            _updating = true;
            Presenter.Value1 = Hue;
            Presenter.Value2 = Saturation;
            Presenter.Value3 = Value;
            _updating = false;
        }
    }

    protected override void UpdatePropertyValues()
    {
        if (_updating == false && Presenter != null)
        {
            _updating = true;
            Hue = Presenter.Value1;
            Saturation = Presenter.Value2;
            Value = Presenter.Value3;
            _updating = false;
        }
    }
}
