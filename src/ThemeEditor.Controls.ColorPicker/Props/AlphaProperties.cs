using System;
using Avalonia;

namespace ThemeEditor.Controls.ColorPicker.Props;

public class AlphaProperties : ColorPickerProperties
{
    public static readonly StyledProperty<double?> AlphaProperty =
        AvaloniaProperty.Register<AlphaProperties, double?>(nameof(Alpha), 100.0, validate: ValidateAlpha, coerce: CoerceAlpha);

    private static double? CoerceAlpha(IAvaloniaObject arg1, double? arg2)
    {
        if (arg2 is null)
        {
            return null;
        }
        if (double.IsNaN(arg2.Value))
        {
            return 100.0;
        }
        return ColorPickerHelpers.Clamp(arg2.Value, 0.0, 100.0);
    }

    private static bool ValidateAlpha(double? alpha)
    {
        if (alpha is null)
        {
            return true;
        }
        if (alpha < 0.0 || alpha > 100.0)
        {
            return false;
        }
        return true;
    }

    private bool _updating;

    public AlphaProperties()
    {
        _updating = false;
        this.GetObservable(AlphaProperty).Subscribe(_ => UpdateColorPickerValues());
    }

    public double? Alpha
    {
        get => GetValue(AlphaProperty);
        set => SetValue(AlphaProperty, value);
    }

    protected override void UpdateColorPickerValues()
    {
        if (_updating == false && Presenter != null)
        {
            _updating = true;
            Presenter.Value4 = Alpha ?? 100.0;
            _updating = false;
        }
    }

    protected override void UpdatePropertyValues()
    {
        if (_updating == false && Presenter != null)
        {
            _updating = true;
            Alpha = Presenter.Value4;
            _updating = false;
        }
    }
}
