using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ColorPickerDemo;

public partial class MainWindow : Window
{
    public static readonly StyledProperty<Color?> ColorProperty =
        AvaloniaProperty.Register<MainWindow, Color?>(nameof(Color));

    public Color? Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }
        
    public MainWindow()
    {
        InitializeComponent();
            
        Color = Avalonia.Media.Color.FromArgb(0x5F, 0x1F, 0x00, 0x1F);
        // Color = null;

        DataContext = this;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
