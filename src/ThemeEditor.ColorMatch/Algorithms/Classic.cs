using ThemeEditor.Colors;

namespace ThemeEditor.ColorMatch.Algorithms
{
    public class Classic : IAlgorithm
    {
        public string Name => "Classic";

        public Blend Match(HSV hsv)
        {
            Blend outp = new Blend();
            outp.Colors[0] = new HSV(hsv);

            HSV y = hsv.WithV(hsv.V > 70 ? hsv.V - 30 : hsv.V + 30);
            outp.Colors[1] = new HSV(y);

            HSV yx = new HSV();

            if ((hsv.H >= 0) && (hsv.H < 30))
            {
                y = new HSV(
                    hsv.H + 30,
                    hsv.S,
                    hsv.V);

                yx = new HSV(
                    hsv.H + 30,
                    hsv.S,
                    hsv.V > 70 ? hsv.V - 30 : hsv.V + 30);
            }

            if ((hsv.H >= 30) && (hsv.H < 60))
            {
                y = new HSV(
                    hsv.H + 150,
                    MathHelpers.RC(hsv.S - 30, 100),
                    MathHelpers.RC(hsv.V - 20, 100));

                yx = new HSV(
                    hsv.H + 150,
                    MathHelpers.RC(hsv.S - 50, 100),
                    MathHelpers.RC(hsv.V + 20, 100));
            }

            if ((hsv.H >= 60) && (hsv.H < 180))
            {
                y = new HSV(
                    hsv.H - 40,
                    hsv.S,
                    hsv.V);

                yx = new HSV(
                    hsv.H - 40,
                    hsv.S,
                    hsv.V > 70 ? hsv.V - 30 : hsv.V + 30);
            }

            if ((hsv.H >= 180) && (hsv.H < 220))
            {
                y = new HSV(
                    hsv.H - 160,
                    hsv.S,
                    hsv.V);

                yx = new HSV(
                    hsv.H - 170,
                    hsv.S,
                    hsv.V > 70 ? hsv.V - 30 : hsv.V + 30);
            }

            if ((hsv.H >= 220) && (hsv.H < 300))
            {
                yx = new HSV(
                    hsv.H,
                    MathHelpers.RC(hsv.S - 40, 100),
                    hsv.V);

                yx = new HSV(
                    hsv.H,
                    MathHelpers.RC(hsv.S - 40, 100),
                    hsv.V > 70 ? hsv.V - 30 : hsv.V + 30);
            }

            if (hsv.H >= 300)
            {
                yx = new HSV(
                    (hsv.H + 20) % 360,
                    hsv.S > 50 ? hsv.S - 40 : hsv.S + 40,
                    hsv.V);

                yx = new HSV(
                    (hsv.H + 20) % 360,
                    hsv.S > 50 ? hsv.S - 40 : hsv.S + 40,
                    hsv.V > 70 ? hsv.V - 30 : hsv.V + 30);
            }

            outp.Colors[2] = new HSV(y);

            outp.Colors[3] = new HSV(yx);

            y = new HSV(0, 0, 100 - hsv.V);
            outp.Colors[4] = new HSV(y);

            y = new HSV(0, 0, hsv.V);
            outp.Colors[5] = new HSV(y);

            return outp;
        }
    }
}
