# Release Checklist (Universal · WPF + Avalonia)

Use this checklist for every release of `XamlFontPicker`.

Projects in scope:
- `ALampy.XamlFontPicker.WPF`
- `ALampy.XamlFontPicker.WPF.Sample`
- `ALampy.XamlFontPicker.Avalonia`
- `ALampy.XamlFontPicker.Avalonia.Sample`

---

## 1) Pre-flight

- [ ] Working tree is clean (`git status`)
- [ ] Branch is correct (`v2.x` or release branch)
- [ ] `origin` is `https://github.com/ArcticLampyrid/XamlFontPicker`
- [ ] Release version decided (SemVer)
- [ ] Changelog/release note draft prepared

---

## 2) Build validation

- [ ] `dotnet build XamlFontPicker.sln -c Release /p:EnableWindowsTargeting=true`
- [ ] No build errors
- [ ] Warnings reviewed (legacy TFM warnings acknowledged if expected)

Optional per-project checks:
- [ ] `dotnet build ALampy.XamlFontPicker.WPF/ALampy.XamlFontPicker.WPF.csproj -c Release`
- [ ] `dotnet build ALampy.XamlFontPicker.Avalonia/ALampy.XamlFontPicker.Avalonia.csproj -c Release`

---

## 3) Runtime smoke tests

### WPF
- [ ] WPF sample launches
- [ ] Font dialog open/select/OK/Cancel works
- [ ] `FontPicker` value updates correctly
- [ ] Screenshot utility passes: `./Utils/WpfSampleScreenshot/run.sh`

### Avalonia
- [ ] Avalonia sample launches
- [ ] Font dialog open/select/OK/Cancel works
- [ ] Search behavior works (match/no-match/autocomplete constraints)
- [ ] Typeface list is based on actual supported family typefaces
- [ ] Screenshot utility passes: `./Utils/AvaloniaSampleScreenshot/run.sh`

---

## 4) API & behavior compatibility

- [ ] Public API remains stable (`PickedFontInfo`, `FontPicker`, `FontDialog`)
- [ ] `SelectedFontInfo` round-trip behaves as expected
- [ ] WPF and Avalonia behaviors are aligned where intended
- [ ] Default font fallback behavior remains valid

---

## 5) Localization consistency

- [ ] WPF resources valid:
  - `ALampy.XamlFontPicker.WPF/Resources/Strings.resx`
  - `ALampy.XamlFontPicker.WPF/Resources/Strings.zh-CN.resx`
- [ ] Avalonia resources valid:
  - `ALampy.XamlFontPicker.Avalonia/Resources/Strings.resx`
  - `ALampy.XamlFontPicker.Avalonia/Resources/Strings.zh-CN.resx`
- [ ] Keys are aligned between WPF and Avalonia
- [ ] UI strings display correctly for target cultures

---

## 6) Packaging metadata check

### WPF package
- [ ] `PackageId`: `ALampy.XamlFontPicker.WPF`
- [ ] `RepositoryUrl` points to `XamlFontPicker`
- [ ] `PackageDescription/Tags/License` reviewed

### Avalonia package
- [ ] `PackageId`: `ALampy.XamlFontPicker.Avalonia`
- [ ] `RepositoryUrl` points to `XamlFontPicker`
- [ ] `PackageDescription/Tags/License` reviewed

---

## 7) Pack artifacts

- [ ] `dotnet pack ALampy.XamlFontPicker.WPF/ALampy.XamlFontPicker.WPF.csproj -c Release`
- [ ] `dotnet pack ALampy.XamlFontPicker.Avalonia/ALampy.XamlFontPicker.Avalonia.csproj -c Release`
- [ ] Inspect generated `.nupkg` and `.snupkg`

---

## 8) Documentation sync

- [ ] `README.md` reflects latest release status for both WPF and Avalonia
- [ ] Namespace examples are correct
- [ ] NuGet links/badges are correct
- [ ] Release notes include notable changes for both stacks

---

## 9) Publish & finalize

- [ ] Commit release-related changes
- [ ] Tag release (`git tag vX.Y.Z`)
- [ ] Push branch and tags
- [ ] Publish NuGet packages
- [ ] Create GitHub Release

---

## Useful commands

```bash
cd /home/alampy/sources/XamlFontPicker

# Build
DOTNET_CLI_TELEMETRY_OPTOUT=1 dotnet build XamlFontPicker.sln -c Release /p:EnableWindowsTargeting=true

# Pack
dotnet pack ALampy.XamlFontPicker.WPF/ALampy.XamlFontPicker.WPF.csproj -c Release
dotnet pack ALampy.XamlFontPicker.Avalonia/ALampy.XamlFontPicker.Avalonia.csproj -c Release
```
