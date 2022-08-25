﻿using System;
using Avalonia;
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
        this.GetObservable(RedProperty).Subscribe(_ => UpdateColorPickerValues());
        this.GetObservable(GreenProperty).Subscribe(_ => UpdateColorPickerValues());
        this.GetObservable(BlueProperty).Subscribe(_ => UpdateColorPickerValues());
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
            var hsv = new HSV(Presenter.Value1 ?? 0x00, Presenter.Value2 ?? 0x00, Presenter.Value3 ?? 0x00);
            var rgb = hsv.ToRGB();
            Red = (byte)rgb.R;
            Green = (byte)rgb.G;
            Blue = (byte)rgb.B;
            _updating = false;
        }
    }
}
