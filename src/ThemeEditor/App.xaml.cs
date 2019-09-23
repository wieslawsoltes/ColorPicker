using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ThemeEditor.ViewModels;

namespace ThemeEditor
{
    public class App : Application
    {
        static string s_ThemesResource = "ThemeEditor.Themes.Themes.themes";
        static string s_ThemesPath = "Themes.themes";

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var mainWindowViewModel = new ThemeEditorViewModel();
    
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                if (File.Exists(s_ThemesPath))
                {
                    mainWindowViewModel.LoadFromFile(s_ThemesPath);
                }
                else
                {
                    mainWindowViewModel.LoadFromResource<App>(s_ThemesResource);
                }

                var mainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel,
                };

                desktopLifetime.MainWindow = mainWindow;

                desktopLifetime.Exit += (sennder, e) =>
                {
                    mainWindowViewModel.SaveAsFile(s_ThemesPath);
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
            {
                mainWindowViewModel.LoadFromResource<App>(s_ThemesResource);

                var mainView = new MainView()
                {
                    DataContext = mainWindowViewModel
                };

                singleViewLifetime.MainView = mainView;
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
