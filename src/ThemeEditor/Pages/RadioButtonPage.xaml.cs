using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Pages
{
    public class RadioButtonPage : UserControl
    {
        public RadioButtonPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
