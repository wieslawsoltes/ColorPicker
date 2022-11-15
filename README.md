# ColorPicker

[![Build Status](https://dev.azure.com/wieslawsoltes/GitHub/_apis/build/status/wieslawsoltes.ColorPicker?repoName=wieslawsoltes%2FColorPicker&branchName=release%2F0.10.11-rc.1)](https://dev.azure.com/wieslawsoltes/GitHub/_build/latest?definitionId=106&repoName=wieslawsoltes%2FColorPicker&branchName=release%2F0.10.11-rc.1)
[![CI](https://github.com/wieslawsoltes/ColorPicker/actions/workflows/build.yml/badge.svg)](https://github.com/wieslawsoltes/ThemeEditor/actions/workflows/build.yml)

[![NuGet](https://img.shields.io/nuget/v/ThemeEditor.Controls.ColorPicker.svg)](https://www.nuget.org/packages/ThemeEditor.Controls.ColorPicker)
[![NuGet](https://img.shields.io/nuget/dt/ThemeEditor.Controls.ColorPicker.svg)](https://www.nuget.org/packages/ThemeEditor.Controls.ColorPicker)
[![MyGet](https://img.shields.io/myget/themeeditor-nightly/vpre/ThemeEditor.Controls.ColorPicker.svg?label=myget)](https://www.myget.org/gallery/themeeditor-nightly)

[![Github All Releases](https://img.shields.io/github/downloads/wieslawsoltes/themeeditor/total.svg)](https://github.com/wieslawsoltes/ThemeEditor/releases)
[![GitHub Release](https://img.shields.io/github/release/wieslawsoltes/themeeditor.svg)](https://github.com/wieslawsoltes/ThemeEditor/releases/latest)
[![Github Releases](https://img.shields.io/github/downloads/wieslawsoltes/themeeditor/latest/total.svg)](https://github.com/wieslawsoltes/ThemeEditor/releases)

## Download

[The latest downloads are available in the release section.](https://github.com/wieslawsoltes/ColorPicker/releases/latest)

# About

ColorPicker is an [Avalonia UI Framework](http://avaloniaui.net/) color picker control.

# Usage

`Install-Package ThemeEditor.Controls.ColorPicker`

```xaml
<StyleInclude Source="avares://ThemeEditor.Controls.ColorPicker/ColorPicker.axaml" />
```

```xaml
xmlns:cp="clr-namespace:ThemeEditor.Controls.ColorPicker;assembly=ThemeEditor.Controls.ColorPicker"
```

```xaml
<cp:ColorPicker Color="Red" />
```

## NuGet

Color picker control is delivered as a NuGet package.

You can find the packages here [NuGet](https://www.nuget.org/packages/ThemeEditor.Controls.ColorPicker/) and install the package like this:

`Install-Package ThemeEditor.Controls.ColorPicker`

or by using nightly build feed:
* Add `https://www.myget.org/F/themeeditor-nightly/api/v2` to your package sources
* Alternative nightly build feed `https://pkgs.dev.azure.com/wieslawsoltes/GitHub/_packaging/Nightly/nuget/v3/index.json`
* Update your package using `ThemeEditor.Controls.ColorPicker` feed

and install the package like this:

`Install-Package ThemeEditor.Controls.ColorPicker -Pre`

# Resources

* [GitHub source code repository.](https://github.com/wieslawsoltes/ColorPicker)
* [Wiki.](https://github.com/wieslawsoltes/ColorPicker/wiki)

# License

ColorPicker is licensed under the [MIT license](LICENSE.TXT).
