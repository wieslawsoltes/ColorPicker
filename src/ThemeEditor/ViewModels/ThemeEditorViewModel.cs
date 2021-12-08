using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Threading;
using ReactiveUI;
using ThemeEditor.ViewModels.Serializer;

namespace ThemeEditor.ViewModels;

[DataContract]
public class ThemeEditorViewModel : ReactiveObject
{
    private IList<ThemeViewModel>? _themes;
    private ThemeViewModel? _currentTheme;
    private ThemeViewModel? _defaultTheme;
    private IDisposable? _themeObservable;
    private IDisposable? _editorObservable;
    private readonly ViewModelsSerializer? _serializer;

    [DataMember]
    public IList<ThemeViewModel>? Themes
    {
        get { return _themes; }
        set { this.RaiseAndSetIfChanged(ref _themes, value); }
    }

    [IgnoreDataMember]
    public ThemeViewModel? CurrentTheme
    {
        get { return _currentTheme; }
        set { this.RaiseAndSetIfChanged(ref _currentTheme, value); }
    }

    [IgnoreDataMember]
    public ThemeViewModel? DefaultTheme
    {
        get { return _defaultTheme; }
        set { this.RaiseAndSetIfChanged(ref _defaultTheme, value); }
    }

    public ReactiveCommand<Unit, Unit> ResetThemeCommand { get; }

    public ReactiveCommand<Unit, Unit> RemoveThemeCommand { get; }

    public ReactiveCommand<Unit, Unit> AddThemeCommand { get; }

    public ReactiveCommand<Unit, Unit> LoadThemesCommand { get; }

    public ReactiveCommand<Unit, Unit> SaveThemesCommand { get; }

    public ReactiveCommand<Unit, Unit> ExportThemeCommand { get; }

    public ThemeEditorViewModel()
    {
        _serializer = new ViewModelsSerializer();
        ResetThemeCommand = ReactiveCommand.Create(ResetTheme);
        RemoveThemeCommand = ReactiveCommand.Create(RemoveTheme);
        AddThemeCommand = ReactiveCommand.Create(AddTheme);
        LoadThemesCommand = ReactiveCommand.CreateFromTask(LoadThemes);
        SaveThemesCommand = ReactiveCommand.CreateFromTask(SaveThemes);
        ExportThemeCommand = ReactiveCommand.CreateFromTask(ExportTheme);
    }

    public string? GetResource<T>(string name)
    {
        var assembly = typeof(T).GetTypeInfo().Assembly;
        var stream = assembly.GetManifestResourceStream(name);
        if (stream != null)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        return null;
    }

    public void LoadFromJson(string json)
    {
        var themes = _serializer?.Deserialize<ObservableCollection<ThemeViewModel>>(json);
        Themes = themes;
        CurrentTheme = themes?.FirstOrDefault();
    }

    public void LoadFromResource<T>(string name)
    {
        var json = GetResource<T>(name);
        if (json != null)
        {
            LoadFromJson(json);
        }
    }

    public void LoadFromFile(string path)
    {
        var json = File.ReadAllText(path);
        LoadFromJson(json);
    }

    public void SaveAsFile(string path)
    {
        var json = _serializer?.Serialize(Themes);
        if (json != null)
        {
            File.WriteAllText(path, json); 
        }
    }

    public void ExportAsFile(string path, ThemeViewModel theme)
    {
        var xaml = theme.ToXaml();
        File.WriteAllText(path, xaml);
    }

    public void ResetTheme(ThemeViewModel theme)
    {
        if (Themes != null && CurrentTheme != null)
        {
            int index = Themes.IndexOf(CurrentTheme);
            if (index >= 0)
            {
                string name = CurrentTheme.Name;
                Themes[index] = theme;
                CurrentTheme = theme;
                CurrentTheme.Name = name;
            }
        }
    }

    public void RemoveTheme(ThemeViewModel theme)
    {
        if (Themes != null)
        {
            Themes.Remove(theme);
            CurrentTheme = Themes.FirstOrDefault(); 
        }
    }

    public void AddTheme(ThemeViewModel theme)
    {
        if (Themes != null)
        {
            Themes.Add(theme);
            CurrentTheme = theme; 
        }
    }

    public void ResetTheme()
    {
        var clone = DefaultTheme?.Clone();
        if (clone != null)
        {
            ResetTheme();
        }
    }

