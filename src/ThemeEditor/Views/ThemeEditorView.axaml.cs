using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Views;

public class ThemeEditorView : UserControl
{
    public ThemeEditorView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}