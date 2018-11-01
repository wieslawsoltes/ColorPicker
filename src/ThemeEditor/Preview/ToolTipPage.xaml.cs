using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Preview
{
    public class ToolTipPage : UserControl
    {
        public ToolTipPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
