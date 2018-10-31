using Avalonia;
using Avalonia.Markup.Xaml;

namespace ThemeEditor
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
