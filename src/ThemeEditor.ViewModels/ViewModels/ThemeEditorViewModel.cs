using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.Serialization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Legacy;

namespace ThemeEditor.ViewModels
{
    [DataContract]
    public class ThemeEditorViewModel : ReactiveObject
    {
#pragma warning disable CS0618
        private IReactiveList<ThemeViewModel> _themes;
#pragma warning restore CS0618
        private ThemeViewModel _currentTheme;
        private ThemeViewModel _defaultTheme;
        private IDisposable _themeObservable;
        private IDisposable _editorObservable;

#pragma warning disable CS0618
        [DataMember]
        public IReactiveList<ThemeViewModel> Themes
        {
            get { return _themes; }
            set { this.RaiseAndSetIfChanged(ref _themes, value); }
        }
#pragma warning restore CS0618

        [IgnoreDataMember]
        public ThemeViewModel CurrentTheme
        {
            get { return _currentTheme; }
            set { this.RaiseAndSetIfChanged(ref _currentTheme, value); }
        }

        [IgnoreDataMember]
        public ThemeViewModel DefaultTheme
        {
            get { return _defaultTheme; }
            set { this.RaiseAndSetIfChanged(ref _defaultTheme, value); }
        }

        public void Load(string path)
        {
            var json = File.ReadAllText(path);
#pragma warning disable CS0618
            var themes = Deserialize<ReactiveList<ThemeViewModel>>(json);
#pragma warning restore CS0618
            Themes = themes;
            CurrentTheme = themes.FirstOrDefault();
        }

        public void Save(string path)
        {
            var json = Serialize(Themes);
            File.WriteAllText(path, json);
        }

        public void Export(string path, ThemeViewModel theme)
        {
            var xaml = theme.ToXaml();
            File.WriteAllText(path, xaml);
        }

        public void Reset(ThemeViewModel theme)
        {
            if (CurrentTheme != null)
            {
                int index = Themes.IndexOf(CurrentTheme);
                if (index >= 0)
                {
                    Themes[index] = theme;
                    CurrentTheme = theme;
                }
            }
        }

        public void Remove(ThemeViewModel theme)
        {
            Themes.Remove(theme);
            CurrentTheme = Themes.FirstOrDefault();
        }

        public void Add(ThemeViewModel theme)
        {
            Themes.Add(theme);
            CurrentTheme = theme;
        }

        public void ResetCommand()
        {
            Reset(DefaultTheme.Clone());
        }

        public void RemoveCommand()
        {
            Remove(CurrentTheme);
        }

        public void AddCommand()
        {
            var theme = Themes.Count == 0 || CurrentTheme == null ? DefaultTheme.Clone() : CurrentTheme.Clone();
            if (Themes.Count > 0 && Themes.Count(x => x.Name == theme.Name) > 0)
            {
                theme.Name += "-copy";
            }
            Add(theme);
        }

        public async void LoadCommand()
        {
            var dlg = new OpenFileDialog() { Title = "Load" };
            dlg.Filters.Add(new FileDialogFilter() { Name = "Themes", Extensions = { "themes" } });
            dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
            var result = await dlg.ShowAsync(Application.Current.Windows.FirstOrDefault());
            if (result != null && result[0] != null)
            {
                var path = result.FirstOrDefault();
                Load(path);
            }
        }

        public async void SaveCommand()
        {
            var dlg = new SaveFileDialog() { Title = "Save" };
            dlg.Filters.Add(new FileDialogFilter() { Name = "Themes", Extensions = { "themes" } });
            dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
            dlg.InitialFileName = "Themes";
            dlg.DefaultExtension = "themes";
            var result = await dlg.ShowAsync(Application.Current.Windows.FirstOrDefault());
            if (result != null)
            {
                Save(result);
            }
        }

        public async void ExportCommand()
        {
            var dlg = new SaveFileDialog() { Title = "Save" };
            dlg.Filters.Add(new FileDialogFilter() { Name = "Xaml", Extensions = { "xaml" } });
            dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
            dlg.InitialFileName = CurrentTheme.Name;
            dlg.DefaultExtension = "xaml";
            var result = await dlg.ShowAsync(Application.Current.Windows.FirstOrDefault());
            if (result != null)
            {
                Export(result, CurrentTheme);
            }
        }

