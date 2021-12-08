using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Preview;

public class ContextMenuPage : UserControl
{
    public ContextMenuPage()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}