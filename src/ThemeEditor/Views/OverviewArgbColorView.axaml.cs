using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Views;

public class OverviewArgbColorView : UserControl
{
    public OverviewArgbColorView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}