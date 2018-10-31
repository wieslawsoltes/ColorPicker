using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Pages
{
    public class CheckBoxPage : UserControl
    {
        public CheckBoxPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
