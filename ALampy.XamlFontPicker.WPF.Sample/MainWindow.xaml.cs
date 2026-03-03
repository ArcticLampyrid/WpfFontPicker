using System;
using System.Windows;

namespace ALampy.XamlFontPicker.WPF.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly bool _openDialogOnStartup;

        public MainWindow() : this(false)
        {
        }

        public MainWindow(bool openDialogOnStartup)
        {
            _openDialogOnStartup = openDialogOnStartup;
            InitializeComponent();

            if (_openDialogOnStartup)
            {
                this.ContentRendered += ContentRenderedFirstTime;
            }
        }

        private void ContentRenderedFirstTime(object sender, EventArgs e)
        {
            this.ContentRendered -= ContentRenderedFirstTime;
            FontPicker1.OpenFontDialog();
        }

        private void OpenFontDialogButton_Click(object sender, RoutedEventArgs e)
        {
            FontPicker1.OpenFontDialog();
        }
    }
}
