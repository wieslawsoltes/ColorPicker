using System;
using ThemeEditor.Colors;

namespace ThemeEditor.ColorMatch.Algorithms
{
    public class ColorExplorer : IAlgorithm
    {
        public Blend Match(HSV hsv)
        {
            Blend outp = new Blend();
            outp.Colors[0] = new HSV(hsv);

            HSV z = new HSV(
                hsv.H,
                Math.Round(hsv.S * 0.3),
                Math.Min(Math.Round(hsv.V * 1.3), 100));
            outp.Colors[1] = new HSV(z);

            z = hsv.WithH((hsv.H + 300) % 360);
            outp.Colors[3] = new HSV(z);

            z = z.WithS(Math.Min(Math.Round(z.S * 1.2), 100));
            z = z.WithV(Math.Min(Math.Round(z.V * 0.5), 100));
            outp.Colors[2] = new HSV(z);

            z = z.WithS(0);
            z = z.WithV((hsv.V + 50) % 100);
            outp.Colors[4] = new HSV(z);

            z = z.WithV((z.V + 50) % 100);
            outp.Colors[5] = new HSV(z);

            return outp;
        }
    }
}
