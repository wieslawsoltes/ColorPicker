# Avalonia Theme Editor

[![Build status](https://dev.azure.com/wieslawsoltes/ThemeEditor/_apis/build/status/ThemeEditor)](https://dev.azure.com/wieslawsoltes/ThemeEditor/_build/latest?definitionId=20)

# About

ThemeEditor is an [Avalonia UI Framework](http://avaloniaui.net/) theme editor.

# Usage

* Export theme as Xaml file (e.g. `MyTheme.xaml`).

* Add exported Xaml file (e.g. `MyTheme.xaml`) to your `Avalonia` application project.

* Add `DefaultThemeAddons.xaml` file to your project (copy from `ThemeEditor` project folder).

* Update `Styles` collection in `App.xaml` (replace `YourApp` with your project name).

```XAML
<Application xmlns="https://github.com/avaloniaui">
    <Application.Styles>
        <StyleInclude Source="resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default"/>
        <!--<StyleInclude Source="resm:Avalonia.Themes.Default.Accents.BaseLight.xaml?assembly=Avalonia.Themes.Default"/>-->
        <StyleInclude Source="resm:YourApp.DefaultThemeAddons.xaml?assembly=YourApp"/>
        <StyleInclude Source="resm:YourApp.MyTheme.xaml?assembly=YourApp"/>
    </Application.Styles>
</Application>
```

* Set `UserControl` or `Window` properties.

You have to set style in your application for `UserControl` or `Window` to apply theme depending if want to style entire window or only part of the window e.g. user control.

```XAML
<UserControl xmlns="https://github.com/avaloniaui"
             Background="{DynamicResource ThemeBackgroundBrush}"
             TextBlock.Foreground="{DynamicResource ThemeForegroundBrush}"
             TextBlock.FontSize="{DynamicResource FontSizeNormal}">
</UserControl>
```

```XAML
<Window xmlns="https://github.com/avaloniaui"
        Background="{DynamicResource ThemeBackgroundBrush}"
        TextBlock.Foreground="{DynamicResource ThemeForegroundBrush}"
        TextBlock.FontSize="{DynamicResource FontSizeNormal}">
</Window>
```

# Build

### Clone

```
git clone https://github.com/wieslawsoltes/ThemeEditor.git
cd ThemeEditor
```

### Publish

```
dotnet publish .\src\ThemeEditor\ThemeEditor.csproj -r win7-x64 -c Release -f netcoreapp2.0 --output ThemeEditor-publish-win7-x64
```

### Run

```
.\src\ThemeEditor\ThemeEditor-publish-win7-x64\ThemeEditor.exe
```

# Issues

### AccessText

You have to set style in your application to apply theme.

```XAML
<Style Selector="AccessText">
    <Setter Property="TextBlock.Foreground" Value="{DynamicResource ThemeForegroundBrush}"/>
    <Setter Property="TextBlock.FontSize" Value="{DynamicResource FontSizeNormal}"/>
</Style>
```

### Border

You have to set style in your application to apply theme.

```XAML
<Style Selector="Border">
    <Setter Property="Background" Value="{DynamicResource ThemeBackgroundBrush}"/>
    <Setter Property="BorderBrush" Value="{DynamicResource ThemeBorderMidBrush}"/>
    <Setter Property="BorderThickness" Value="{DynamicResource ThemeBorderThickness}"/>
</Style>
```

### TextBlock

The Foreground brush is hard-coded.

```C#
public static readonly AttachedProperty<IBrush> ForegroundProperty =
    AvaloniaProperty.RegisterAttached<TextBlock, Control, IBrush>(
        nameof(Foreground),
        Brushes.Black,
        inherits: true);
```

You have to set style in your application to apply theme.

```XAML
<Style Selector="TextBlock">
    <Setter Property="TextBlock.Foreground" Value="{DynamicResource ThemeForegroundBrush}"/>
    <Setter Property="TextBlock.FontSize" Value="{DynamicResource FontSizeNormal}"/>
</Style>
```

### TextBox

You have to set style in your application to apply theme.

```XAML
<Style Selector="TextBox">
    <Setter Property="TextBlock.Foreground" Value="{DynamicResource ThemeForegroundBrush}"/>
    <Setter Property="TextBlock.FontSize" Value="{DynamicResource FontSizeNormal}"/>
</Style>
```

### MenuItem

You have to set style in your application to apply theme.

```XAML
<Style Selector="MenuItem">
    <Setter Property="TextBlock.Foreground" Value="{DynamicResource ThemeForegroundBrush}"/>
    <Setter Property="TextBlock.FontSize" Value="{DynamicResource FontSizeNormal}"/>
</Style>
```

### ToolTip

When you only set `ToolTip.Tip` content to text like this `<Border ToolTip.Tip="This is a ToolTip">` the theme font size and brush are not applied to the tooltip.

You have to set style in your application to apply theme.

```XAML
<Style Selector="ToolTip">
    <Setter Property="TextBlock.Foreground" Value="{DynamicResource ThemeForegroundBrush}"/>
    <Setter Property="TextBlock.FontSize" Value="{DynamicResource FontSizeNormal}"/>
</Style>
```

## Fonts

The `FontSizeSmall`, `FontSizeNormal` and `FontSizeLarge` dynamic resource is not used consistently across default theme.

### FontSizeNormal

Some control templates do not handle correctly increased font sizes (e.g. change FontSizeNormal to 28).

* ButtonSpinner
* Calendar
* NumericUpDown

# Resources

* [GitHub source code repository.](https://github.com/wieslawsoltes/ThemeEditor)

# License

ThemeEditor is licensed under the [MIT license](LICENSE.TXT).
