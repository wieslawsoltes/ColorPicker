using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ThemeEditor.ColorMatch;
using ThemeEditor.ColorMatch.Algorithms;
using ThemeEditor.Colors;

namespace ThemeEditor.Controls.ColorBlender
{
    internal static class ColorHelpers
    {
        private static Regex s_hexRegex = new Regex("^#[a-fA-F0-9]{8}$");

        public static bool IsValidHexColor(string hex)
        {
            return !string.IsNullOrWhiteSpace(hex) && s_hexRegex.Match(hex).Success;
        }

        public static string ToHexColor(Color color)
        {
            return $"#{color.ToUint32():X8}";
        }

        public static Color FromHexColor(string hex)
        {
            return Color.Parse(hex);
        }

        public static RGB ToRGB(this Color c)
        {
            return new RGB(c.R, c.G, c.B);
        }

        public static HSV ToHSV(this Color c)
        {
            return ToRGB(c).ToHSV();
        }

        public static Color ToColor(this HSV hsv)
        {
            return ToColor(hsv.ToRGB());
        }

        public static Color ToColor(this RGB rgb)
        {
            return Color.FromRgb(
                (byte)Math.Round(rgb.R),
                (byte)Math.Round(rgb.G),
                (byte)Math.Round(rgb.B));
        }

        public static SolidColorBrush ToSolidColorBrush(this RGB rgb)
        {
            return new SolidColorBrush(ToColor(rgb));
        }

        public static SolidColorBrush ToSolidColorBrush(this HSV hsv)
        {
            return new SolidColorBrush(ToColor(hsv));
        }
    }

