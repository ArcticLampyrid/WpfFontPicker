using System.Linq;
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
                var openDialogOnStartup = desktop.Args?.Contains("--open-dialog-on-startup") == true;
                desktop.MainWindow = new MainWindow(openDialogOnStartup);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
