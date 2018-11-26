using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ThemeEditor.ColorMatch;
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
        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<ColorBlender, Color>(nameof(Color));

        public static readonly StyledProperty<IAlgorithm> AlgorithmProperty =
            AvaloniaProperty.Register<ColorBlender, IAlgorithm>(nameof(Algorithm));

        private Rectangle _swatch1;
        private Rectangle _swatch2;
        private Rectangle _swatch3;
        private Rectangle _swatch4;
        private Rectangle _swatch5;
        private Rectangle _swatch6;

        public ColorBlender()
        {
            this.InitializeComponent();

            _swatch1 = this.FindControl<Rectangle>("PART_Swatch1");
            _swatch2 = this.FindControl<Rectangle>("PART_Swatch2");
            _swatch3 = this.FindControl<Rectangle>("PART_Swatch3");
            _swatch4 = this.FindControl<Rectangle>("PART_Swatch4");
            _swatch5 = this.FindControl<Rectangle>("PART_Swatch5");
            _swatch6 = this.FindControl<Rectangle>("PART_Swatch6");

            this.GetObservable(ColorProperty).Subscribe(x => Update());
            this.GetObservable(AlgorithmProperty).Subscribe(x => Update());
        }

        public Color Color
        {
            get { return GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public IAlgorithm Algorithm
        {
            get { return GetValue(AlgorithmProperty); }
            set { SetValue(AlgorithmProperty, value); }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Update()
        {
            System.Diagnostics.Debug.WriteLine($"Update: {ColorHelpers.ToHexColor(Color)}");
            if (Algorithm != null)
            {
                var blend = Algorithm.Match(Color.ToHSV());
                _swatch1.Fill = blend.Colors[0].ToSolidColorBrush();
                _swatch2.Fill = blend.Colors[1].ToSolidColorBrush();
                _swatch3.Fill = blend.Colors[2].ToSolidColorBrush();
                _swatch4.Fill = blend.Colors[3].ToSolidColorBrush();
                _swatch5.Fill = blend.Colors[4].ToSolidColorBrush();
                _swatch6.Fill = blend.Colors[5].ToSolidColorBrush();
            }
        }
    }
}
