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

        public RGB WithR(double r) => new RGB(r, G, B);

        public RGB WithG(double g) => new RGB(R, g, B);

        public RGB WithB(double b) => new RGB(R, G, b);

        public HSV ToHSV() => ToHSV(R, G, B);

        public static HSV ToHSV(double r, double g, double b)
        {
            double H = default;
            double S = default;
            double V = default;

            double min = Math.Min(Math.Min(r, g), b);
            double max = Math.Max(Math.Max(r, g), b);

            double delta = max - min;

            V = 100.0 * max / 255.0;

            if (max == 0.0)
            {
                S = 0;
            }
            else
            {
                S = 100.0 * delta / max;
            }

            if (S == 0)
            {
                H = 0;
            }
            else
            {
                if (r == max)
                {
                    H = 60.0 * (g - b) / delta;
                }
                else if (g == max)
                {
                    H = 120.0 + 60.0 * (b - r) / delta;
                }
                else if (b == max)
                {
                    H = 240.0 + 60.0 * (r - g) / delta;
                }

                if (H < 0.0)
                {
                    H += 360.0;
                }
            }

            return new HSV(H, S, V);
        }
    }
}
