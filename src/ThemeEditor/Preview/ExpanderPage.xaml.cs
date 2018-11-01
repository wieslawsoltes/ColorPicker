using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Preview
{
    public class ExpanderPage : UserControl
    {
        public ExpanderPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
