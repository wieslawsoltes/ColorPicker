using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Views;

public class OverviewThicknessView : UserControl
{
    public OverviewThicknessView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}