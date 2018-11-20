using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
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
        private DropDown _themeSelector = null;
        private StyleInclude _lightTheme = null;
        private StyleInclude _darkTheme = null;

        public MainView()
        {
            this.InitializeComponent();
            _previewView = this.Find<ThemePreviewView>("previewView");
            _exportText = this.Find<TextBox>("exportText");
            _editorView = this.Find<ThemeEditorView>("editorView");
            _colorPicker = this.Find<ColorPicker>("colorPicker");
            _colorBlender = this.Find<ColorBlender>("colorBlender");
            _colorBlender.DataContext = new ColorMatchViewModel(199, 95, 62);
            _themeSelector = this.Find<DropDown>("themeSelector");
            _themeSelector.SelectionChanged += ThemeSelectionChanged;
            _lightTheme = AvaloniaXamlLoader.Parse<StyleInclude>(@"<StyleInclude xmlns='https://github.com/avaloniaui' Source='resm:Avalonia.Themes.Default.Accents.BaseLight.xaml?assembly=Avalonia.Themes.Default'/>");
            _darkTheme = AvaloniaXamlLoader.Parse<StyleInclude>(@"<StyleInclude xmlns='https://github.com/avaloniaui' Source='resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default'/>");
            Styles.Add(_darkTheme);
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
                var defaultThem = editor.GetTheme(_lightTheme);
                defaultThem.Name = "BaseLight";
                editor.DefaultTheme = defaultThem;
                editor.Attach(_previewView.Resources, (x) => _exportText.Text = x.ToXaml());
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

        private void ThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (_themeSelector.SelectedIndex)
            {
                case 0:
                    Styles[0]  = _lightTheme;
                    break;
                case 1:
                    Styles[0]  = _darkTheme;
                    break;
            }
        }
    }
}