    public void RemoveTheme()
    {
        if (CurrentTheme != null)
        {
            RemoveTheme(CurrentTheme); 
        }
    }

    public void AddTheme()
    {
        if (Themes != null)
        {
            var theme = Themes.Count == 0 || CurrentTheme == null ? DefaultTheme?.Clone() : CurrentTheme?.Clone();
            if (theme != null)
            {
                if (Themes.Count > 0 && Themes.Count(x => x.Name == theme.Name) > 0)
                {
                    theme.Name += "-copy";
                }
                AddTheme(theme); 
            }
        }
    }

    public async Task LoadThemes()
    {
        var dlg = new OpenFileDialog() { Title = "Load" };
        dlg.Filters.Add(new FileDialogFilter() { Name = "Themes", Extensions = { "themes" } });
        dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
        var window = GetWindow();
        if (window is null)
        {
            return;
        }
        var result = await dlg.ShowAsync(window);
        if (result != null)
        {
            var path = result.FirstOrDefault();
            if (path != null)
            {
                LoadFromFile(path);
            }
        }
    }

    public async Task SaveThemes()
    {
        var dlg = new SaveFileDialog() { Title = "Save" };
        dlg.Filters.Add(new FileDialogFilter() { Name = "Themes", Extensions = { "themes" } });
        dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
        dlg.InitialFileName = "Themes";
        dlg.DefaultExtension = "themes";
        var window = GetWindow();
        if (window is null)
        {
            return;
        }
        var result = await dlg.ShowAsync(window);
        if (result != null)
        {
            SaveAsFile(result);
        }
    }

    public async Task ExportTheme()
    {
        if (CurrentTheme != null)
        {
            var dlg = new SaveFileDialog() { Title = "Save" };
            dlg.Filters.Add(new FileDialogFilter() { Name = "Xaml", Extensions = { "xaml" } });
            dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
            dlg.InitialFileName = CurrentTheme.Name;
            dlg.DefaultExtension = "xaml";
            var window = GetWindow();
            if (window is null)
            {
                return;
            }
            var result = await dlg.ShowAsync(window);
            if (result != null)
            {
                ExportAsFile(result, CurrentTheme);
            } 
        }
    }

    private ArgbColorViewModel? GetColorResource(IResourceNode node, string key)
    {
        if (node.TryGetResource(key, out var resource) && resource is Color color)
        {
            return color.ArgbFromColor();
        }
        return null;
    }

    private ThicknessViewModel? GetThicknessResource(IResourceNode node, string key)
    {
        if (node.TryGetResource(key, out var resource) && resource is Thickness thickness)
        {
            return thickness.FromThickness();
        }
        return null;
    }

    private double GetDoubleResource(IResourceNode node, string key)
    {
        if (node.TryGetResource(key, out var resource) && resource is double value)
        {
            return value;
        }
        return default;
    }

    public ThemeViewModel GetTheme(IResourceNode node)
    {
        return new ThemeViewModel
        {
            ThemeAccentColor = GetColorResource(node, "ThemeAccentColor"),
            ThemeAccentColor2 = GetColorResource(node, "ThemeAccentColor2"),
            ThemeAccentColor3 = GetColorResource(node, "ThemeAccentColor3"),
            ThemeAccentColor4 = GetColorResource(node, "ThemeAccentColor4"),
            ThemeBackgroundColor = GetColorResource(node, "ThemeBackgroundColor"),
            ThemeBorderLowColor = GetColorResource(node, "ThemeBorderLowColor"),
            ThemeBorderMidColor = GetColorResource(node, "ThemeBorderMidColor"),
            ThemeBorderHighColor = GetColorResource(node, "ThemeBorderHighColor"),
            ThemeControlLowColor = GetColorResource(node, "ThemeControlLowColor"),
            ThemeControlMidColor = GetColorResource(node, "ThemeControlMidColor"),
            ThemeControlHighColor = GetColorResource(node, "ThemeControlHighColor"),
            ThemeControlHighlightLowColor = GetColorResource(node, "ThemeControlHighlightLowColor"),
            ThemeControlHighlightMidColor = GetColorResource(node, "ThemeControlHighlightMidColor"),
            ThemeControlHighlightHighColor = GetColorResource(node, "ThemeControlHighlightHighColor"),
            ThemeForegroundColor = GetColorResource(node, "ThemeForegroundColor"),
            ThemeForegroundLowColor = GetColorResource(node, "ThemeForegroundLowColor"),
            HighlightColor = GetColorResource(node, "HighlightColor"),
            ErrorColor = GetColorResource(node, "ErrorColor"),
            ErrorLowColor = GetColorResource(node, "ErrorLowColor"),
            ThemeBorderThickness = GetThicknessResource(node, "ThemeBorderThickness"),
            ThemeDisabledOpacity = GetDoubleResource(node, "ThemeDisabledOpacity"),
            FontSizeSmall = GetDoubleResource(node, "FontSizeSmall"),
            FontSizeNormal = GetDoubleResource(node, "FontSizeNormal"),
            FontSizeLarge = GetDoubleResource(node, "FontSizeLarge"),
            ScrollBarThickness = GetDoubleResource(node, "ScrollBarThickness")
        };
    }

