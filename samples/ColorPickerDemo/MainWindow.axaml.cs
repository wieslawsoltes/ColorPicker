using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ColorPickerDemo;

public partial class MainWindow : Window
{
    public static readonly StyledProperty<Color?> ColorProperty =
        AvaloniaProperty.Register<MainWindow, Color?>(nameof(Color));
    
    public static readonly StyledProperty<Color?> NullColorProperty =
        AvaloniaProperty.Register<MainWindow, Color?>(nameof(NullColor));

    public static readonly StyledProperty<string?> HexProperty =
        AvaloniaProperty.Register<MainWindow, string?>(nameof(Hex));

    public static readonly StyledProperty<string?> NullHexProperty =
        AvaloniaProperty.Register<MainWindow, string?>(nameof(NullHex));

    public static readonly StyledProperty<uint> ValueProperty =
        AvaloniaProperty.Register<MainWindow, uint>(nameof(Value));

    public static readonly StyledProperty<uint?> NullValueProperty =
        AvaloniaProperty.Register<MainWindow, uint?>(nameof(NullValue));

    public Color? Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public Color? NullColor
    {
        get => GetValue(NullColorProperty);
        set => SetValue(NullColorProperty, value);
    }

    public string? Hex
    {
        get => GetValue(HexProperty);
        set => SetValue(HexProperty, value);
    }

    public string? NullHex
    {
        get => GetValue(NullHexProperty);
        set => SetValue(NullHexProperty, value);
    }

    public uint Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public uint? NullValue
    {
        get => GetValue(NullValueProperty);
        set => SetValue(NullValueProperty, value);
    }

    public MainWindow()
    {
        InitializeComponent();
            
        Color = Avalonia.Media.Color.FromArgb(0x5F, 0x1F, 0x00, 0x1F);
        NullColor = null;
        Hex = "#5F1F001F";
        NullHex = null;
        Value = Colors.Green.ToUint32();
        NullValue = null;
        DataContext = this;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
