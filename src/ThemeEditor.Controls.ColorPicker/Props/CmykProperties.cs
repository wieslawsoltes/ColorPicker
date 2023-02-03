using Avalonia;
using Avalonia.Reactive;
using ThemeEditor.Controls.ColorPicker.Colors;

namespace ThemeEditor.Controls.ColorPicker.Props;

public class CmykProperties : ColorPickerProperties
{
    public static readonly StyledProperty<double?> CyanProperty =
        AvaloniaProperty.Register<CmykProperties, double?>(nameof(Cyan), 0.0);

    public static readonly StyledProperty<double?> MagentaProperty =
        AvaloniaProperty.Register<CmykProperties, double?>(nameof(Magenta), 100.0);

    public static readonly StyledProperty<double?> YellowProperty =
        AvaloniaProperty.Register<CmykProperties, double?>(nameof(Yellow), 100.0);

    public static readonly StyledProperty<double?> BlackKeyProperty =
        AvaloniaProperty.Register<CmykProperties, double?>(nameof(BlackKey), 0.0);

    private bool _updating;

    public CmykProperties()
    {
        this.GetObservable(CyanProperty).Subscribe(new AnonymousObserver<double?>(_ => UpdateColorPickerValues()));
        this.GetObservable(MagentaProperty).Subscribe(new AnonymousObserver<double?>(_ => UpdateColorPickerValues()));
        this.GetObservable(YellowProperty).Subscribe(new AnonymousObserver<double?>(_ => UpdateColorPickerValues()));
        this.GetObservable(BlackKeyProperty).Subscribe(new AnonymousObserver<double?>(_ => UpdateColorPickerValues()));
    }

    public double? Cyan
    {
        get => GetValue(CyanProperty);
        set => SetValue(CyanProperty, value);
    }

    public double? Magenta
    {
        get => GetValue(MagentaProperty);
        set => SetValue(MagentaProperty, value);
    }

    public double? Yellow
    {
        get => GetValue(YellowProperty);
        set => SetValue(YellowProperty, value);
    }

    public double? BlackKey
    {
        get => GetValue(BlackKeyProperty);
        set => SetValue(BlackKeyProperty, value);
    }

    protected override void UpdateColorPickerValues()
    {
        if (_updating == false && Presenter != null)
        {
            _updating = true;
            var cmyk = new CMYK(Cyan ?? 0.0, Magenta ?? 0.0, Yellow ?? 0.0, BlackKey ?? 0.0);
            var hsv = cmyk.ToHSV();
            Presenter.Value1 = hsv.H;
            Presenter.Value2 = hsv.S;
            Presenter.Value3 = hsv.V;
            _updating = false;
        }
    }

    protected override void UpdatePropertyValues()
    {
        if (_updating == false && Presenter != null)
        {
            _updating = true;
            var hsv = new HSV(Presenter.Value1 ?? 0.0, Presenter.Value2 ?? 0.0, Presenter.Value3 ?? 0.0);
            var cmyk = hsv.ToCMYK();
            Cyan = cmyk.C;
            Magenta = cmyk.M;
            Yellow = cmyk.Y;
            BlackKey = cmyk.K;
            _updating = false;
        }
    }
}
