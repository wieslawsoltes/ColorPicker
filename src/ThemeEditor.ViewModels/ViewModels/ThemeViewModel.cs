using System.Runtime.Serialization;
using ReactiveUI;

namespace ThemeEditor.ViewModels
{
    [DataContract]
    public class ThemeViewModel : ReactiveObject
    {
        private string _name;
        private ColorViewModel _themeAccentColor;
        private ColorViewModel _themeAccentColor2;
        private ColorViewModel _themeAccentColor3;
        private ColorViewModel _themeAccentColor4;
        private ColorViewModel _themeBackgroundColor;
        private ColorViewModel _themeBorderLowColor;
        private ColorViewModel _themeBorderMidColor;
        private ColorViewModel _themeBorderHighColor;
        private ColorViewModel _themeControlLowColor;
        private ColorViewModel _themeControlMidColor;
        private ColorViewModel _themeControlHighColor;
        private ColorViewModel _themeControlHighlightLowColor;
        private ColorViewModel _themeControlHighlightMidColor;
        private ColorViewModel _themeControlHighlightHighColor;
        private ColorViewModel _themeForegroundColor;
        private ColorViewModel _themeForegroundLowColor;
        private ColorViewModel _highlightColor;
        private ColorViewModel _errorColor;
        private ColorViewModel _errorLowColor;
        private ThicknessViewModel _themeBorderThickness;
        private double _themeDisabledOpacity;
        private double _fontSizeSmall;
        private double _fontSizeNormal;
        private double _fontSizeLarge;

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        [DataMember]
        public ColorViewModel ThemeAccentColor
        {
            get { return _themeAccentColor; }
            set { this.RaiseAndSetIfChanged(ref _themeAccentColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeAccentColor2
        {
            get { return _themeAccentColor2; }
            set { this.RaiseAndSetIfChanged(ref _themeAccentColor2, value); }
        }

        [DataMember]
        public ColorViewModel ThemeAccentColor3
        {
            get { return _themeAccentColor3; }
            set { this.RaiseAndSetIfChanged(ref _themeAccentColor3, value); }
        }

        [DataMember]
        public ColorViewModel ThemeAccentColor4
        {
            get { return _themeAccentColor4; }
            set { this.RaiseAndSetIfChanged(ref _themeAccentColor4, value); }
        }

        [DataMember]
        public ColorViewModel ThemeBackgroundColor
        {
            get { return _themeBackgroundColor; }
            set { this.RaiseAndSetIfChanged(ref _themeBackgroundColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeBorderLowColor
        {
            get { return _themeBorderLowColor; }
            set { this.RaiseAndSetIfChanged(ref _themeBorderLowColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeBorderMidColor
        {
            get { return _themeBorderMidColor; }
            set { this.RaiseAndSetIfChanged(ref _themeBorderMidColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeBorderHighColor
        {
            get { return _themeBorderHighColor; }
            set { this.RaiseAndSetIfChanged(ref _themeBorderHighColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeControlLowColor
        {
            get { return _themeControlLowColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlLowColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeControlMidColor
        {
            get { return _themeControlMidColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlMidColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeControlHighColor
        {
            get { return _themeControlHighColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlHighColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeControlHighlightLowColor
        {
            get { return _themeControlHighlightLowColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlHighlightLowColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeControlHighlightMidColor
        {
            get { return _themeControlHighlightMidColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlHighlightMidColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeControlHighlightHighColor
        {
            get { return _themeControlHighlightHighColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlHighlightHighColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeForegroundColor
        {
            get { return _themeForegroundColor; }
            set { this.RaiseAndSetIfChanged(ref _themeForegroundColor, value); }
        }

        [DataMember]
        public ColorViewModel ThemeForegroundLowColor
        {
            get { return _themeForegroundLowColor; }
            set { this.RaiseAndSetIfChanged(ref _themeForegroundLowColor, value); }
        }

        [DataMember]
        public ColorViewModel HighlightColor
        {
            get { return _highlightColor; }
            set { this.RaiseAndSetIfChanged(ref _highlightColor, value); }
        }

        [DataMember]
        public ColorViewModel ErrorColor
        {
            get { return _errorColor; }
            set { this.RaiseAndSetIfChanged(ref _errorColor, value); }
        }

        [DataMember]
        public ColorViewModel ErrorLowColor
        {
            get { return _errorLowColor; }
            set { this.RaiseAndSetIfChanged(ref _errorLowColor, value); }
        }

        [DataMember]
        public ThicknessViewModel ThemeBorderThickness
        {
            get { return _themeBorderThickness; }
            set { this.RaiseAndSetIfChanged(ref _themeBorderThickness, value); }
        }

        [DataMember]
        public double ThemeDisabledOpacity
        {
            get { return _themeDisabledOpacity; }
            set { this.RaiseAndSetIfChanged(ref _themeDisabledOpacity, value); }
        }

        [DataMember]
        public double FontSizeSmall
        {
            get { return _fontSizeSmall; }
            set { this.RaiseAndSetIfChanged(ref _fontSizeSmall, value); }
        }

        [DataMember]
        public double FontSizeNormal
        {
            get { return _fontSizeNormal; }
            set { this.RaiseAndSetIfChanged(ref _fontSizeNormal, value); }
        }

        [DataMember]
        public double FontSizeLarge
        {
            get { return _fontSizeLarge; }
            set { this.RaiseAndSetIfChanged(ref _fontSizeLarge, value); }
        }

        public ThemeViewModel Clone()
        {
            return new ThemeViewModel()
            {
                Name = this.Name,
                ThemeAccentColor = this.ThemeAccentColor.Clone(),
                ThemeAccentColor2 = this.ThemeAccentColor2.Clone(),
                ThemeAccentColor3 = this.ThemeAccentColor3.Clone(),
                ThemeAccentColor4 = this.ThemeAccentColor4.Clone(),
                ThemeBackgroundColor = this.ThemeBackgroundColor.Clone(),
                ThemeBorderLowColor = this.ThemeBorderLowColor.Clone(),
                ThemeBorderMidColor = this.ThemeBorderMidColor.Clone(),
                ThemeBorderHighColor = this.ThemeBorderHighColor.Clone(),
                ThemeControlLowColor = this.ThemeControlLowColor.Clone(),
                ThemeControlMidColor = this.ThemeControlMidColor.Clone(),
                ThemeControlHighColor = this.ThemeControlHighColor.Clone(),
                ThemeControlHighlightLowColor = this.ThemeControlHighlightLowColor.Clone(),
                ThemeControlHighlightMidColor = this.ThemeControlHighlightMidColor.Clone(),
                ThemeControlHighlightHighColor = this.ThemeControlHighlightHighColor.Clone(),
                ThemeForegroundColor = this.ThemeForegroundColor.Clone(),
                ThemeForegroundLowColor = this.ThemeForegroundLowColor.Clone(),
                HighlightColor = this.HighlightColor.Clone(),
                ErrorColor = this.ErrorColor.Clone(),
                ErrorLowColor = this.ErrorLowColor.Clone(),
                ThemeBorderThickness = this.ThemeBorderThickness.Clone(),
                ThemeDisabledOpacity = this.ThemeDisabledOpacity,
                FontSizeSmall = this.FontSizeSmall,
                FontSizeNormal = this.FontSizeNormal,
                FontSizeLarge = this.FontSizeLarge
            };
        }
    }
}
