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
        private readonly StyleInclude? _lightTheme;
        private readonly StyleInclude? _darkTheme;

        public MainView()
        {
            this.InitializeComponent();

            var themeSelector = this.Find<ComboBox>("themeSelector");

            themeSelector.SelectionChanged += (sender, e) =>
            {
                switch (themeSelector.SelectedIndex)
                {
                    case 0:
                        if (_lightTheme is not null)
                        {
                            Styles[0] = _lightTheme;
                        }
                        break;
                    case 1:
                        if (_darkTheme is not null)
                        {
                            Styles[0] = _darkTheme;
                        }
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
                if (_lightTheme is not null)
                {
                    var defaultThem = editor.GetTheme(_lightTheme);
                    defaultThem.Name = "BaseLight";
                    editor.DefaultTheme = defaultThem;
                }
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
