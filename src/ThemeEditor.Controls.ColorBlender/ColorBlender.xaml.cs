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
        private Rectangle _rgbvar1;
        private Rectangle _rgbvar2;
        private Rectangle _rgbvar3;
        private Rectangle _rgbvar4;
        private Rectangle _rgbvar5;
        private Rectangle _rgbvar6;
        private Rectangle _rgbvar7;
        private Rectangle _hsvvar1;
        private Rectangle _hsvvar2;
        private Rectangle _hsvvar3;
        private Rectangle _hsvvar4;
        private Rectangle _hsvvar5;
        private Rectangle _hsvvar6;
        private Rectangle _hsvvar7;
        private Rectangle _hsvvar8;
        private Rectangle _hsvvar9;
        private Swatch _swatch1;
        private Swatch _swatch2;
        private Swatch _swatch3;
        private Swatch _swatch4;
        private Swatch _swatch5;
        private Swatch _swatch6;
        private bool _updatingSliders = false;

        public ColorBlender()
        {
            this.InitializeComponent();
            _algorithm = this.FindControl<DropDown>("algorithm");
            _sliderR = this.FindControl<Slider>("sliderR");
            _sliderG = this.FindControl<Slider>("sliderG");
            _sliderB = this.FindControl<Slider>("sliderB");
            _sliderH = this.FindControl<Slider>("sliderH");
            _sliderS = this.FindControl<Slider>("sliderS");
            _sliderV = this.FindControl<Slider>("sliderV");
            _rgbvar1 = this.FindControl<Rectangle>("rgbvar1");
            _rgbvar2 = this.FindControl<Rectangle>("rgbvar2");
            _rgbvar3 = this.FindControl<Rectangle>("rgbvar3");
            _rgbvar4 = this.FindControl<Rectangle>("rgbvar4");
            _rgbvar5 = this.FindControl<Rectangle>("rgbvar5");
            _rgbvar6 = this.FindControl<Rectangle>("rgbvar6");
            _rgbvar7 = this.FindControl<Rectangle>("rgbvar7");
            _hsvvar1 = this.FindControl<Rectangle>("hsvvar1");
            _hsvvar2 = this.FindControl<Rectangle>("hsvvar2");
            _hsvvar3 = this.FindControl<Rectangle>("hsvvar3");
            _hsvvar4 = this.FindControl<Rectangle>("hsvvar4");
            _hsvvar5 = this.FindControl<Rectangle>("hsvvar5");
            _hsvvar6 = this.FindControl<Rectangle>("hsvvar6");
            _hsvvar7 = this.FindControl<Rectangle>("hsvvar7");
            _hsvvar8 = this.FindControl<Rectangle>("hsvvar8");
            _hsvvar9 = this.FindControl<Rectangle>("hsvvar9");
            _swatch1 = this.FindControl<Swatch>("swatch1");
            _swatch2 = this.FindControl<Swatch>("swatch2");
            _swatch3 = this.FindControl<Swatch>("swatch3");
            _swatch4 = this.FindControl<Swatch>("swatch4");
            _swatch5 = this.FindControl<Swatch>("swatch5");
            _swatch6 = this.FindControl<Swatch>("swatch6");
            _rgbvar1.PointerPressed += Rectangle_PointerPressed;
            _rgbvar2.PointerPressed += Rectangle_PointerPressed;
            _rgbvar3.PointerPressed += Rectangle_PointerPressed;
            _rgbvar4.PointerPressed += Rectangle_PointerPressed;
            _rgbvar5.PointerPressed += Rectangle_PointerPressed;
            _rgbvar6.PointerPressed += Rectangle_PointerPressed;
            _rgbvar7.PointerPressed += Rectangle_PointerPressed;
            _hsvvar1.PointerPressed += Rectangle_PointerPressed;
            _hsvvar2.PointerPressed += Rectangle_PointerPressed;
            _hsvvar3.PointerPressed += Rectangle_PointerPressed;
            _hsvvar4.PointerPressed += Rectangle_PointerPressed;
            _hsvvar5.PointerPressed += Rectangle_PointerPressed;
            _hsvvar6.PointerPressed += Rectangle_PointerPressed;
            _hsvvar7.PointerPressed += Rectangle_PointerPressed;
            _hsvvar8.PointerPressed += Rectangle_PointerPressed;
            _hsvvar9.PointerPressed += Rectangle_PointerPressed;
            _swatch1._col.PointerPressed += Rectangle_PointerPressed;
            _swatch2._col.PointerPressed += Rectangle_PointerPressed;
            _swatch3._col.PointerPressed += Rectangle_PointerPressed;
            _swatch4._col.PointerPressed += Rectangle_PointerPressed;
            _swatch5._col.PointerPressed += Rectangle_PointerPressed;
            _swatch6._col.PointerPressed += Rectangle_PointerPressed;
            _algorithm.SelectionChanged += Algorithm_SelectionChanged;
        }

        public IAlgorithm[] Algorithms { get; set; }

        public IAlgorithm CurrentAlgorithm { get; set; }

        public Blend CurrentBlend { get; set; }

        public RGB CurrentRGB { get; set; }

        public HSV CurrentHSV { get; set; }

        public RGB[] VariationsRGB { get; set; }

        public RGB[] VariationsHSV { get; set; }

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
            VariationsRGB = new RGB[7];
            VariationsHSV = new RGB[9];
            CurrentHSV = new HSV(199, 95, 62);
            CurrentRGB = new RGB(CurrentHSV);
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

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            InitializeProperties();
            Update();
            UpdateRectangles();
            UpdateSwatches();
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

        private void UpdateRectangles()
        {
            _rgbvar1.Fill = VariationsRGB[0].ToSolidColorBrush();
            _rgbvar2.Fill = VariationsRGB[1].ToSolidColorBrush();
            _rgbvar3.Fill = VariationsRGB[2].ToSolidColorBrush();
            _rgbvar4.Fill = VariationsRGB[3].ToSolidColorBrush();
            _rgbvar5.Fill = VariationsRGB[4].ToSolidColorBrush();
            _rgbvar6.Fill = VariationsRGB[5].ToSolidColorBrush();
            _rgbvar7.Fill = VariationsRGB[6].ToSolidColorBrush();
            _hsvvar1.Fill = VariationsHSV[0].ToSolidColorBrush();
            _hsvvar2.Fill = VariationsHSV[1].ToSolidColorBrush();
            _hsvvar3.Fill = VariationsHSV[2].ToSolidColorBrush();
            _hsvvar4.Fill = VariationsHSV[3].ToSolidColorBrush();
            _hsvvar5.Fill = VariationsHSV[4].ToSolidColorBrush();
            _hsvvar6.Fill = VariationsHSV[5].ToSolidColorBrush();
            _hsvvar7.Fill = VariationsHSV[6].ToSolidColorBrush();
            _hsvvar8.Fill = VariationsHSV[7].ToSolidColorBrush();
            _hsvvar9.Fill = VariationsHSV[8].ToSolidColorBrush();
        }

        private void UpdateSwatches()
        {
            _swatch1._col.Fill = CurrentBlend.Colors[0].ToSolidColorBrush();
            _swatch2._col.Fill = CurrentBlend.Colors[1].ToSolidColorBrush();
            _swatch3._col.Fill = CurrentBlend.Colors[2].ToSolidColorBrush();
            _swatch4._col.Fill = CurrentBlend.Colors[3].ToSolidColorBrush();
            _swatch5._col.Fill = CurrentBlend.Colors[4].ToSolidColorBrush();
            _swatch6._col.Fill = CurrentBlend.Colors[5].ToSolidColorBrush();
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

        private void Algorithm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_updatingSliders == false)
            {
                Update();
                UpdateRectangles();
                UpdateSwatches();
            }
        }

        private void SliderRGB_ValueChanged()
        {
            if (_updatingSliders == false)
            {
                HandleSliderValueChangedRGB();
            }
        }

        private void SliderHSV_ValueChanged()
        {
            if (_updatingSliders == false)
            {
                HandleSliderValueChangedHSV();
            }
        }

        private void Rectangle_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            HandleRectangleClick((sender as Rectangle).Fill as SolidColorBrush);
        }

        private void HandleRectangleClick(SolidColorBrush b)
        {
            CurrentRGB = b.Color.ToRGB();
            CurrentHSV = CurrentRGB.ToHSV();
            Update();
            UpdateRectangles();
            UpdateSwatches();
            UpdateSlidersRGB();
            UpdateSlidersHSV();
        }

        private void HandleSliderValueChangedRGB()
        {
            CurrentRGB = new RGB(_sliderR.Value, _sliderG.Value, _sliderB.Value);
            CurrentHSV = CurrentRGB.ToHSV();
            CurrentRGB = CurrentHSV.ToRGB();
            Update();
            UpdateRectangles();
            UpdateSwatches();
            UpdateSlidersHSV();
        }

        private void HandleSliderValueChangedHSV()
        {
            CurrentHSV = new HSV(_sliderH.Value, _sliderS.Value, _sliderV.Value);
            CurrentRGB = CurrentHSV.ToRGB();
            Update();
            UpdateRectangles();
            UpdateSwatches();
            UpdateSlidersRGB();
        }
    }
}
