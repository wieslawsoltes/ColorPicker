using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Views;

public class EditThicknessView : UserControl
{
    public EditThicknessView()
    {
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}