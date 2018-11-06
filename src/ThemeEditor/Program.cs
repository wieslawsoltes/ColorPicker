using System;
using System.IO;
using Avalonia;
using ThemeEditor.ViewModels;

namespace ThemeEditor
{
    static class Program
    {
        static string s_ThemesResource = "ThemeEditor.Themes.Themes.themes";
        static string s_ThemesPath = "Themes.themes";

        [STAThread]
        static void Main(string[] args)
        {
            ThemeEditorViewModel vm = default;

            BuildAvaloniaApp().BeforeStarting(builder =>
            {
                vm = new ThemeEditorViewModel();
                if (File.Exists(s_ThemesPath))
                {
                    vm.LoadFromFile(s_ThemesPath);
                }
                else
                {
                    vm.LoadFromResource<App>(s_ThemesResource);
                }
            }).Start<MainWindow>(() => vm);

            vm.SaveAsFile(s_ThemesPath);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .UseReactiveUI();
    }
}
