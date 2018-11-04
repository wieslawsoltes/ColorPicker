using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ThemeEditor.Preview
{
    public class ListBoxPage : UserControl
    {
        public ListBoxPage()
        {
            this.InitializeComponent();
            DataContext = Enumerable.Range(1, 10).Select(i => $"Item {i}")
                .ToArray();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
