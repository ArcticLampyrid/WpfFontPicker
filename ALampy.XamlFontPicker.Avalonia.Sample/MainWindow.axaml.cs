using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ALampy.XamlFontPicker.AvaloniaSample
{
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
                Opened += OpenedFirstTime;
        }

        private void OpenedFirstTime(object? sender, EventArgs e)
        {
            Opened -= OpenedFirstTime;
            _ = FontPicker1.OpenFontDialog(this);
        }

        private void OpenFontDialogButton_Click(object? sender, RoutedEventArgs e)
        {
            _ = FontPicker1.OpenFontDialog(this);
        }
    }
}
