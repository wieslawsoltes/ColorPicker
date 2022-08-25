using System;
using Avalonia;

namespace ThemeEditor.Controls.ColorPicker.Props;

public class AlphaProperties : ColorPickerProperties
{
    public static readonly StyledProperty<double?> AlphaProperty =
        AvaloniaProperty.Register<AlphaProperties, double?>(nameof(Alpha), 100.0);

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
