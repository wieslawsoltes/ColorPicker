using System;

namespace ThemeEditor.Colors
{
    public readonly struct HSV
    {
        public double H { get; }
        public double S { get; }
        public double V { get; }

        public HSV(double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }

        public HSV(HSV hsv)
        {
            H = hsv.H;
            S = hsv.S;
            V = hsv.V;
        }

        public HSV(RGB rgb)
        {
            HSV hsv = rgb.ToHSV();
            H = hsv.H;
            S = hsv.S;
            V = hsv.V;
        }

        public HSV WithH(double h) => new HSV(h, S, V);

        public HSV WithS(double s) => new HSV(H, s, V);

        public HSV WithV(double v) => new HSV(H, S, v);

        public RGB ToRGB() => ToRGB(H, S, V);

        public static RGB ToRGB(double h, double s, double v)
        {
            double R = default;
            double G = default;
            double B = default;

            if (s == 0)
            {
                R = G = B = Math.Round(v * 2.55);
                return new RGB(R, G, B);
            }

            s = s / 100;
            v = v / 100;
            h /= 60;

            var i = Math.Floor(h);
            var f = h - i;
            var p = v * (1 - s);
            var q = v * (1 - s * f);
            var t = v * (1 - s * (1 - f));

            switch ((int)i)
            {
                case 0:
                    R = v;
                    G = t;
                    B = p;
                    break;
                case 1:
                    R = q;
                    G = v;
                    B = p;
                    break;
                case 2:
                    R = p;
                    G = v;
                    B = t;
                    break;
                case 3:
                    R = p;
                    G = q;
                    B = v;
                    break;
                case 4:
                    R = t;
                    G = p;
                    B = v;
                    break;
                default:
                    R = v;
                    G = p;
                    B = q;
                    break;
            }

            R = Math.Round(R * 255);
            G = Math.Round(G * 255);
            B = Math.Round(B * 255);

            return new RGB(R, G, B);
        }
    }
}
