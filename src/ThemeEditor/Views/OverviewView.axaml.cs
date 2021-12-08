using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Views;

public class OverviewView : UserControl
{
    public OverviewView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}