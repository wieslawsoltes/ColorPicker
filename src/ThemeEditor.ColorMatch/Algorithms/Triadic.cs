using ThemeEditor.Colors;

namespace ThemeEditor.ColorMatch.Algorithms
{
    public class Triadic : IAlgorithm
    {
        public Blend Match(HSV hsv)
        {
            Blend outp = new Blend();
            outp.Colors[0] = new HSV(hsv);

            double w = MathHelpers.HueToWheel(hsv.H);

            HSV z = hsv.WithV(100 - hsv.V);
            outp.Colors[1] = new HSV(z);

            z = hsv.WithH(MathHelpers.WheelToHue((w + 120) % 360));
            outp.Colors[2] = new HSV(z);

            z = z.WithV(100 - z.V);
            outp.Colors[3] = new HSV(z);

            z = hsv.WithH(MathHelpers.WheelToHue((w + 240) % 360));
            outp.Colors[4] = new HSV(z);

            z = z.WithV(100 - z.V);
            outp.Colors[5] = new HSV(z);

            return outp;
        }
    }
}
