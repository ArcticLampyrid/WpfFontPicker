using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
                foreach (var item in ViewModel.FontFamilies)
                {
                    if (item.FamilyNames.Values.FirstOrDefault() == value.Family.FamilyNames.Values.FirstOrDefault())
                    {
                        actualFontFamily = item;
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

        private void FontSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = FontSearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            var match = FontFamilyListbox.Items
                .Cast<FontFamily>()
                .FirstOrDefault(fontFamily => IsFontFamilyMatch(fontFamily, searchText));

            if (match == null)
                return;

            FontFamilyListbox.SelectedItem = match;
            FontFamilyListbox.ScrollIntoView(match);
        }

        private static bool IsFontFamilyMatch(FontFamily fontFamily, string searchText)
        {
            if (fontFamily.Source?.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                return true;

            return fontFamily.FamilyNames.Values
                .Any(name => name?.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0);
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
