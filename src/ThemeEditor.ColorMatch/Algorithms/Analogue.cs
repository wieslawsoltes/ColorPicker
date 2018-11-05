using System;
using ThemeEditor.Colors;

namespace ThemeEditor.ColorMatch.Algorithms
{
    public class Analogue : IAlgorithm
    {
        public string Name => "Analogue";

        public Blend Match(HSV hsv)
        {
            Blend outp = new Blend();
            outp.Colors[0] = new HSV(hsv);

            var w = MathHelpers.HueToWheel(hsv.H);
            HSV z = hsv.WithH(MathHelpers.WheelToHue((w + 30) % 360));
            outp.Colors[1] = new HSV(z);

            z = hsv.WithH(MathHelpers.WheelToHue((w + 60) % 360));
            outp.Colors[2] = new HSV(z);

            z = new HSV(0, 0, 100 - hsv.V);
            outp.Colors[3] = new HSV(z);

            z = z.WithV(Math.Round(hsv.V * 1.3) % 100);
            outp.Colors[4] = new HSV(z);

            z = z.WithV(Math.Round(hsv.V / 1.3) % 100);
            outp.Colors[5] = new HSV(z);

            return outp;
        }
    }
}
