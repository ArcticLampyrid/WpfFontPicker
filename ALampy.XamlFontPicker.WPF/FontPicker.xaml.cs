using System.Windows;
using System.Windows.Controls;

namespace ALampy.XamlFontPicker.WPF
{
    /// <summary>
    /// FontPicker.xaml 的交互逻辑
    /// </summary>
    public partial class FontPicker : UserControl
    {
        public PackedFontInfo SelectedFontInfo
        {
            get { return (PackedFontInfo)GetValue(SelectedFontInfoProperty); }
            set { SetValue(SelectedFontInfoProperty, value); }
        }

        public static readonly DependencyProperty SelectedFontInfoProperty =
            DependencyProperty.Register(nameof(SelectedFontInfo),
                typeof(PackedFontInfo),
                typeof(FontPicker),
                new PropertyMetadata(new PackedFontInfo(), OnSelectedFontInfoPropertyChanged));

        private static void OnSelectedFontInfoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (FontPicker)d;
            target.FontInfoLabel.DataContext = e.NewValue;
        }

        public FontPicker()
        {
            InitializeComponent();
            SelectedFontInfo = PackedFontInfo.From(this);
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
