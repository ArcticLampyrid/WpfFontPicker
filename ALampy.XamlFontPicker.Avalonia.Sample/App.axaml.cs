using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaXamlLoader = Avalonia.Markup.Xaml.AvaloniaXamlLoader;

namespace ALampy.XamlFontPicker.AvaloniaSample
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
