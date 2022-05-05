using System;
using Avalonia;
using ThemeEditor.Controls.ColorPicker.Colors;

namespace ThemeEditor.Controls.ColorPicker.Props;

public class CmykProperties : ColorPickerProperties
{
    public static readonly StyledProperty<double> CyanProperty =
        AvaloniaProperty.Register<CmykProperties, double>(nameof(Cyan), 0.0, validate: ValidateCyan, coerce: CoerceCyan);

    public static readonly StyledProperty<double> MagentaProperty =
        AvaloniaProperty.Register<CmykProperties, double>(nameof(Magenta), 100.0, validate: ValidateMagenta, coerce: CoerceMagenta);

    public static readonly StyledProperty<double> YellowProperty =
        AvaloniaProperty.Register<CmykProperties, double>(nameof(Yellow), 100.0, validate: ValidateYellow, coerce: CoerceYellow);

    public static readonly StyledProperty<double> BlackKeyProperty =
        AvaloniaProperty.Register<CmykProperties, double>(nameof(BlackKey), 0.0, validate: ValidateBlackKey, coerce: CoerceBlackKey);

    private static double CoerceCyan(IAvaloniaObject arg1, double arg2)
    {
        return ColorPickerHelpers.Clamp(arg2, 0.0, 100.0);
    }

    private static double CoerceMagenta(IAvaloniaObject arg1, double arg2)
    {
        return ColorPickerHelpers.Clamp(arg2, 0.0, 100.0);
    }

    private static double CoerceYellow(IAvaloniaObject arg1, double arg2)
    {
        return ColorPickerHelpers.Clamp(arg2, 0.0, 100.0);
    }

    private static double CoerceBlackKey(IAvaloniaObject arg1, double arg2)
    {
        return ColorPickerHelpers.Clamp(arg2, 0.0, 100.0);
    }

    private static bool ValidateCyan(double cyan)
    {
        if (cyan < 0.0 || cyan > 100.0)
        {
            throw new ArgumentException("Invalid Cyan value.");
        }
        return true;
    }

    private static bool ValidateMagenta(double magenta)
    {
        if (magenta < 0.0 || magenta > 100.0)
        {
            throw new ArgumentException("Invalid Magenta value.");
        }
        return true;
    }

    private static bool ValidateYellow(double yellow)
    {
        if (yellow < 0.0 || yellow > 100.0)
        {
            throw new ArgumentException("Invalid Yellow value.");
        }
        return true;
    }

    private static bool ValidateBlackKey(double blackKey)
    {
        if (blackKey < 0.0 || blackKey > 100.0)
        {
            throw new ArgumentException("Invalid BlackKey value.");
        }
        return true;
    }

    private bool _updating;

    public CmykProperties()
    {
        this.GetObservable(CyanProperty).Subscribe(_ => UpdateColorPickerValues());
        this.GetObservable(MagentaProperty).Subscribe(_ => UpdateColorPickerValues());
        this.GetObservable(YellowProperty).Subscribe(_ => UpdateColorPickerValues());
        this.GetObservable(BlackKeyProperty).Subscribe(_ => UpdateColorPickerValues());
    }

    public double Cyan
    {
        get => GetValue(CyanProperty);
        set => SetValue(CyanProperty, value);
    }

    public double Magenta
    {
        get => GetValue(MagentaProperty);
        set => SetValue(MagentaProperty, value);
    }

    public double Yellow
    {
        get => GetValue(YellowProperty);
        set => SetValue(YellowProperty, value);
    }

    public double BlackKey
    {
        get => GetValue(BlackKeyProperty);
        set => SetValue(BlackKeyProperty, value);
    }

    protected override void UpdateColorPickerValues()
    {
        if (_updating == false && Presenter != null)
        {
            _updating = true;
            var cmyk = new CMYK(Cyan, Magenta, Yellow, BlackKey);
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
            var hsv = new HSV(Presenter.Value1, Presenter.Value2, Presenter.Value3);
            var cmyk = hsv.ToCMYK();
            Cyan = cmyk.C;
            Magenta = cmyk.M;
            Yellow = cmyk.Y;
            BlackKey = cmyk.K;
            _updating = false;
        }
    }
}
