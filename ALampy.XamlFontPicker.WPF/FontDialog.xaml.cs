using System;
using System.ComponentModel;
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

        private bool _isUpdatingSearchText;

        public FontDialog()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            EnsureSearchTextMatchesCurrentSelection();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FontDialogViewModel.SelectedFontFamily))
                EnsureSearchTextMatchesCurrentSelection();
        }

        private void FontSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingSearchText)
                return;

            var searchText = FontSearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            var current = ViewModel.SelectedFontFamily;
            if (current != null && IsFontFamilyMatch(current, searchText))
                return;

            var match = ViewModel.FontFamilies
                .FirstOrDefault(fontFamily => !ReferenceEquals(fontFamily, current) && IsFontFamilyMatch(fontFamily, searchText));

            if (match == null)
                return;

            ViewModel.SelectedFontFamily = match;
            FontFamilyListbox.ScrollIntoView(match);
        }

        private static bool IsFontFamilyMatch(FontFamily fontFamily, string searchText)
        {
            if (fontFamily.Source?.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                return true;

            return fontFamily.FamilyNames.Values
                .Any(name => name?.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }

        private void EnsureSearchTextMatchesCurrentSelection()
        {
            var selectedFontFamily = ViewModel.SelectedFontFamily;
            if (selectedFontFamily == null)
                return;

            var searchText = FontSearchTextBox.Text;
            if (!string.IsNullOrWhiteSpace(searchText) && IsFontFamilyMatch(selectedFontFamily, searchText))
                return;

            var fullName = GetFontFamilyFullName(selectedFontFamily);
            if (string.Equals(searchText, fullName, StringComparison.CurrentCulture))
                return;

            _isUpdatingSearchText = true;
            try
            {
                FontSearchTextBox.Text = fullName;
            }
            finally
            {
                _isUpdatingSearchText = false;
            }
        }

        private static string GetFontFamilyFullName(FontFamily fontFamily)
        {
            if (fontFamily == null)
                return string.Empty;

            return LanguageSpecificStringConverter.GetValue(fontFamily.FamilyNames)
                ?? fontFamily.Source
                ?? string.Empty;
        }

        protected override void OnClosed(EventArgs e)
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            base.OnClosed(e);
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
