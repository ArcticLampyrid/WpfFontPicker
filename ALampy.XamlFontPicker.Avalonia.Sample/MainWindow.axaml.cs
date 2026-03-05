using System;
using Avalonia.Controls;
using ALampy.XamlFontPicker.Avalonia;

namespace ALampy.XamlFontPicker.AvaloniaSample
{
    public partial class MainWindow : Window
    {
        private FontPicker? _fontPicker;
        private TextBlock? _resultTextBlock;

        public MainWindow()
        {
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
