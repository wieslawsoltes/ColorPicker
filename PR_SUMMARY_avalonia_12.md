# PR Summary: Upgrade ColorPicker to Avalonia 12.0.0

## Overview

This branch upgrades the ColorPicker solution to Avalonia `12.0.0`, updates the project/package version to `12.0.0`, replaces the removed Avalonia diagnostics package with `ProDiagnostics`, and aligns the build/test/tooling stack with the new runtime baseline.

Branch: `upgrade/avalonia-12.0.0`

Commits:

1. `fc7e23f` `Upgrade projects to Avalonia 12.0.0`
2. `fbb8cb4` `Refresh build and test tooling for .NET 10`
3. `97b12af` `Refresh packaged README links`

## What Changed

### 1. Avalonia 12 migration baseline

- Upgraded shared Avalonia package references to `12.0.0`.
- Updated `VersionPrefix` to `12.0.0`.
- Replaced `build/Avalonia.Diagnostics.props` with `build/ProDiagnostics.props`.
- Switched the sample app to import `ProDiagnostics`.
- Updated the solution item entry to reference the new diagnostics props file.

Affected areas:

- `build/Avalonia.props`
- `build/Avalonia.Desktop.props`
- `build/Avalonia.ReactiveUI.props`
- `build/Avalonia.Themes.Fluent.props`
- `build/Avalonia.Themes.Simple.props`
- `build/Base.props`
- `build/ProDiagnostics.props`
- `ThemeEditor.sln`

### 2. Runtime and target framework alignment

- Updated `global.json` to a .NET 10 SDK baseline.
- Kept the library multi-targeted for `net8.0` and `net10.0`.
- Moved the runnable sample and unit tests to `net10.0` so they execute on the current toolchain/runtime.
- Updated GitHub Actions to use the new baseline and removed Azure DevOps CI.

Affected areas:

- `global.json`
- `.github/workflows/build.yml`
- `.github/workflows/release.yml`
- `src/ThemeEditor.Controls.ColorPicker/ThemeEditor.Controls.ColorPicker.csproj`
- `samples/ColorPickerDemo/ColorPickerDemo.csproj`
- `tests/ThemeEditor.Controls.ColorPicker.UnitTests/ThemeEditor.Controls.ColorPicker.UnitTests.csproj`

### 3. Packaging updates

- Added package readme metadata so the NuGet package includes `README.md`.
- Corrected the package project URL to the ColorPicker repository.

Affected areas:

- `src/ThemeEditor.Controls.ColorPicker/ThemeEditor.Controls.ColorPicker.csproj`
- `build/Base.props`

### 4. Vulnerability and tooling cleanup

- Added a direct `Newtonsoft.Json` reference in test tooling to override the vulnerable transitive version.
- Added a direct `Tmds.DBus.Protocol` reference in desktop props to override the vulnerable transitive version.
- Upgraded the Nuke build project to `Nuke.Common 10.1.0`.
- Moved the Nuke build runner to `net10.0`.
- Adjusted build cleanup logic to avoid the removed older filesystem helper API.
- Regenerated `.nuke/build.schema.json` from the upgraded Nuke toolchain.

Affected areas:

- `build/XUnit.props`
- `build/Avalonia.Desktop.props`
- `build/build/_build.csproj`
- `build/build/Build.cs`
- `.nuke/build.schema.json`

### 5. README cleanup

- Fixed stale links that still pointed to the old ThemeEditor repository/workflow.
- Updated release badge targets to the ColorPicker repository.
- Updated the Avalonia website link to HTTPS.
- Removed stale nightly feed references that were tied to the old CI setup.

Affected areas:

- `README.md`

### 6. CI migration to GitHub Actions

- Replaced the Azure DevOps pipeline with GitHub Actions workflows based on the `PanAndZoom` workflow structure.
- Added a matrix build/test workflow with package and sample-app artifacts.
- Added a release workflow for tag/manual releases, NuGet publishing, and GitHub release creation.
- Removed the obsolete Azure DevOps pipeline file and its solution entry.

Affected areas:

- `.github/workflows/build.yml`
- `.github/workflows/release.yml`
- `ThemeEditor.sln`
- `README.md`

## Verification

Executed successfully on this branch:

- `dotnet test ThemeEditor.sln -c Release`
- `dotnet run --project build/build/_build.csproj -- --help`
- `dotnet list build/build/_build.csproj package --vulnerable --include-transitive`
- `dotnet pack src/ThemeEditor.Controls.ColorPicker/ThemeEditor.Controls.ColorPicker.csproj -c Release -o artifacts/NuGet`

Results:

- Unit tests passed: `6/6`
- Nuke runner starts successfully on the .NET 10 baseline
- No vulnerable packages reported for the Nuke build project
- Package produced successfully: `ThemeEditor.Controls.ColorPicker.12.0.0.nupkg`

## Notes For Review

- The library still targets `net8.0` in addition to `net10.0` to preserve consumer compatibility while keeping the repo runnable on the installed .NET 10 toolchain.
- The sample and tests target `net10.0` because the local environment only has .NET 10 runtimes installed.
- `ProDiagnostics` is used instead of `AvaloniaUI.DiagnosticsSupport` per request.
