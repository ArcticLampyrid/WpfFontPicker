using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;

namespace ALampy.XamlFontPicker.Avalonia
{
    public partial class FontPicker : UserControl
    {
        public static readonly StyledProperty<PickedFontInfo?> SelectedFontInfoProperty =
            AvaloniaProperty.Register<FontPicker, PickedFontInfo?>(nameof(SelectedFontInfo), new PickedFontInfo());

        public PickedFontInfo? SelectedFontInfo
        {
            get => GetValue(SelectedFontInfoProperty);
            set => SetValue(SelectedFontInfoProperty, value);
        }

        public FontPicker()
        {
            InitializeComponent();
        }

        private async void SetFontButton_Click(object? sender, RoutedEventArgs e)
        {
            await OpenFontDialog();
        }

        public async Task OpenFontDialog(Window? owner = null)
        {
            Window? parentWindow = owner ?? VisualRoot as Window;

            if (parentWindow == null && Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                parentWindow = desktop.MainWindow;
            }

            if (parentWindow == null)
                return;

            var dialog = new FontDialog();
            if (SelectedFontInfo != null)
                dialog.SelectedFontInfo = SelectedFontInfo;

            var result = await dialog.ShowDialog<bool>(parentWindow);
            if (result)
                SelectedFontInfo = dialog.SelectedFontInfo;
        }
    }
}
