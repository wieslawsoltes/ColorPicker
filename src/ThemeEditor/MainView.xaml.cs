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
        private ThemePreviewView _previewView = null;
        private TextBox _exportText = null;
        private ThemeEditorView _editorView = null;
        private ColorPicker _colorPicker = null;
        private ColorBlender _colorBlender = null;
        private TextBox _hexText = null;
        private bool _updating = false;

        public MainView()
        {
            this.InitializeComponent();

            _previewView = this.Find<ThemePreviewView>("previewView");
            _exportText = this.Find<TextBox>("exportText");
            _editorView = this.Find<ThemeEditorView>("editorView");
            _colorPicker = this.Find<ColorPicker>("colorPicker");
            _colorBlender = this.Find<ColorBlender>("colorBlender");
            _hexText = this.Find<TextBox>("hexText");

            _colorPicker.GetObservable(ColorPicker.HueProperty).Subscribe(x => UpdateOnHsvaChange());
            _colorPicker.GetObservable(ColorPicker.SaturationProperty).Subscribe(x => UpdateOnHsvaChange());
            _colorPicker.GetObservable(ColorPicker.ValueProperty).Subscribe(x => UpdateOnHsvaChange());
            _colorPicker.GetObservable(ColorPicker.AlphaProperty).Subscribe(x => UpdateOnHsvaChange());
            _hexText.GetObservable(TextBox.TextProperty).Subscribe(x => UpdateOnHexChange());

            _colorBlender.DataContext = new ColorMatchViewModel(199, 95, 62);
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

        private void UpdateHex()
        {
            Color color = ColorHelpers.ToColor(
                _colorPicker.Hue,
                _colorPicker.Saturation,
                _colorPicker.Value,
                _colorPicker.Alpha);
            _hexText.Text = ColorHelpers.ToHexColor(color);
        }

        private void UpdatePicker()
        {
            string hex = _hexText.Text;
            if (ColorHelpers.IsValidHexColor(hex))
            {
                Color color = Color.Parse(hex);
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
                _updating = false;
            }
        }

        private void UpdateOnHexChange()
        {
            if (_updating == false)
            {
                _updating = true;
                UpdatePicker();
                _updating = false;
            }
        }
    }
}
