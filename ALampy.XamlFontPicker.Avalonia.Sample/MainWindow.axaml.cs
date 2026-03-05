using Avalonia.Controls;
using Avalonia.Interactivity;
using ALampy.XamlFontPicker.Avalonia;

namespace ALampy.XamlFontPicker.AvaloniaSample
{
    public partial class MainWindow : Window
    {
        private Button? _openFontDialogButton;
        private TextBlock? _resultTextBlock;

        public MainWindow()
        {
            InitializeComponent();

            Opened += MainWindow_Opened;
        }

        private void MainWindow_Opened(object? sender, EventArgs e)
        {
            _openFontDialogButton = this.FindControl<Button>("OpenFontDialogButton");
            _resultTextBlock = this.FindControl<TextBlock>("ResultTextBlock");

            if (_openFontDialogButton != null)
                _openFontDialogButton.Click += OpenFontDialogButton_Click;
        }

        private async void OpenFontDialogButton_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new FontDialog();
            var result = await dialog.ShowDialog<bool>(this);
            if (result && _resultTextBlock != null)
            {
                var fontInfo = dialog.SelectedFontInfo;
                _resultTextBlock.Text = fontInfo?.ToString() ?? "No font selected";
            }
        }
    }
}
