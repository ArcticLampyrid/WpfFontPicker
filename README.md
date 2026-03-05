# ALampy.XamlFontPicker
> (formerly WpfFontPicker) A Xaml control for picking fonts, with a built-in font dialog, supporting WPF and Avalonia.

- WPF: [![NuGet](https://img.shields.io/nuget/v/ALampy.XamlFontPicker.WPF.svg)](https://www.nuget.org/packages/ALampy.XamlFontPicker.WPF)   
  Namespace: `ALampy.XamlFontPicker.WPF`(CLR) or `xmlns:fp="https://github.com/ArcticLampyrid/WpfFontPicker"` (XAML)

- Avalonia 11: Coming soon
  Namespace: `ALampy.XamlFontPicker.Avalonia` (CLR) or `xmlns:fp="using:ALampy.XamlFontPicker.Avalonia"` (XAML)

## Install
- Package Manager `Install-Package ALampy.XamlFontPicker.WPF`  
- .NET CLI `dotnet add package ALampy.XamlFontPicker.WPF`  

## Usage (WPF)
- See `ALampy.XamlFontPicker.WPF.Sample`

## Usage (Avalonia 11)

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:fp="using:ALampy.XamlFontPicker.Avalonia">
  <StackPanel>
    <fp:FontPicker x:Name="FontPicker1" Width="400" />
  </StackPanel>
</Window>
```

```csharp
// Subscribe to selection changes
_fontPicker.GetObservable(FontPicker.SelectedFontInfoProperty)
    .Subscribe(fontInfo => Console.WriteLine(fontInfo));

// Or open dialog directly
var dialog = new FontDialog();
var result = await dialog.ShowDialog<bool>(this);
if (result)
{
    var fontInfo = dialog.SelectedFontInfo;
}
```

## Preview
![Screenshot 1](https://github.com/ArcticLampyrid/WpfFontPicker/blob/master/Screenshot/1.png)   

```xaml
<StackPanel Orientation="Vertical">
    <Button x:Name="OpenFontDialogButton" Click="OpenFontDialogButton_Click">OpenFontDialog</Button>
    <fp:FontPicker x:Name="FontPicker1"/>
</StackPanel>
```

```c#
private void OpenFontDialogButton_Click(object sender, RoutedEventArgs e)
{
    FontPicker1.OpenFontDialog();
}
```

## Development
### Utilities
#### WpfSampleScreenshot
A utility for taking screenshots of WPF samples, which can be used in quick preview. This utility is run in Docker container with Wine, so it can be used on non-Windows platforms as well. But some features may work sightly differently from native Windows environment.

```bash
./Utils/WpfSampleScreenshot/run.sh
```

#### AvaloniaSampleScreenshot
A utility for taking screenshots of Avalonia samples in Docker on Arch Linux. It uses headless X11 (`Xvfb`) + a lightweight WM (`herbstluftwm`) and multi-stage image build, aligned with the WPF screenshot pipeline but without Wine.

```bash
./Utils/AvaloniaSampleScreenshot/run.sh
```