        private class ColorViewModelConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ColorViewModel);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                switch (value as ColorViewModel)
                {
                    case ColorViewModel color:
                        writer.WriteValue(color.ToHexString());
                        break;
                    default:
                        throw new NotSupportedException($"The {value.GetType()} type is not supported.");
                }
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (objectType == typeof(ColorViewModel))
                {
                    return Color.Parse((string)reader.Value).FromColor();
                }
                throw new ArgumentException("objectType");
            }
        }

        private class ThicknessViewModelConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ThicknessViewModel);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                switch (value as ThicknessViewModel)
                {
                    case ThicknessViewModel thickness:
                        writer.WriteValue(thickness.ToThickness().ToString());
                        break;
                    default:
                        throw new NotSupportedException($"The {value.GetType()} type is not supported.");
                }
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (objectType == typeof(ThicknessViewModel))
                {
                    return Thickness.Parse((string)reader.Value).FromThickness();
                }
                throw new ArgumentException("objectType");
            }
        }

        private string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(
                value,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    Converters =
                    {
                        new ColorViewModelConverter(),
                        new ThicknessViewModelConverter()
                    }
                });
        }

        private T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(
                json,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    Converters =
                    {
                        new ColorViewModelConverter(),
                        new ThicknessViewModelConverter()
                    }
                });
        }

        private ColorViewModel GetColorResource(IResourceNode node, string key)
        {
            return ((Color)node.FindResource(key)).FromColor();
        }

        private ThicknessViewModel GetThicknessResource(IResourceNode node, string key)
        {
            return ((Thickness)node.FindResource(key)).FromThickness();
        }

        private double GetDoubleResource(IResourceNode node, string key)
        {
            return (double)node.FindResource(key);
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
                FontSizeLarge = GetDoubleResource(node, "FontSizeLarge")
            };
        }

        private void UpdateThemeAccent(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeAccentColor"] = theme.ThemeAccentColor.ToColor();
            resources["ThemeAccentBrush"] = theme.ThemeAccentColor.ToBrush();
        }

        private void UpdateThemeAccent2(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeAccentColor2"] = theme.ThemeAccentColor2.ToColor();
            resources["ThemeAccentBrush2"] = theme.ThemeAccentColor2.ToBrush();
        }

        private void UpdateThemeAccent3(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeAccentColor3"] = theme.ThemeAccentColor3.ToColor();
            resources["ThemeAccentBrush3"] = theme.ThemeAccentColor3.ToBrush();
        }

        private void UpdateThemeAccent4(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeAccentColor4"] = theme.ThemeAccentColor4.ToColor();
            resources["ThemeAccentBrush4"] = theme.ThemeAccentColor4.ToBrush();
        }

        private void UpdateThemeBackground(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeBackgroundColor"] = theme.ThemeBackgroundColor.ToColor();
            resources["ThemeBackgroundBrush"] = theme.ThemeBackgroundColor.ToBrush();
        }

        private void UpdateThemeBorderLow(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeBorderLowColor"] = theme.ThemeBorderLowColor.ToColor();
            resources["ThemeBorderLowBrush"] = theme.ThemeBorderLowColor.ToBrush();
        }

        private void UpdateThemeBorderMid(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeBorderMidColor"] = theme.ThemeBorderMidColor.ToColor();
            resources["ThemeBorderMidBrush"] = theme.ThemeBorderMidColor.ToBrush();
        }

        private void UpdateThemeBorderHigh(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeBorderHighColor"] = theme.ThemeBorderHighColor.ToColor();
            resources["ThemeBorderHighBrush"] = theme.ThemeBorderHighColor.ToBrush();
        }

        private void UpdateThemeControlLow(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeControlLowColor"] = theme.ThemeControlLowColor.ToColor();
            resources["ThemeControlLowBrush"] = theme.ThemeControlLowColor.ToBrush();
        }

        private void UpdateThemeControlMid(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeControlMidColor"] = theme.ThemeControlMidColor.ToColor();
            resources["ThemeControlMidBrush"] = theme.ThemeControlMidColor.ToBrush();
        }

        private void UpdateThemeControlHigh(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeControlHighColor"] = theme.ThemeControlHighColor.ToColor();
            resources["ThemeControlHighBrush"] = theme.ThemeControlHighColor.ToBrush();
        }

        private void UpdateThemeControlHighlightLow(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeControlHighlightLowColor"] = theme.ThemeControlHighlightLowColor.ToColor();
            resources["ThemeControlHighlightLowBrush"] = theme.ThemeControlHighlightLowColor.ToBrush();
        }

        private void UpdateThemeControlHighlightMid(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeControlHighlightMidColor"] = theme.ThemeControlHighlightMidColor.ToColor();
            resources["ThemeControlHighlightMidBrush"] = theme.ThemeControlHighlightMidColor.ToBrush();
        }

        private void UpdateThemeControlHighlightHigh(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeControlHighlightHighColor"] = theme.ThemeControlHighlightHighColor.ToColor();
            resources["ThemeControlHighlightHighBrush"] = theme.ThemeControlHighlightHighColor.ToBrush();
        }

        private void UpdateThemeForeground(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeForegroundColor"] = theme.ThemeForegroundColor.ToColor();
            resources["ThemeForegroundBrush"] = theme.ThemeForegroundColor.ToBrush();
        }

        private void UpdateThemeForegroundLow(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeForegroundLowColor"] = theme.ThemeForegroundLowColor.ToColor();
            resources["ThemeForegroundLowBrush"] = theme.ThemeForegroundLowColor.ToBrush();
        }

        private void UpdateHighlight(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["HighlightColor"] = theme.HighlightColor.ToColor();
            resources["HighlightBrush"] = theme.HighlightColor.ToBrush();
        }

        private void UpdateError(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ErrorColor"] = theme.ErrorColor.ToColor();
            resources["ErrorBrush"] = theme.ErrorColor.ToBrush();
        }

        private void UpdateErrorLow(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ErrorLowColor"] = theme.ErrorLowColor.ToColor();
            resources["ErrorLowBrush"] = theme.ErrorLowColor.ToBrush();
        }

        private void UpdateThemeBorderThickness(IResourceDictionary resources, ThemeViewModel theme)
        {
            resources["ThemeBorderThickness"] = theme.ThemeBorderThickness.ToThickness();
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
        }

        public IDisposable ObserveTheme(IResourceDictionary resources, ThemeViewModel theme, Action<ThemeViewModel> preview = null)
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

            void Observe(ReactiveObject value, Action<IResourceDictionary, ThemeViewModel> update)
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

        public void Attach(IResourceNode node, IResourceDictionary resources, Action<ThemeViewModel> preview = null)
        {
            var defaultThem = GetTheme(node);
            defaultThem.Name = "BaseLight";

            DefaultTheme = defaultThem;

            if (Themes == null)
            {
                CurrentTheme = defaultThem.Clone();
#pragma warning disable CS0618
                Themes = new ReactiveList<ThemeViewModel>();
                Themes.Add(CurrentTheme);
#pragma warning restore CS0618
            }
            else
            {
                CurrentTheme = Themes.FirstOrDefault();
            }

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

        public void Detach()
        {
            _themeObservable?.Dispose();
            _editorObservable?.Dispose();
        }
    }
}
