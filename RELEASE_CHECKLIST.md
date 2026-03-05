# Release Checklist

## Pre-Release Checklist

### Code Quality
- [ ] All Avalonia projects build without errors (`dotnet build -c Release`)
- [ ] No new warnings introduced
- [ ] Code follows existing style conventions

### Testing
- [ ] Sample app runs and displays FontPicker correctly
- [ ] FontDialog opens and allows font selection
- [ ] Selected font info is returned correctly
- [ ] OK/Cancel buttons work as expected

### API Surface
- [ ] `PickedFontInfo` model is stable
- [ ] `FontDialogViewModel` properties are correct
- [ ] `FontDialog` opens and returns results
- [ ] `FontPicker` control with `SelectedFontInfo` property works

### Localization
- [ ] English strings in `Resources/Strings.axaml`
- [ ] Chinese strings in `Resources/Strings.zh-CN.axaml`
- [ ] Resource keys match WPF version

### Packaging (NuGet)
- [ ] Package version is updated in `.csproj`
- [ ] Package description is accurate
- [ ] Dependencies are correct (Avalonia 11.x)
- [ ] Target framework is appropriate (net8.0 recommended)

### Documentation
- [ ] README.md updated with Avalonia usage
- [ ] API usage examples are correct

### Repository
- [ ] Changes are committed locally
- [ ] No unintended changes to WPF project
- [ ] Solution builds (Avalonia projects only on non-Windows)

## Release Commands

```bash
# Build for release
dotnet build -c Release

# Pack NuGet package
dotnet pack -c Release
```

## Post-Release
- [ ] Tag version in git
- [ ] Push to NuGet.org
- [ ] Update release notes
