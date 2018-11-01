using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Preview
{
    public class DropDownPage : UserControl
    {
        public DropDownPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
