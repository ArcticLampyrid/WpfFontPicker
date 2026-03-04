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
        private bool _isSelectingFromSearch;

        public FontDialog()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            SyncSearchTextWithCurrentSelection();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(FontDialogViewModel.SelectedFontFamily))
                return;

            if (_isSelectingFromSearch)
                return;

            SyncSearchTextWithCurrentSelection();
        }

        private void FontSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingSearchText)
                return;

            var searchText = FontSearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            var matched = GetMatchedFontFamilyName(searchText, out var matchedFontFamily, out var matchedName);
            if (!matched)
                return;

            _isSelectingFromSearch = true;
            try
            {
                ViewModel.SelectedFontFamily = matchedFontFamily;
                // When you add text and subsequently move the cursor to the end

                if (e.Changes.All(x => x.AddedLength > 0) && FontSearchTextBox.SelectionStart == searchText.Length)
                {
                    _isUpdatingSearchText = true;
                    FontSearchTextBox.SelectedText = matchedName.Substring(searchText.Length);
                    _isUpdatingSearchText = false;
                }
            }
            finally
            {
                _isSelectingFromSearch = false;
            }

            FontFamilyListbox.ScrollIntoView(matchedFontFamily);
        }


        private bool GetMatchedFontFamilyName(string searchText, out FontFamily matchedFontFamily, out string matchedName)
        {
            foreach (var fontFamily in ViewModel.FontFamilies)
            {
                foreach (var name in fontFamily.FamilyNames.Values)
                {
                    if (name.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase) == true)
                    {
                        matchedFontFamily = fontFamily;
                        matchedName = name;
                        return true;
                    }
                }
            }
            matchedFontFamily = null;
            matchedName = null;
            return false;
        }

        private void SyncSearchTextWithCurrentSelection()
        {
            var selectedFontFamily = ViewModel.SelectedFontFamily;
            if (selectedFontFamily == null)
                return;

            var fullName = GetFontFamilyFullName(selectedFontFamily);
            if (string.Equals(FontSearchTextBox.Text, fullName, StringComparison.CurrentCulture))
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollSelectedFontIntoView();
        }

        private void ScrollSelectedFontIntoView()
        {
            var selectedFontFamily = ViewModel.SelectedFontFamily;
            if (selectedFontFamily == null)
                return;

            FontFamilyListbox.UpdateLayout();
            FontFamilyListbox.ScrollIntoView(selectedFontFamily);
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
