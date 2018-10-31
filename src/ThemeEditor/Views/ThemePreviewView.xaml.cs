using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Views
{
    public class ThemePreviewView : UserControl
    {
        public ThemePreviewView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
