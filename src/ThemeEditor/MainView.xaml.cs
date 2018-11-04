using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ThemeEditor.Controls.ColorBlender;
using ThemeEditor.Controls.ColorPicker;
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

        public MainView()
        {
            this.InitializeComponent();

            _previewView = this.Find<ThemePreviewView>("previewView");
            _exportText = this.Find<TextBox>("exportText");
            _editorView = this.Find<ThemeEditorView>("editorView");
            _colorPicker = this.Find<ColorPicker>("colorPicker");
            _colorBlender = this.Find<ColorBlender>("colorBlender");

            _colorBlender.DataContext = new ColorMatchViewModel(213, 46, 49);
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
    }
}
