using System;
using System.IO;
using Avalonia;
using ThemeEditor.ViewModels;

namespace ThemeEditor
{
    static class Program
    {
        static string s_ThemesPath = "Themes.themes";

        [STAThread]
        static void Main(string[] args)
        {
            var editor = new ThemeEditorViewModel();

            if (File.Exists(s_ThemesPath))
            {
                editor.Load(s_ThemesPath);
            }

            BuildAvaloniaApp().Start<MainWindow>(() => editor);

            editor.Save(s_ThemesPath);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .UseReactiveUI();
    }
}
