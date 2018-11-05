using System.Runtime.Serialization;
using ReactiveUI;

namespace ThemeEditor.ViewModels
{
    [DataContract]
    public class ThemeViewModel : ReactiveObject
    {
        private string _name;
        private IColor _themeAccentColor;
        private IColor _themeAccentColor2;
        private IColor _themeAccentColor3;
        private IColor _themeAccentColor4;
        private IColor _themeBackgroundColor;
        private IColor _themeBorderLowColor;
        private IColor _themeBorderMidColor;
        private IColor _themeBorderHighColor;
        private IColor _themeControlLowColor;
        private IColor _themeControlMidColor;
        private IColor _themeControlHighColor;
        private IColor _themeControlHighlightLowColor;
        private IColor _themeControlHighlightMidColor;
        private IColor _themeControlHighlightHighColor;
        private IColor _themeForegroundColor;
        private IColor _themeForegroundLowColor;
        private IColor _highlightColor;
        private IColor _errorColor;
        private IColor _errorLowColor;
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
        public IColor ThemeAccentColor
        {
            get { return _themeAccentColor; }
            set { this.RaiseAndSetIfChanged(ref _themeAccentColor, value); }
        }

        [DataMember]
        public IColor ThemeAccentColor2
        {
            get { return _themeAccentColor2; }
            set { this.RaiseAndSetIfChanged(ref _themeAccentColor2, value); }
        }

        [DataMember]
        public IColor ThemeAccentColor3
        {
            get { return _themeAccentColor3; }
            set { this.RaiseAndSetIfChanged(ref _themeAccentColor3, value); }
        }

        [DataMember]
        public IColor ThemeAccentColor4
        {
            get { return _themeAccentColor4; }
            set { this.RaiseAndSetIfChanged(ref _themeAccentColor4, value); }
        }

        [DataMember]
        public IColor ThemeBackgroundColor
        {
            get { return _themeBackgroundColor; }
            set { this.RaiseAndSetIfChanged(ref _themeBackgroundColor, value); }
        }

        [DataMember]
        public IColor ThemeBorderLowColor
        {
            get { return _themeBorderLowColor; }
            set { this.RaiseAndSetIfChanged(ref _themeBorderLowColor, value); }
        }

        [DataMember]
        public IColor ThemeBorderMidColor
        {
            get { return _themeBorderMidColor; }
            set { this.RaiseAndSetIfChanged(ref _themeBorderMidColor, value); }
        }

        [DataMember]
        public IColor ThemeBorderHighColor
        {
            get { return _themeBorderHighColor; }
            set { this.RaiseAndSetIfChanged(ref _themeBorderHighColor, value); }
        }

        [DataMember]
        public IColor ThemeControlLowColor
        {
            get { return _themeControlLowColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlLowColor, value); }
        }

        [DataMember]
        public IColor ThemeControlMidColor
        {
            get { return _themeControlMidColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlMidColor, value); }
        }

        [DataMember]
        public IColor ThemeControlHighColor
        {
            get { return _themeControlHighColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlHighColor, value); }
        }

        [DataMember]
        public IColor ThemeControlHighlightLowColor
        {
            get { return _themeControlHighlightLowColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlHighlightLowColor, value); }
        }

        [DataMember]
        public IColor ThemeControlHighlightMidColor
        {
            get { return _themeControlHighlightMidColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlHighlightMidColor, value); }
        }

        [DataMember]
        public IColor ThemeControlHighlightHighColor
        {
            get { return _themeControlHighlightHighColor; }
            set { this.RaiseAndSetIfChanged(ref _themeControlHighlightHighColor, value); }
        }

        [DataMember]
        public IColor ThemeForegroundColor
        {
            get { return _themeForegroundColor; }
            set { this.RaiseAndSetIfChanged(ref _themeForegroundColor, value); }
        }

        [DataMember]
        public IColor ThemeForegroundLowColor
        {
            get { return _themeForegroundLowColor; }
            set { this.RaiseAndSetIfChanged(ref _themeForegroundLowColor, value); }
        }

        [DataMember]
        public IColor HighlightColor
        {
            get { return _highlightColor; }
            set { this.RaiseAndSetIfChanged(ref _highlightColor, value); }
        }

        [DataMember]
        public IColor ErrorColor
        {
            get { return _errorColor; }
            set { this.RaiseAndSetIfChanged(ref _errorColor, value); }
        }

        [DataMember]
        public IColor ErrorLowColor
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
                Name = Name,
                ThemeAccentColor = ThemeAccentColor.Clone(),
                ThemeAccentColor2 = ThemeAccentColor2.Clone(),
                ThemeAccentColor3 = ThemeAccentColor3.Clone(),
                ThemeAccentColor4 = ThemeAccentColor4.Clone(),
                ThemeBackgroundColor = ThemeBackgroundColor.Clone(),
                ThemeBorderLowColor = ThemeBorderLowColor.Clone(),
                ThemeBorderMidColor = ThemeBorderMidColor.Clone(),
                ThemeBorderHighColor = ThemeBorderHighColor.Clone(),
                ThemeControlLowColor = ThemeControlLowColor.Clone(),
                ThemeControlMidColor = ThemeControlMidColor.Clone(),
                ThemeControlHighColor = ThemeControlHighColor.Clone(),
                ThemeControlHighlightLowColor = ThemeControlHighlightLowColor.Clone(),
                ThemeControlHighlightMidColor = ThemeControlHighlightMidColor.Clone(),
                ThemeControlHighlightHighColor = ThemeControlHighlightHighColor.Clone(),
                ThemeForegroundColor = ThemeForegroundColor.Clone(),
                ThemeForegroundLowColor = ThemeForegroundLowColor.Clone(),
                HighlightColor = HighlightColor.Clone(),
                ErrorColor = ErrorColor.Clone(),
                ErrorLowColor = ErrorLowColor.Clone(),
                ThemeBorderThickness = ThemeBorderThickness.Clone(),
                ThemeDisabledOpacity = ThemeDisabledOpacity,
                FontSizeSmall = FontSizeSmall,
                FontSizeNormal = FontSizeNormal,
                FontSizeLarge = FontSizeLarge
            };
        }
    }
}
