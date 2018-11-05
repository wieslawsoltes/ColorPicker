using ThemeEditor.Colors;

namespace ThemeEditor.ColorMatch.Algorithms
{
    public class SingleHue : IAlgorithm
    {
        public string Name => "Single Hue";

        public Blend Match(HSV hsv)
        {
            Blend outp = new Blend();
            outp.Colors[0] = new HSV(hsv);

            HSV z = hsv.WithV(hsv.V + ((hsv.V < 50) ? 20 : -20));
            outp.Colors[1] = new HSV(z);

            z = z.WithS(hsv.S);
            z = z.WithV(hsv.V + ((hsv.V < 50) ? 40 : -40));
            outp.Colors[2] = new HSV(z);

            z = z.WithS(hsv.S + ((hsv.S < 50) ? 20 : -20));
            z = z.WithV(hsv.V);
            outp.Colors[3] = new HSV(z);

            z = z.WithS(hsv.S + ((hsv.S < 50) ? 40 : -40));
            z = z.WithV(hsv.V);
            outp.Colors[4] = new HSV(z);

            z = z.WithS(hsv.S + ((hsv.S < 50) ? 40 : -40));
            z = z.WithV(hsv.V + ((hsv.V < 50) ? 40 : -40));
            outp.Colors[5] = new HSV(z);

            return outp;
        }
    }
}
