using System;
using System.IO;
//using System.Reflection;
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
                editor.LoadFromFile(s_ThemesPath);
            }
            //else
            //{
            //    editor.LoadFromResource<App>("ThemeEditor.Themes.Themes.themes");
            //}

            BuildAvaloniaApp().Start<MainWindow>(() => editor);

            editor.SaveAsFile(s_ThemesPath);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .UseReactiveUI();
    }
}
