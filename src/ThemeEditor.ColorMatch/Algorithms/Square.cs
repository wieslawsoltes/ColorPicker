using ThemeEditor.Colors;

namespace ThemeEditor.ColorMatch.Algorithms
{
    public class Square : IAlgorithm
    {
        public string Name => "Square";

        public Blend Match(HSV hsv)
        {
            Blend outp = new Blend();
            outp.Colors[0] = new HSV(hsv);

            var w = MathHelpers.HueToWheel(hsv.H);

            HSV z = hsv.WithH(MathHelpers.WheelToHue((w + 90) % 360));
            outp.Colors[1] = new HSV(z);

            z = hsv.WithH(MathHelpers.WheelToHue((w + 180) % 360));
            outp.Colors[2] = new HSV(z);

            z = hsv.WithH(MathHelpers.WheelToHue((w + 270) % 360));
            outp.Colors[3] = new HSV(z);

            z = z.WithS(0);
            outp.Colors[4] = new HSV(z);

            z = z.WithV(100 - z.V);
            outp.Colors[5] = new HSV(z);

            return outp;
        }
    }
}