    private void UpdateThemeAccent(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeAccentColor != null)
        {
            resources["ThemeAccentColor"] = theme.ThemeAccentColor.ToColor();
            resources["ThemeAccentBrush"] = theme.ThemeAccentColor.ToBrush(); 
        }
    }

    private void UpdateThemeAccent2(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeAccentColor2 != null)
        {
            resources["ThemeAccentColor2"] = theme.ThemeAccentColor2.ToColor();
            resources["ThemeAccentBrush2"] = theme.ThemeAccentColor2.ToBrush(); 
        }
    }

    private void UpdateThemeAccent3(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeAccentColor3 != null)
        {
            resources["ThemeAccentColor3"] = theme.ThemeAccentColor3.ToColor();
            resources["ThemeAccentBrush3"] = theme.ThemeAccentColor3.ToBrush(); 
        }
    }

    private void UpdateThemeAccent4(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeAccentColor4 != null)
        {
            resources["ThemeAccentColor4"] = theme.ThemeAccentColor4.ToColor();
            resources["ThemeAccentBrush4"] = theme.ThemeAccentColor4.ToBrush(); 
        }
    }

    private void UpdateThemeBackground(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeBackgroundColor != null)
        {
            resources["ThemeBackgroundColor"] = theme.ThemeBackgroundColor.ToColor();
            resources["ThemeBackgroundBrush"] = theme.ThemeBackgroundColor.ToBrush(); 
        }
    }

    private void UpdateThemeBorderLow(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeBorderLowColor != null)
        {
            resources["ThemeBorderLowColor"] = theme.ThemeBorderLowColor.ToColor();
            resources["ThemeBorderLowBrush"] = theme.ThemeBorderLowColor.ToBrush(); 
        }
    }

    private void UpdateThemeBorderMid(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeBorderMidColor != null)
        {
            resources["ThemeBorderMidColor"] = theme.ThemeBorderMidColor.ToColor();
            resources["ThemeBorderMidBrush"] = theme.ThemeBorderMidColor.ToBrush(); 
        }
    }

    private void UpdateThemeBorderHigh(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeBorderHighColor != null)
        {
            resources["ThemeBorderHighColor"] = theme.ThemeBorderHighColor.ToColor();
            resources["ThemeBorderHighBrush"] = theme.ThemeBorderHighColor.ToBrush(); 
        }
    }

    private void UpdateThemeControlLow(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeControlLowColor != null)
        {
            resources["ThemeControlLowColor"] = theme.ThemeControlLowColor.ToColor();
            resources["ThemeControlLowBrush"] = theme.ThemeControlLowColor.ToBrush(); 
        }
    }

    private void UpdateThemeControlMid(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeControlMidColor != null)
        {
            resources["ThemeControlMidColor"] = theme.ThemeControlMidColor.ToColor();
            resources["ThemeControlMidBrush"] = theme.ThemeControlMidColor.ToBrush(); 
        }
    }

    private void UpdateThemeControlHigh(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeControlHighColor != null)
        {
            resources["ThemeControlHighColor"] = theme.ThemeControlHighColor.ToColor();
            resources["ThemeControlHighBrush"] = theme.ThemeControlHighColor.ToBrush(); 
        }
    }

    private void UpdateThemeControlHighlightLow(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeControlHighlightLowColor != null)
        {
            resources["ThemeControlHighlightLowColor"] = theme.ThemeControlHighlightLowColor.ToColor();
            resources["ThemeControlHighlightLowBrush"] = theme.ThemeControlHighlightLowColor.ToBrush(); 
        }
    }

    private void UpdateThemeControlHighlightMid(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeControlHighlightMidColor != null)
        {
            resources["ThemeControlHighlightMidColor"] = theme.ThemeControlHighlightMidColor.ToColor();
            resources["ThemeControlHighlightMidBrush"] = theme.ThemeControlHighlightMidColor.ToBrush(); 
        }
    }

    private void UpdateThemeControlHighlightHigh(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeControlHighlightHighColor != null)
        {
            resources["ThemeControlHighlightHighColor"] = theme.ThemeControlHighlightHighColor.ToColor();
            resources["ThemeControlHighlightHighBrush"] = theme.ThemeControlHighlightHighColor.ToBrush(); 
        }
    }

    private void UpdateThemeForeground(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeForegroundColor != null)
        {
            resources["ThemeForegroundColor"] = theme.ThemeForegroundColor.ToColor();
            resources["ThemeForegroundBrush"] = theme.ThemeForegroundColor.ToBrush(); 
        }
    }

    private void UpdateThemeForegroundLow(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeForegroundLowColor != null)
        {
            resources["ThemeForegroundLowColor"] = theme.ThemeForegroundLowColor.ToColor();
            resources["ThemeForegroundLowBrush"] = theme.ThemeForegroundLowColor.ToBrush(); 
        }
    }

    private void UpdateHighlight(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.HighlightColor != null)
        {
            resources["HighlightColor"] = theme.HighlightColor.ToColor();
            resources["HighlightBrush"] = theme.HighlightColor.ToBrush(); 
        }
    }

    private void UpdateError(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ErrorColor != null)
        {
            resources["ErrorColor"] = theme.ErrorColor.ToColor();
            resources["ErrorBrush"] = theme.ErrorColor.ToBrush(); 
        }
    }

    private void UpdateErrorLow(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ErrorLowColor != null)
        {
            resources["ErrorLowColor"] = theme.ErrorLowColor.ToColor();
            resources["ErrorLowBrush"] = theme.ErrorLowColor.ToBrush(); 
        }
    }

    private void UpdateThemeBorderThickness(IResourceDictionary resources, ThemeViewModel theme)
    {
        if (theme.ThemeBorderThickness != null)
        {
            resources["ThemeBorderThickness"] = theme.ThemeBorderThickness.ToThickness(); 
        }
    }

    private void UpdateThemeDisabledOpacity(IResourceDictionary resources, ThemeViewModel theme)
    {
        resources["ThemeDisabledOpacity"] = theme.ThemeDisabledOpacity;
    }

    private void UpdateFontSizeSmall(IResourceDictionary resources, ThemeViewModel theme)
    {
        resources["FontSizeSmall"] = theme.FontSizeSmall;
    }

    private void UpdateFontSizeNormal(IResourceDictionary resources, ThemeViewModel theme)
    {
        resources["FontSizeNormal"] = theme.FontSizeNormal;
    }

    private void UpdateUpdateFontSizeLarge(IResourceDictionary resources, ThemeViewModel theme)
    {
        resources["FontSizeLarge"] = theme.FontSizeLarge;
    }

    private void UpdateUpdateScrollBarThickness(IResourceDictionary resources, ThemeViewModel theme)
    {
        resources["ScrollBarThickness"] = theme.ScrollBarThickness;
    }

    public void UpdateTheme(IResourceDictionary resources, ThemeViewModel theme)
    {
        UpdateThemeAccent(resources, theme);
        UpdateThemeAccent2(resources, theme);
        UpdateThemeAccent3(resources, theme);
        UpdateThemeAccent4(resources, theme);
        UpdateThemeBackground(resources, theme);
        UpdateThemeBorderLow(resources, theme);
        UpdateThemeBorderMid(resources, theme);
        UpdateThemeBorderHigh(resources, theme);
        UpdateThemeControlLow(resources, theme);
        UpdateThemeControlMid(resources, theme);
        UpdateThemeControlHigh(resources, theme);
        UpdateThemeControlHighlightLow(resources, theme);
        UpdateThemeControlHighlightMid(resources, theme);
        UpdateThemeControlHighlightHigh(resources, theme);
        UpdateThemeForeground(resources, theme);
        UpdateThemeForegroundLow(resources, theme);
        UpdateHighlight(resources, theme);
        UpdateError(resources, theme);
        UpdateErrorLow(resources, theme);
        UpdateThemeBorderThickness(resources, theme);
        UpdateThemeDisabledOpacity(resources, theme);
        UpdateFontSizeSmall(resources, theme);
        UpdateFontSizeNormal(resources, theme);
        UpdateUpdateFontSizeLarge(resources, theme);
        UpdateUpdateScrollBarThickness(resources, theme);
    }

    public IDisposable ObserveTheme(IResourceDictionary resources, ThemeViewModel theme, Action<ThemeViewModel>? preview = null)
    {
        var disposable = new CompositeDisposable();

        Observe(theme, UpdateTheme);
        Observe(theme.ThemeAccentColor, UpdateThemeAccent);
        Observe(theme.ThemeAccentColor2, UpdateThemeAccent2);
        Observe(theme.ThemeAccentColor3, UpdateThemeAccent3);
        Observe(theme.ThemeAccentColor4, UpdateThemeAccent4);
        Observe(theme.ThemeBackgroundColor, UpdateThemeBackground);
        Observe(theme.ThemeBorderLowColor, UpdateThemeBorderLow);
        Observe(theme.ThemeBorderMidColor, UpdateThemeBorderMid);
        Observe(theme.ThemeBorderHighColor, UpdateThemeBorderHigh);
        Observe(theme.ThemeControlLowColor, UpdateThemeControlLow);
        Observe(theme.ThemeControlMidColor, UpdateThemeControlMid);
        Observe(theme.ThemeControlHighColor, UpdateThemeControlHigh);
        Observe(theme.ThemeControlHighlightLowColor, UpdateThemeControlHighlightLow);
        Observe(theme.ThemeControlHighlightMidColor, UpdateThemeControlHighlightMid);
        Observe(theme.ThemeControlHighlightHighColor, UpdateThemeControlHighlightHigh);
        Observe(theme.ThemeForegroundColor, UpdateThemeForeground);
        Observe(theme.ThemeForegroundLowColor, UpdateThemeForegroundLow);
        Observe(theme.HighlightColor, UpdateHighlight);
        Observe(theme.ErrorColor, UpdateError);
        Observe(theme.ErrorLowColor, UpdateErrorLow);
        Observe(theme.ThemeBorderThickness, UpdateThemeBorderThickness);

        if (resources != null)
        {
            UpdateTheme(resources, theme);
        }
        preview?.Invoke(theme);

        return disposable;

        void Observe(IReactiveNotifyPropertyChanged<IReactiveObject>? value, Action<IResourceDictionary, ThemeViewModel> update)
        {
            if (value != null)
            {
                disposable.Add(value.Changed.Subscribe(x =>
                {
                    if (x.PropertyName != nameof(ThemeViewModel.Name))
                    {
                        if (resources != null)
                        {
                            update(resources, theme);
                        }
                        preview?.Invoke(theme);
                    }
                }));
            }
        }
    }

    public void Attach(IResourceDictionary resources, Action<ThemeViewModel>? preview = null)
    {
        if (Themes == null)
        {
            CurrentTheme = DefaultTheme?.Clone();
            Themes = new ObservableCollection<ThemeViewModel>();
            if (CurrentTheme != null)
            {
                Themes.Add(CurrentTheme);
            }
        }
        else
        {
            CurrentTheme = Themes.FirstOrDefault();
        }

        if (CurrentTheme != null)
        {
            _themeObservable = ObserveTheme(resources, CurrentTheme, preview);

            _editorObservable = Changed.Subscribe(x =>
            {
                if (x.PropertyName == nameof(CurrentTheme))
                {
                    _themeObservable?.Dispose();
                    if (CurrentTheme != null)
                    {
                        _themeObservable = ObserveTheme(resources, CurrentTheme, preview);
                    }
                }
            }); 
        }
    }

    public void Detach()
    {
        _themeObservable?.Dispose();
        _editorObservable?.Dispose();
    }

    private Window? GetWindow()
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            return desktopLifetime.MainWindow;
        }
        return null;
    }
}
