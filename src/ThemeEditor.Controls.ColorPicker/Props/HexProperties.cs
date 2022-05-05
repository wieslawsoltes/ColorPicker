using System;
using Avalonia;
using Avalonia.Media;

namespace ThemeEditor.Controls.ColorPicker.Props;

public class HexProperties : ColorPickerProperties
{
    public static readonly StyledProperty<string> HexProperty =
        AvaloniaProperty.Register<HexProperties, string>(nameof(Hex), "#FFFF0000", validate: ValidateHex);

    private static bool ValidateHex(string hex)
    {
        if (!ColorPickerHelpers.IsValidHexColor(hex))
        {
            throw new ArgumentException("Invalid Hex value.");
        }
        return true;
    }

    private bool _updating;

    public HexProperties()
    {
        _updating = false;
        this.GetObservable(HexProperty).Subscribe(_ => UpdateColorPickerValues());
    }

    public string Hex
    {
        get { return GetValue(HexProperty); }
        set { SetValue(HexProperty, value); }
    }

    protected override void UpdateColorPickerValues()
    {
        if (_updating == false && ColorPicker != null)
        {
            _updating = true;
            var color = Color.Parse(Hex);
            ColorPickerHelpers.FromColor(color, out var h, out var s, out var v, out var a);
            ColorPicker.Value1 = h;
            ColorPicker.Value2 = s;
            ColorPicker.Value3 = v;
            ColorPicker.Value4 = a;
            _updating = false;
        }
    }

    public override void UpdatePropertyValues()
    {
        if (_updating == false && ColorPicker != null)
        {
            _updating = true;
            var color = ColorPickerHelpers.FromHSVA(ColorPicker.Value1, ColorPicker.Value2, ColorPicker.Value3, ColorPicker.Value4);
            Hex = ColorPickerHelpers.ToHexColor(color);
            _updating = false;
        }
    }
}
