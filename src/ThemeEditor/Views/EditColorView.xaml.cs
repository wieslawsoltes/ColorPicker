using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Views
{
    public class EditColorView : UserControl
    {
        public EditColorView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
