using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Collections;
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
        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<ColorBlender, Color>(nameof(Color));

        public static readonly StyledProperty<AvaloniaList<IAlgorithm>> AlgorithmsProperty =
            AvaloniaProperty.Register<ColorBlender, AvaloniaList<IAlgorithm>>(nameof(Algorithms));

        public ColorBlender()
        {
            this.InitializeComponent();

            Algorithms = new AvaloniaList<IAlgorithm>(
                new IAlgorithm[]
                {
                    new Classic(),
                    new ColorExplorer(),
                    new SingleHue(),
                    new Complementary(),
                    new SplitComplementary(),
                    new Analogue(),
                    new Triadic(),
                    new Square()
                });

            var algorithms = this.FindControl<DropDown>("PART_Algorithms");
            var swatch1 = this.FindControl<Rectangle>("PART_Swatch1");
            var swatch2 = this.FindControl<Rectangle>("PART_Swatch2");
            var swatch3 = this.FindControl<Rectangle>("PART_Swatch3");
            var swatch4 = this.FindControl<Rectangle>("PART_Swatch4");
            var swatch5 = this.FindControl<Rectangle>("PART_Swatch5");
            var swatch6 = this.FindControl<Rectangle>("PART_Swatch6");

            algorithms.Items = Algorithms;
            algorithms.SelectedIndex = 0;
            algorithms.SelectionChanged += (sender, e) => Update();

            this.GetObservable(ColorProperty).Subscribe(x => Update());

            void Update()
            {
                if (algorithms?.SelectedItem is IAlgorithm algorithm)
                {
                    Blend blend = algorithm.Match(Color.ToHSV());
                    swatch1.Fill = blend.Colors[0].ToSolidColorBrush();
                    swatch2.Fill = blend.Colors[1].ToSolidColorBrush();
                    swatch3.Fill = blend.Colors[2].ToSolidColorBrush();
                    swatch4.Fill = blend.Colors[3].ToSolidColorBrush();
                    swatch5.Fill = blend.Colors[4].ToSolidColorBrush();
                    swatch6.Fill = blend.Colors[5].ToSolidColorBrush();
                }
            }
        }

        public Color Color
        {
            get { return GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public AvaloniaList<IAlgorithm> Algorithms
        {
            get { return GetValue(AlgorithmsProperty); }
            set { SetValue(AlgorithmsProperty, value); }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
