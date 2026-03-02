using System.Windows;
using System.Windows.Controls;

namespace ALampy.XamlFontPicker.WPF
{
    public partial class FontPicker : UserControl
    {
        public PickedFontInfo SelectedFontInfo
        {
            get { return (PickedFontInfo)GetValue(SelectedFontInfoProperty); }
            set { SetValue(SelectedFontInfoProperty, value); }
        }

        public static readonly DependencyProperty SelectedFontInfoProperty =
            DependencyProperty.Register(nameof(SelectedFontInfo),
                typeof(PickedFontInfo),
                typeof(FontPicker),
                new PropertyMetadata(new PickedFontInfo(), OnSelectedFontInfoPropertyChanged));

        private static void OnSelectedFontInfoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (FontPicker)d;
            target.FontInfoLabel.DataContext = e.NewValue;
        }

        public FontPicker()
        {
            InitializeComponent();
            SelectedFontInfo = PickedFontInfoAccessor.From(this);
        }

        public void OpenFontDialog()
        {
            var dialog = new FontDialog
            {
                SelectedFontInfo = SelectedFontInfo
            };
            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                SelectedFontInfo = dialog.SelectedFontInfo;
            }
        }

        private void SetFontButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenFontDialog();
        }
    }
}
