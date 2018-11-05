using System;
using ThemeEditor.Colors;

namespace ThemeEditor.ColorMatch.Algorithms
{
    public class Complementary : IAlgorithm
    {
        public string Name => "Complementary";

        public Blend Match(HSV hsv)
        {
            Blend outp = new Blend();
            outp.Colors[0] = new HSV(hsv);

            HSV z = new HSV(
                hsv.H,
                (hsv.S > 50) ? (hsv.S * 0.5) : (hsv.S * 2),
                (hsv.V < 50) ? (Math.Min(hsv.V * 1.5, 100)) : (hsv.V / 1.5));
            outp.Colors[1] = new HSV(z);

            var w = MathHelpers.HueToWheel(hsv.H);

            z = hsv.WithH(MathHelpers.WheelToHue((w + 180) % 360));
            outp.Colors[2] = new HSV(z);

            z = z.WithS((z.S > 50) ? (z.S * 0.5) : (z.S * 2));
            z = z.WithV((z.V < 50) ? (Math.Min(z.V * 1.5, 100)) : (z.V / 1.5));
            outp.Colors[3] = new HSV(z);

            z = new HSV(0, 0, hsv.V);
            outp.Colors[4] = new HSV(z);

            z = z.WithV(100 - hsv.V);
            outp.Colors[5] = new HSV(z);

            return outp;
        }
    }
}
