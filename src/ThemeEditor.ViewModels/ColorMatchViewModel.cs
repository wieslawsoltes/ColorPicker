using System.Linq;
using System.Runtime.Serialization;
using ReactiveUI;
using ReactiveUI.Legacy;
using ThemeEditor.ColorMatch;
using ThemeEditor.ColorMatch.Algorithms;
using ThemeEditor.Colors;

namespace ThemeEditor.ViewModels
{
    [DataContract]
    public class ColorMatchViewModel : ReactiveObject
    {
#pragma warning disable CS0618
        [DataMember]
        public IReactiveList<IAlgorithm> Algorithms { get; set; }
#pragma warning restore CS0618

        [DataMember]
        public IAlgorithm CurrentAlgorithm { get; set; }

        [DataMember]
        public Blend CurrentBlend { get; set; }

        [DataMember]
        public RGB CurrentRGB { get; set; }

        [DataMember]
        public HSV CurrentHSV { get; set; }

        [DataMember]
        public RGB[] VariationsRGB { get; set; }

        [DataMember]
        public RGB[] VariationsHSV { get; set; }

        public ColorMatchViewModel()
        {
#pragma warning disable CS0618
            Algorithms = new ReactiveList<IAlgorithm>()
            {
                new Classic(),
                new ColorExplorer(),
                new SingleHue(),
                new Complementary(),
                new SplitComplementary(),
                new Analogue(),
                new Triadic(),
                new Square()
            };
#pragma warning restore CS0618
            CurrentAlgorithm = Algorithms.FirstOrDefault();
        }

        public ColorMatchViewModel(double h, double s, double v) : this()
        {
            VariationsRGB = new RGB[7];
            VariationsHSV = new RGB[9];
            CurrentHSV = new HSV(h, s, v);
            CurrentRGB = new RGB(CurrentHSV);
            Update();
        }

        public ColorMatchViewModel(HSV hsv) : this(hsv.H, hsv.S, hsv.V)
        {
        }

        private double AddLimit(double x, double d, double min, double max)
        {
            x = x + d;
            if (x < min)
                return min;
            if (x > max)
                return max;
            if ((x >= min) && (x <= max))
                return x;

            return double.NaN;
        }

        private RGB HsvVariation(HSV hsv, double addsat, double addval)
        {
            return new HSV(hsv.H, AddLimit(hsv.S, addsat, 0, 99), AddLimit(hsv.V, addval, 0, 99)).ToRGB();
        }

        public void UpdateVariationsRGB()
        {
            double vv = 20;
            double vw = 10;

            VariationsRGB[0] = new RGB(AddLimit(CurrentRGB.R, -vw, 0, 255), AddLimit(CurrentRGB.G, vv, 0, 255), AddLimit(CurrentRGB.B, -vw, 0, 255));
            VariationsRGB[1] = new RGB(AddLimit(CurrentRGB.R, vw, 0, 255), AddLimit(CurrentRGB.G, vw, 0, 255), AddLimit(CurrentRGB.B, -vv, 0, 255));
            VariationsRGB[2] = new RGB(AddLimit(CurrentRGB.R, -vv, 0, 255), AddLimit(CurrentRGB.G, vw, 0, 255), AddLimit(CurrentRGB.B, vw, 0, 255));
            VariationsRGB[3] = new RGB(CurrentRGB.R, CurrentRGB.G, CurrentRGB.B);
            VariationsRGB[4] = new RGB(AddLimit(CurrentRGB.R, vv, 0, 255), AddLimit(CurrentRGB.G, -vw, 0, 255), AddLimit(CurrentRGB.B, -vw, 0, 255));
            VariationsRGB[5] = new RGB(AddLimit(CurrentRGB.R, -vw, 0, 255), AddLimit(CurrentRGB.G, -vw, 0, 255), AddLimit(CurrentRGB.B, vv, 0, 255));
            VariationsRGB[6] = new RGB(AddLimit(CurrentRGB.R, vw, 0, 255), AddLimit(CurrentRGB.G, -vv, 0, 255), AddLimit(CurrentRGB.B, vw, 0, 255));
        }

        public void UpdateVariationsHSV()
        {
            double vv = 10;

            VariationsHSV[0] = HsvVariation(CurrentHSV, -vv, vv);
            VariationsHSV[1] = HsvVariation(CurrentHSV, 0, vv);
            VariationsHSV[2] = HsvVariation(CurrentHSV, vv, vv);
            VariationsHSV[3] = HsvVariation(CurrentHSV, -vv, 0);
            VariationsHSV[4] = CurrentHSV.ToRGB();
            VariationsHSV[5] = HsvVariation(CurrentHSV, vv, 0);
            VariationsHSV[6] = HsvVariation(CurrentHSV, -vv, -vv);
            VariationsHSV[7] = HsvVariation(CurrentHSV, 0, -vv);
            VariationsHSV[8] = HsvVariation(CurrentHSV, vv, -vv);
        }

        public void Update()
        {
            CurrentBlend = CurrentAlgorithm.Match(CurrentHSV);
            UpdateVariationsRGB();
            UpdateVariationsHSV();
        }
    }
}
