using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Views
{
    public class EditView : UserControl
    {
        public EditView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
