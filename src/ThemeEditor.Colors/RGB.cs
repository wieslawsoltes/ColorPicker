using System;

namespace ThemeEditor.Colors
{
    public readonly struct RGB
    {
        public double R { get; }
        public double G { get; }
        public double B { get; }

        public RGB(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public RGB(RGB rgb)
        {
            R = rgb.R;
            G = rgb.G;
            B = rgb.B;
        }

        public RGB(HSV hsv)
        {
            RGB rgb = hsv.ToRGB();
            R = rgb.R;
            G = rgb.G;
            B = rgb.B;
        }

        public HSV ToHSV() => ToHSV(R, G, B);

        public static HSV ToHSV(double r, double g, double b)
        {
            double H = default;
            double S = default;
            double V = default;

            var m = r;

            if (g < m)
            {
                m = g;
            }

            if (b < m)
            {
                m = b;
            }

            var v = r;

            if (g > v)
            {
                v = g;
            }

            if (b > v)
            {
                v = b;
            }

            var value = 100 * v / 255;
            var delta = v - m;

            if (v == 0.0)
            {
                S = 0;
            }
            else
            {
                S = 100 * delta / v;
            }

            if (S == 0)
            {
                H = 0;
            }
            else
            {
                if (r == v)
                {
                    H = 60.0 * (g - b) / delta;
                }
                else if (g == v)
                {
                    H = 120.0 + 60.0 * (b - r) / delta;
                }
                else if (b == v)
                {
                    H = 240.0 + 60.0 * (r - g) / delta;
                }

                if (H < 0.0)
                {
                    H = H + 360.0;
                }
            }

            H = Math.Round(H);
            S = Math.Round(S);
            V = Math.Round(value);

            return new HSV(H, S, V);
        }
    }
}
