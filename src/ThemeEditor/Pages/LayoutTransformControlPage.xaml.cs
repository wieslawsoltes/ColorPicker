using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Pages
{
    public class LayoutTransformControlPage : UserControl
    {
        public LayoutTransformControlPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
