# WpfFontPicker
[![NuGet](https://img.shields.io/nuget/v/ALampy.XamlFontPicker.WPF.svg)](https://www.nuget.org/packages/ALampy.XamlFontPicker.WPF)   
Namespace: `ALampy.XamlFontPicker.WPF`(CLR)  or `xmlns:fp="https://github.com/ArcticLampyrid/WpfFontPicker"`(XAML)

# Install
Package Manager `Install-Package ALampy.XamlFontPicker.WPF`  
.NET CLI `dotnet add package ALampy.XamlFontPicker.WPF`  

# Usage
See `ALampy.XamlFontPicker.WPF.Sample`

# Preview
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