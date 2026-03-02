using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace ALampy.XamlFontPicker.WPF
{
    public partial class FontDialog : Window
    {
        public PickedFontInfo SelectedFontInfo
        {
            get => ViewModel.SelectedFontInfo;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(SelectedFontInfo));
                ViewModel.FontSize = value.Size;
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
                    ViewModel.SelectedFontFamily = actualFontFamily;
                    ViewModel.SelectedTypeface = actualFontFamily.FamilyTypefaces
                        .FirstOrDefault(x => x.Stretch == value.Stretch && x.Style == value.Style && x.Weight == value.Weight);
                }
            }
        }

        public FontDialogViewModel ViewModel { get; } = new FontDialogViewModel();

        public FontDialog()
        {
            InitializeComponent();
            DataContext = ViewModel;
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
