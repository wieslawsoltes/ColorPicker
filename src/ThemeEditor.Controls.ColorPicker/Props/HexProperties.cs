using Avalonia;
using Avalonia.Media;
using Avalonia.Reactive;

namespace ThemeEditor.Controls.ColorPicker.Props;

public class HexProperties : ColorPickerProperties
{
    public static readonly StyledProperty<string?> HexProperty =
        AvaloniaProperty.Register<HexProperties, string?>(nameof(Hex));

    private bool _updating;

    public HexProperties()
    {
        _updating = false;
        this.GetObservable(HexProperty).Subscribe(new AnonymousObserver<string?>(_ => UpdateColorPickerValues()));
    }

    public string? Hex
    {
        get => GetValue(HexProperty);
        set => SetValue(HexProperty, value);
    }

    protected override void UpdateColorPickerValues()
    {
        if (_updating == false && Presenter != null && Hex is { })
        {
            _updating = true;
            var color = Color.Parse(Hex);
            ColorPickerHelpers.FromColor(color, out var h, out var s, out var v, out var a);
            Presenter.SetCurrentValue(ColorPickerValuesPresenter.Value1Property, h);
            Presenter.SetCurrentValue(ColorPickerValuesPresenter.Value2Property, s);
            Presenter.SetCurrentValue(ColorPickerValuesPresenter.Value3Property, v);
            Presenter.SetCurrentValue(ColorPickerValuesPresenter.Value4Property, a);
            _updating = false;
        }
    }

    protected override void UpdatePropertyValues()
    {
        if (_updating == false && Presenter != null)
        {
            _updating = true;
            var color = ColorPickerHelpers.FromHSVA(Presenter.Value1 ?? 0.0, Presenter.Value2 ?? 0.0, Presenter.Value3 ?? 0.0, Presenter.Value4 ?? 100.0);
            SetCurrentValue(HexProperty, ColorPickerHelpers.ToHexColor(color));
            _updating = false;
        }
    }
}
