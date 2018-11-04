using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Controls.ColorPicker
{
    public class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
