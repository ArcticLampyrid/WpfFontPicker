using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace ALampy.XamlFontPicker.WPF
{
    /// <summary>
    /// FontDialog.xaml 的交互逻辑
    /// </summary>
    public partial class FontDialog : Window
    {
        public PackedFontInfo SelectedFontInfo
        {
            get
            {
                return PackedFontInfo.From(PreviewTextBlock);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(SelectedFontInfo));
                FontFamily actualFontFamily = null;
                foreach (var item in FontFamilyListbox.Items)
                {
                    if (((FontFamily)item).FamilyNames.Values.FirstOrDefault() == value.Family.FamilyNames.Values.FirstOrDefault())
                    {
                        actualFontFamily = (FontFamily)item;
                        break;
                    }
                }
                if (actualFontFamily != null)
                {
                    PreviewTextBlock.FontFamily = actualFontFamily;
                }
                PreviewTextBlock.FontSize = value.Size;
                FamilyTypefaceListBox.SelectedItem = actualFontFamily?.FamilyTypefaces
                    .First(x => x.Stretch == value.Stretch && x.Style == value.Style && x.Weight == value.Weight);
            }
        }
        public FontDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = true;
            }
            catch (InvalidOperationException)
            {
            }
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
