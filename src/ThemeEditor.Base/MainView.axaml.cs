using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Themes.Simple;
using ThemeEditor.ViewModels;
using ThemeEditor.Views;

namespace ThemeEditor;

public class MainView : UserControl
{
    private readonly SimpleTheme? _lightTheme;
    private readonly SimpleTheme? _darkTheme;

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
            
        _lightTheme = new SimpleTheme(new Uri("resm:Styles?assembly=ThemeEditor"))
        {
            Mode = SimpleThemeMode.Light
        };
            
        _darkTheme = new SimpleTheme(new Uri("resm:Styles?assembly=ThemeEditor"))
        {
            Mode = SimpleThemeMode.Dark
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
