using System;
using System.IO;
using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
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
            BuildAvaloniaApp().Start(AppMain, args);
        }

        static void AppMain(Application app, string[] args)
        {
            var vm = new ThemeEditorViewModel();
            if (File.Exists(s_ThemesPath))
            {
                vm.LoadFromFile(s_ThemesPath);
            }
            else
            {
                vm.LoadFromResource<App>(s_ThemesResource);
            }

            var window = new MainWindow
            {
                DataContext = vm,
            };

            app.Run(window);

            vm.SaveAsFile(s_ThemesPath);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .UseReactiveUI()
                         .LogToDebug();
    }
}
