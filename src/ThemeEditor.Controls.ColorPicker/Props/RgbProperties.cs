using Avalonia;
using Avalonia.Reactive;
using ThemeEditor.Controls.ColorPicker.Colors;

namespace ThemeEditor.Controls.ColorPicker.Props;

public class RgbProperties : ColorPickerProperties
{
    public static readonly StyledProperty<byte?> RedProperty =
        AvaloniaProperty.Register<RgbProperties, byte?>(nameof(Red), 0xFF);

    public static readonly StyledProperty<byte?> GreenProperty =
        AvaloniaProperty.Register<RgbProperties, byte?>(nameof(Green));

    public static readonly StyledProperty<byte?> BlueProperty =
        AvaloniaProperty.Register<RgbProperties, byte?>(nameof(Blue));

    private bool _updating;

    public RgbProperties()
    {
        this.GetObservable(RedProperty).Subscribe(new AnonymousObserver<byte?>(_ => UpdateColorPickerValues()));
        this.GetObservable(GreenProperty).Subscribe(new AnonymousObserver<byte?>(_ => UpdateColorPickerValues()));
        this.GetObservable(BlueProperty).Subscribe(new AnonymousObserver<byte?>(_ => UpdateColorPickerValues()));
    }

    public byte? Red
    {
        get => GetValue(RedProperty);
        set => SetValue(RedProperty, value);
    }

    public byte? Green
    {
        get => GetValue(GreenProperty);
        set => SetValue(GreenProperty, value);
    }

    public byte? Blue
    {
        get => GetValue(BlueProperty);
        set => SetValue(BlueProperty, value);
    }

    protected override void UpdateColorPickerValues()
    {
        if (_updating == false && Presenter != null)
        {
            _updating = true;
            var rgb = new RGB(Red ?? 0x00, Green ?? 0x00, Blue ?? 0x00);
            var hsv = rgb.ToHSV();
            Presenter.SetCurrentValue(ColorPickerValuesPresenter.Value1Property, hsv.H);
            Presenter.SetCurrentValue(ColorPickerValuesPresenter.Value2Property, hsv.S);
            Presenter.SetCurrentValue(ColorPickerValuesPresenter.Value3Property, hsv.V);
            _updating = false;
        }
    }

    protected override void UpdatePropertyValues()
    {
        if (_updating == false && Presenter != null)
        {
            _updating = true;
            var hsv = new HSV(Presenter.Value1 ?? 0x00, Presenter.Value2 ?? 0x00, Presenter.Value3 ?? 0x00);
            var rgb = hsv.ToRGB();
            SetCurrentValue(RedProperty, (byte)rgb.R);
            SetCurrentValue(GreenProperty, (byte)rgb.G);
            SetCurrentValue(BlueProperty, (byte)rgb.B);
            _updating = false;
        }
    }
}
