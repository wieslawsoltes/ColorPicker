using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Controls.ColorBlender
{
    public partial class Swatch : UserControl
    {
        internal Rectangle _col;

        public Swatch()
        {
            this.InitializeComponent();
            _col = this.FindControl<Rectangle>("col");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
