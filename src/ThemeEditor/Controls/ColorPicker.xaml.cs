using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ThemeEditor.Controls
{
    public class HSV
    {
        public double H { get; set; }
        public double S { get; set; }
        public double V { get; set; }

        public HSV()
        {
        }

        public HSV(double h, double s, double v)
        {
            this.H = h;
            this.S = s;
            this.V = v;
        }

        public HSV(HSV hs)
        {
            this.H = hs.H;
            this.S = hs.S;
            this.V = hs.V;
        }

        public HSV(RGB rg)
        {
            HSV hs = rg.ToHSV();
            this.H = hs.H;
            this.S = hs.S;
            this.V = hs.V;
        }

        public RGB ToRGB()
        {
            RGB rg = new RGB();
            HSV hsx = new HSV(this.H, this.S, this.V);

            if (hsx.S == 0)
            {
                rg.R = rg.G = rg.B = Math.Round(hsx.V * 2.55); return (rg);
            }

            hsx.S = hsx.S / 100;
            hsx.V = hsx.V / 100;
            hsx.H /= 60;

            var i = Math.Floor(hsx.H);
            var f = hsx.H - i;
            var p = hsx.V * (1 - hsx.S);
            var q = hsx.V * (1 - hsx.S * f);
            var t = hsx.V * (1 - hsx.S * (1 - f));

            switch ((int)i)
            {
                case 0: rg.R = hsx.V; rg.G = t; rg.B = p; break;
                case 1: rg.R = q; rg.G = hsx.V; rg.B = p; break;
                case 2: rg.R = p; rg.G = hsx.V; rg.B = t; break;
                case 3: rg.R = p; rg.G = q; rg.B = hsx.V; break;
                case 4: rg.R = t; rg.G = p; rg.B = hsx.V; break;
                default: rg.R = hsx.V; rg.G = p; rg.B = q; break;
            }

            rg.R = Math.Round(rg.R * 255);
            rg.G = Math.Round(rg.G * 255);
            rg.B = Math.Round(rg.B * 255);

            return rg;
        }
    }

    public class RGB
    {
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public RGB()
        {
        }

        public RGB(double r, double g, double b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public RGB(RGB rg)
        {
            this.R = rg.R;
            this.G = rg.G;
            this.B = rg.B;
        }

        public RGB(HSV hs)
        {
            RGB rg = hs.ToRGB();
            this.R = rg.R;
            this.G = rg.G;
            this.B = rg.B;
        }

        public HSV ToHSV()
        {
            HSV hs = new HSV();
            RGB rg = new RGB(this.R, this.G, this.B);

            var m = rg.R;
            if (rg.G < m) { m = rg.G; }
            if (rg.B < m) { m = rg.B; }
            var v = rg.R;
            if (rg.G > v) { v = rg.G; }
            if (rg.B > v) { v = rg.B; }
            var value = 100 * v / 255;
            var delta = v - m;
            if (v == 0.0) { hs.S = 0; } else { hs.S = 100 * delta / v; }

            if (hs.S == 0) { hs.H = 0; }
            else
            {
                if (rg.R == v) { hs.H = 60.0 * (rg.G - rg.B) / delta; }
                else if (rg.G == v) { hs.H = 120.0 + 60.0 * (rg.B - rg.R) / delta; }
                else if (rg.B == v) { hs.H = 240.0 + 60.0 * (rg.R - rg.G) / delta; }
                if (hs.H < 0.0) { hs.H = hs.H + 360.0; }
            }

            hs.H = Math.Round(hs.H);
            hs.S = Math.Round(hs.S);
            hs.V = Math.Round(value);

            return hs;
        }
    }

    public class HueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double hue && targetType == typeof(Color))
            {
                var rgb = new HSV(hue, 100, 100).ToRGB();
                var color = new Color(255, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
                return color;
            }
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