    internal class ColorToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color c && targetType == typeof(string))
            {
                try
                {
                    return ColorHelpers.ToHexColor(c);
                }
                catch (Exception)
                {
                    return AvaloniaProperty.UnsetValue;
                }
            }
            return AvaloniaProperty.UnsetValue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && targetType == typeof(Color))
            {
                try
                {
                    if (ColorHelpers.IsValidHexColor(s))
                    {
                        return ColorHelpers.FromHexColor(s);
                    }
                }
                catch (Exception)
                {
                    return AvaloniaProperty.UnsetValue;
                }
            }
            return AvaloniaProperty.UnsetValue;
        }
    }

    public partial class ColorBlender : UserControl
    {
        private DropDown _algorithm;
        private Slider _sliderR;
        private Slider _sliderG;
        private Slider _sliderB;
        private Slider _sliderH;
        private Slider _sliderS;
        private Slider _sliderV;
        private Rectangle _rgb1;
        private Rectangle _rgb2;
        private Rectangle _rgb3;
        private Rectangle _rgb4;
        private Rectangle _rgb5;
        private Rectangle _rgb6;
        private Rectangle _rgb7;
        private Rectangle _hsv1;
        private Rectangle _hsv2;
        private Rectangle _hsv3;
        private Rectangle _hsv4;
        private Rectangle _hsv5;
        private Rectangle _hsv6;
        private Rectangle _hsv7;
        private Rectangle _hsv8;
        private Rectangle _hsv9;
        private Rectangle _swatch1;
        private Rectangle _swatch2;
        private Rectangle _swatch3;
        private Rectangle _swatch4;
        private Rectangle _swatch5;
        private Rectangle _swatch6;
        private bool _updatingSliders = false;

        public ColorBlender()
        {
            this.InitializeComponent();
            _algorithm = this.FindControl<DropDown>("algorithm");
            _algorithm.SelectionChanged += Algorithm_SelectionChanged;
            _sliderR = this.FindControl<Slider>("sliderR");
            _sliderG = this.FindControl<Slider>("sliderG");
            _sliderB = this.FindControl<Slider>("sliderB");
            _sliderH = this.FindControl<Slider>("sliderH");
            _sliderS = this.FindControl<Slider>("sliderS");
            _sliderV = this.FindControl<Slider>("sliderV");
            _rgb1 = this.FindControl<Rectangle>("rgb1");
            _rgb2 = this.FindControl<Rectangle>("rgb2");
            _rgb3 = this.FindControl<Rectangle>("rgb3");
            _rgb4 = this.FindControl<Rectangle>("rgb4");
            _rgb5 = this.FindControl<Rectangle>("rgb5");
            _rgb6 = this.FindControl<Rectangle>("rgb6");
            _rgb7 = this.FindControl<Rectangle>("rgb7");
            _hsv1 = this.FindControl<Rectangle>("hsv1");
            _hsv2 = this.FindControl<Rectangle>("hsv2");
            _hsv3 = this.FindControl<Rectangle>("hsv3");
            _hsv4 = this.FindControl<Rectangle>("hsv4");
            _hsv5 = this.FindControl<Rectangle>("hsv5");
            _hsv6 = this.FindControl<Rectangle>("hsv6");
            _hsv7 = this.FindControl<Rectangle>("hsv7");
            _hsv8 = this.FindControl<Rectangle>("hsv8");
            _hsv9 = this.FindControl<Rectangle>("hsv9");
            _swatch1 = this.FindControl<Rectangle>("swatch1");
            _swatch2 = this.FindControl<Rectangle>("swatch2");
            _swatch3 = this.FindControl<Rectangle>("swatch3");
            _swatch4 = this.FindControl<Rectangle>("swatch4");
            _swatch5 = this.FindControl<Rectangle>("swatch5");
            _swatch6 = this.FindControl<Rectangle>("swatch6");
            _rgb1.PointerPressed += Rectangle_PointerPressed;
            _rgb2.PointerPressed += Rectangle_PointerPressed;
            _rgb3.PointerPressed += Rectangle_PointerPressed;
            _rgb4.PointerPressed += Rectangle_PointerPressed;
            _rgb5.PointerPressed += Rectangle_PointerPressed;
            _rgb6.PointerPressed += Rectangle_PointerPressed;
            _rgb7.PointerPressed += Rectangle_PointerPressed;
            _hsv1.PointerPressed += Rectangle_PointerPressed;
            _hsv2.PointerPressed += Rectangle_PointerPressed;
            _hsv3.PointerPressed += Rectangle_PointerPressed;
            _hsv4.PointerPressed += Rectangle_PointerPressed;
            _hsv5.PointerPressed += Rectangle_PointerPressed;
            _hsv6.PointerPressed += Rectangle_PointerPressed;
            _hsv7.PointerPressed += Rectangle_PointerPressed;
            _hsv8.PointerPressed += Rectangle_PointerPressed;
            _hsv9.PointerPressed += Rectangle_PointerPressed;
            _swatch1.PointerPressed += Rectangle_PointerPressed;
            _swatch2.PointerPressed += Rectangle_PointerPressed;
            _swatch3.PointerPressed += Rectangle_PointerPressed;
            _swatch4.PointerPressed += Rectangle_PointerPressed;
            _swatch5.PointerPressed += Rectangle_PointerPressed;
            _swatch6.PointerPressed += Rectangle_PointerPressed;
        }

        public IAlgorithm[] Algorithms { get; set; }

        public IAlgorithm CurrentAlgorithm { get; set; }

        public RGB CurrentRGB { get; set; }

        public HSV CurrentHSV { get; set; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InitializeProperties()
        {
            Algorithms = new IAlgorithm[]
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
            CurrentAlgorithm = Algorithms[0];
            CurrentHSV = new HSV(199, 95, 62);
            CurrentRGB = CurrentHSV.ToRGB();
        }

        private void UpdateRectangles()
        {
            Blend blend = CurrentAlgorithm.Match(CurrentHSV);
            RGB[] variationsRGB = new RGB[7];
            RGB[] variationsHSV = new RGB[9];
            double vv = 20;
            double vw = 10;
            double vx = 10;

            variationsRGB[0] = new RGB(AddLimit(CurrentRGB.R, -vw, 0, 255), AddLimit(CurrentRGB.G, vv, 0, 255), AddLimit(CurrentRGB.B, -vw, 0, 255));
            variationsRGB[1] = new RGB(AddLimit(CurrentRGB.R, vw, 0, 255), AddLimit(CurrentRGB.G, vw, 0, 255), AddLimit(CurrentRGB.B, -vv, 0, 255));
            variationsRGB[2] = new RGB(AddLimit(CurrentRGB.R, -vv, 0, 255), AddLimit(CurrentRGB.G, vw, 0, 255), AddLimit(CurrentRGB.B, vw, 0, 255));
            variationsRGB[3] = new RGB(CurrentRGB.R, CurrentRGB.G, CurrentRGB.B);
            variationsRGB[4] = new RGB(AddLimit(CurrentRGB.R, vv, 0, 255), AddLimit(CurrentRGB.G, -vw, 0, 255), AddLimit(CurrentRGB.B, -vw, 0, 255));
            variationsRGB[5] = new RGB(AddLimit(CurrentRGB.R, -vw, 0, 255), AddLimit(CurrentRGB.G, -vw, 0, 255), AddLimit(CurrentRGB.B, vv, 0, 255));
            variationsRGB[6] = new RGB(AddLimit(CurrentRGB.R, vw, 0, 255), AddLimit(CurrentRGB.G, -vv, 0, 255), AddLimit(CurrentRGB.B, vw, 0, 255));

            variationsHSV[0] = HsvVariation(CurrentHSV, -vx, vx);
            variationsHSV[1] = HsvVariation(CurrentHSV, 0, vx);
            variationsHSV[2] = HsvVariation(CurrentHSV, vx, vx);
            variationsHSV[3] = HsvVariation(CurrentHSV, -vx, 0);
            variationsHSV[4] = CurrentHSV.ToRGB();
            variationsHSV[5] = HsvVariation(CurrentHSV, vx, 0);
            variationsHSV[6] = HsvVariation(CurrentHSV, -vx, -vx);
            variationsHSV[7] = HsvVariation(CurrentHSV, 0, -vx);
            variationsHSV[8] = HsvVariation(CurrentHSV, vx, -vx);

            double AddLimit(double x, double d, double min, double max)
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

            RGB HsvVariation(HSV hsv, double addsat, double addval)
            {
                return new HSV(hsv.H, AddLimit(hsv.S, addsat, 0, 99), AddLimit(hsv.V, addval, 0, 99)).ToRGB();
            }

            _rgb1.Fill = variationsRGB[0].ToSolidColorBrush();
            _rgb2.Fill = variationsRGB[1].ToSolidColorBrush();
            _rgb3.Fill = variationsRGB[2].ToSolidColorBrush();
            _rgb4.Fill = variationsRGB[3].ToSolidColorBrush();
            _rgb5.Fill = variationsRGB[4].ToSolidColorBrush();
            _rgb6.Fill = variationsRGB[5].ToSolidColorBrush();
            _rgb7.Fill = variationsRGB[6].ToSolidColorBrush();

            _hsv1.Fill = variationsHSV[0].ToSolidColorBrush();
            _hsv2.Fill = variationsHSV[1].ToSolidColorBrush();
            _hsv3.Fill = variationsHSV[2].ToSolidColorBrush();
            _hsv4.Fill = variationsHSV[3].ToSolidColorBrush();
            _hsv5.Fill = variationsHSV[4].ToSolidColorBrush();
            _hsv6.Fill = variationsHSV[5].ToSolidColorBrush();
            _hsv7.Fill = variationsHSV[6].ToSolidColorBrush();
            _hsv8.Fill = variationsHSV[7].ToSolidColorBrush();
            _hsv9.Fill = variationsHSV[8].ToSolidColorBrush();

            _swatch1.Fill = blend.Colors[0].ToSolidColorBrush();
            _swatch2.Fill = blend.Colors[1].ToSolidColorBrush();
            _swatch3.Fill = blend.Colors[2].ToSolidColorBrush();
            _swatch4.Fill = blend.Colors[3].ToSolidColorBrush();
            _swatch5.Fill = blend.Colors[4].ToSolidColorBrush();
            _swatch6.Fill = blend.Colors[5].ToSolidColorBrush();
        }

        private void UpdateSlidersRGB()
        {
            _updatingSliders = true;
            _sliderR.Value = CurrentRGB.R;
            _sliderG.Value = CurrentRGB.G;
            _sliderB.Value = CurrentRGB.B;
            _updatingSliders = false;
        }

        private void UpdateSlidersHSV()
        {
            _updatingSliders = true;
            _sliderH.Value = CurrentHSV.H;
            _sliderS.Value = CurrentHSV.S;
            _sliderV.Value = CurrentHSV.V;
            _updatingSliders = false;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            InitializeProperties();
            UpdateRectangles();
            UpdateSlidersRGB();
            UpdateSlidersHSV();
            _sliderR.GetObservable(Slider.ValueProperty).Subscribe(value => SliderRGB_ValueChanged());
            _sliderG.GetObservable(Slider.ValueProperty).Subscribe(value => SliderRGB_ValueChanged());
            _sliderB.GetObservable(Slider.ValueProperty).Subscribe(value => SliderRGB_ValueChanged());
            _sliderH.GetObservable(Slider.ValueProperty).Subscribe(value => SliderHSV_ValueChanged());
            _sliderS.GetObservable(Slider.ValueProperty).Subscribe(value => SliderHSV_ValueChanged());
            _sliderV.GetObservable(Slider.ValueProperty).Subscribe(value => SliderHSV_ValueChanged());
            DataContext = this;
        }

        private void Algorithm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_updatingSliders == false)
            {
                UpdateRectangles();
            }
        }

        private void SliderRGB_ValueChanged()
        {
            if (_updatingSliders == false)
            {
                CurrentRGB = new RGB(_sliderR.Value, _sliderG.Value, _sliderB.Value);
                CurrentHSV = CurrentRGB.ToHSV();
                UpdateRectangles();
                UpdateSlidersHSV();
            }
        }

        private void SliderHSV_ValueChanged()
        {
            if (_updatingSliders == false)
            {
                CurrentHSV = new HSV(_sliderH.Value, _sliderS.Value, _sliderV.Value);
                CurrentRGB = CurrentHSV.ToRGB();
                UpdateRectangles();
                UpdateSlidersRGB();
            }
        }

        private void Rectangle_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (_updatingSliders == false)
            {
                SolidColorBrush b = (sender as Rectangle).Fill as SolidColorBrush;
                CurrentRGB = b.Color.ToRGB();
                CurrentHSV = CurrentRGB.ToHSV();
                UpdateRectangles();
                UpdateSlidersRGB();
                UpdateSlidersHSV();
            }
        }
    }
}
