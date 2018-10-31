using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ThemeEditor.ViewModels;

namespace ThemeEditor
{
    public class Designer
    {
        public static ThemeEditorViewModel ThemeEditor { get; set; }
    }

    public class App : Application
    {
        static App()
        {
            if (Design.IsDesignMode)
            {
                Designer.ThemeEditor = new ThemeEditorViewModel();
            }
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
