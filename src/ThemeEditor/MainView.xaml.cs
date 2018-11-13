using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ThemeEditor.Controls.ColorBlender;
using ThemeEditor.Controls.ColorPicker;
using ThemeEditor.Controls.ColorPicker.Converters;
using ThemeEditor.ViewModels;
using ThemeEditor.Views;

namespace ThemeEditor
{
    public class MainView : UserControl
    {
        public static readonly StyledProperty<string> HexProperty =
            AvaloniaProperty.Register<MainView, string>(nameof(Hex), "", validate: ValidateHex);

        public static readonly StyledProperty<byte> RedProperty =
            AvaloniaProperty.Register<MainView, byte>(nameof(Red), 0xFF, validate: ValidateRed);

        public static readonly StyledProperty<byte> GreenProperty =
            AvaloniaProperty.Register<MainView, byte>(nameof(Green), 0xFF, validate: ValidateGreen);

        public static readonly StyledProperty<byte> BlueProperty =
            AvaloniaProperty.Register<MainView, byte>(nameof(Blue), 0xFF, validate: ValidateBlue);

        private static string ValidateHex(MainView view, string hex)
        {
            if (!ColorHelpers.IsValidHexColor(hex))
            {
                throw new ArgumentException("Invalid Hex value.");
            }
            return hex;
        }

        private static byte ValidateRed(MainView view, byte red)
        {
            if (red < 0 || red > 255)
            {
                throw new ArgumentException("Invalid Red value.");
            }
            return red;
        }

        private static byte ValidateGreen(MainView view, byte green)
        {
            if (green < 0 || green > 255)
            {
                throw new ArgumentException("Invalid Green value.");
            }
            return green;
        }

        private static byte ValidateBlue(MainView view, byte blue)
        {
            if (blue < 0 || blue > 255)
            {
                throw new ArgumentException("Invalid Blue value.");
            }
            return blue;
        }

        private ThemePreviewView _previewView = null;
        private TextBox _exportText = null;
        private ThemeEditorView _editorView = null;
        private ColorPicker _colorPicker = null;
        private ColorBlender _colorBlender = null;
        private bool _updating = false;

        public MainView()
        {
            this.InitializeComponent();

            _previewView = this.Find<ThemePreviewView>("previewView");
            _exportText = this.Find<TextBox>("exportText");
            _editorView = this.Find<ThemeEditorView>("editorView");
            _colorPicker = this.Find<ColorPicker>("colorPicker");
            _colorBlender = this.Find<ColorBlender>("colorBlender");

            _colorPicker.GetObservable(ColorPicker.HueProperty).Subscribe(x => UpdateOnHsvaChange());
            _colorPicker.GetObservable(ColorPicker.SaturationProperty).Subscribe(x => UpdateOnHsvaChange());
            _colorPicker.GetObservable(ColorPicker.ValueProperty).Subscribe(x => UpdateOnHsvaChange());
            _colorPicker.GetObservable(ColorPicker.AlphaProperty).Subscribe(x => UpdateOnHsvaChange());
            this.GetObservable(MainView.HexProperty).Subscribe(x => UpdateOnHexChange());
            this.GetObservable(MainView.RedProperty).Subscribe(x => UpdateOnRGBChange());
            this.GetObservable(MainView.GreenProperty).Subscribe(x => UpdateOnRGBChange());
            this.GetObservable(MainView.BlueProperty).Subscribe(x => UpdateOnRGBChange());


            _colorBlender.DataContext = new ColorMatchViewModel(199, 95, 62);
        }

        public string Hex
        {
            get { return GetValue(HexProperty); }
            set { SetValue(HexProperty, value); }
        }

        public byte Red
        {
            get { return GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public byte Green
        {
            get { return GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public byte Blue
        {
            get { return GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            if (DataContext is ThemeEditorViewModel editor)
            {
                editor.Attach(Application.Current, _previewView.Resources, (x) => _exportText.Text = x.ToXaml());
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            if (DataContext is ThemeEditorViewModel editor)
            {
                editor.Detach();
            }
        }

        private Color GetColor()
        {
            return ColorHelpers.FromHSVA(
                _colorPicker.Hue,
                _colorPicker.Saturation,
                _colorPicker.Value,
                _colorPicker.Alpha);
        }

        private void UpdateRGB()
        {
            Color color = GetColor();
            Red = color.R;
            Green = color.G;
            Blue = color.B;
        }

        private void UpdateHex()
        {
            Color color = GetColor();
            Hex = ColorHelpers.ToHexColor(color);
        }

        private void UpdatePicker()
        {
            if (ColorHelpers.IsValidHexColor(Hex))
            {
                Color color = Color.Parse(Hex);
                ColorHelpers.FromColor(color,
                    out double h,
                    out double s,
                    out double v,
                    out double a);
                _colorPicker.Hue = h;
                _colorPicker.Saturation = s;
                _colorPicker.Value = v;
                _colorPicker.Alpha = a;
            }
        }

        private void UpdateOnHsvaChange()
        {
            if (_updating == false)
            {
                _updating = true;
                UpdateHex();
                UpdateRGB();
                _updating = false;
            }
        }

        private void UpdateOnHexChange()
        {
            if (_updating == false)
            {
                _updating = true;
                UpdatePicker();
                UpdateRGB();
                _updating = false;
            }
        }

        private void UpdateOnRGBChange()
        {
            if (_updating == false)
            {
                _updating = true;
                Color color = ColorHelpers.FromRGBA(Red, Green, Blue, _colorPicker.Alpha);
                Hex = ColorHelpers.ToHexColor(color);
                UpdatePicker();
                _updating = false;
            }
        }
    }
}
