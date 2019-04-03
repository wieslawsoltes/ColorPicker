using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Preview
{
    public class ComboBoxPage : UserControl
    {
        public ComboBoxPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
