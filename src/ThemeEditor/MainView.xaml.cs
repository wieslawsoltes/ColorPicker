using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using ThemeEditor.ViewModels;
using ThemeEditor.Views;

namespace ThemeEditor
{
    public class MainView : UserControl
    {
        private StyleInclude _lightTheme = null;
        private StyleInclude _darkTheme = null;

        public MainView()
        {
            this.InitializeComponent();

            var _themeSelector = this.Find<ComboBox>("themeSelector");
            _themeSelector.SelectionChanged += (sender, e) =>
            {
                switch (_themeSelector.SelectedIndex)
                {
                    case 0:
                        Styles[0] = _lightTheme;
                        break;
                    case 1:
                        Styles[0] = _darkTheme;
                        break;
                }
            };
            _lightTheme = new StyleInclude(new Uri("resm:Styles?assembly=ThemeEditor"))
            {
                Source = new Uri("resm:Avalonia.Themes.Default.Accents.BaseLight.xaml?assembly=Avalonia.Themes.Default")
            };
            _darkTheme = new StyleInclude(new Uri("resm:Styles?assembly=ThemeEditor"))
            {
                Source = new Uri("resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default")
            };
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
                var previewView = this.Find<ThemePreviewView>("previewView");
                var exportText = this.Find<TextBox>("exportText");
                var defaultThem = editor.GetTheme(_lightTheme);
                defaultThem.Name = "BaseLight";
                editor.DefaultTheme = defaultThem;
                editor.Attach(previewView.Resources, (x) => exportText.Text = x.ToXaml());
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
