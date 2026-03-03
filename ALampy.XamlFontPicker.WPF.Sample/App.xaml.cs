using System;
using System.Linq;
using System.Windows;

namespace ALampy.XamlFontPicker.WPF.Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool openDialogOnStartup = e.Args.Contains("--open-dialog-on-startup", StringComparer.OrdinalIgnoreCase);
            var mainWindow = new MainWindow(openDialogOnStartup);
            mainWindow.Show();
        }
    }
}
