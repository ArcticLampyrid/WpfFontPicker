using System;
using Avalonia.Controls;
using ALampy.XamlFontPicker.Avalonia;

namespace ALampy.XamlFontPicker.AvaloniaSample
{
    public partial class MainWindow : Window
    {
        private readonly bool _openDialogOnStartup;
        private FontPicker? _fontPicker;
        private TextBlock? _resultTextBlock;

        public MainWindow() : this(false)
        {
        }

        public MainWindow(bool openDialogOnStartup)
        {
            _openDialogOnStartup = openDialogOnStartup;
            InitializeComponent();

            Opened += MainWindow_Opened;
        }

        private void MainWindow_Opened(object? sender, EventArgs e)
        {
            _fontPicker = this.FindControl<FontPicker>("FontPicker1");
            _resultTextBlock = this.FindControl<TextBlock>("ResultTextBlock");

            if (_fontPicker != null)
            {
                UpdateResultText();
                _fontPicker.PropertyChanged += (s, args) =>
                {
                    if (args.Property.Name == nameof(FontPicker.SelectedFontInfo))
                        UpdateResultText();
                };

                if (_openDialogOnStartup)
                    _ = _fontPicker.OpenFontDialog(this);
            }
        }

        private void UpdateResultText()
        {
            if (_resultTextBlock == null || _fontPicker == null)
                return;

            _resultTextBlock.Text = _fontPicker.SelectedFontInfo?.ToString() ?? "No font selected";
        }
    }
}
