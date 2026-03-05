using System;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace ALampy.XamlFontPicker.Avalonia
{
    public partial class FontDialog : Window
    {
        public PickedFontInfo? SelectedFontInfo
        {
            get => ViewModel.SelectedFontInfo;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(SelectedFontInfo));

                ViewModel.FontSize = value.Size;

                var actualFontFamily = ViewModel.FontFamilies
                    .FirstOrDefault(item => string.Equals(item.Name, value.Family.Name, StringComparison.CurrentCulture));

                if (actualFontFamily == null)
                    return;

                ViewModel.SelectedFontFamily = actualFontFamily;
                ViewModel.SelectedTypefaceItem = ViewModel.FamilyTypefaces
                    .FirstOrDefault(x => x.Typeface.Stretch == value.Stretch
                                         && x.Typeface.Style == value.Style
                                         && x.Typeface.Weight == value.Weight);
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
        }

        private void Window_Loaded(object? sender, RoutedEventArgs e)
        {
            ScrollSelectedFontIntoView();
            SyncSearchTextWithCurrentSelection();
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(FontDialogViewModel.SelectedFontFamily))
                return;

            if (_isSelectingFromSearch)
                return;

            SyncSearchTextWithCurrentSelection();
        }

        private void FontSearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            if (_isUpdatingSearchText)
                return;

            var searchText = FontSearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            var current = ViewModel.SelectedFontFamily;
            var match = GetMatchedFontFamily(searchText, current, out var matchedName);
            if (match == null)
                return;

            if (ReferenceEquals(current, match))
                return;

            _isSelectingFromSearch = true;
            try
            {
                ViewModel.SelectedFontFamily = match;

                var caretIndex = FontSearchTextBox.CaretIndex;
                if (caretIndex == searchText.Length && matchedName.Length > searchText.Length)
                {
                    _isUpdatingSearchText = true;
                    FontSearchTextBox.Text = matchedName;
                    FontSearchTextBox.SelectionStart = searchText.Length;
                    FontSearchTextBox.SelectionEnd = matchedName.Length;
                    _isUpdatingSearchText = false;
                }
            }
            finally
            {
                _isSelectingFromSearch = false;
            }

            FontFamilyListBox.ScrollIntoView(match);
        }

        private FontFamily? GetMatchedFontFamily(string searchText, FontFamily? currentFontFamily, out string matchedName)
        {
            if (currentFontFamily != null && IsFontFamilyMatch(currentFontFamily, searchText))
            {
                matchedName = currentFontFamily.Name ?? string.Empty;
                return currentFontFamily;
            }

            foreach (var fontFamily in ViewModel.FontFamilies)
            {
                if (ReferenceEquals(fontFamily, currentFontFamily))
                    continue;

                if (!IsFontFamilyMatch(fontFamily, searchText))
                    continue;

                matchedName = fontFamily.Name ?? string.Empty;
                return fontFamily;
            }

            matchedName = string.Empty;
            return null;
        }

        private static bool IsFontFamilyMatch(FontFamily fontFamily, string searchText)
        {
            return fontFamily.Name?.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase) == true;
        }

        private void SyncSearchTextWithCurrentSelection()
        {
            var selectedFontFamily = ViewModel.SelectedFontFamily;
            if (selectedFontFamily == null)
                return;

            var fullName = selectedFontFamily.Name ?? string.Empty;
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

        private void ScrollSelectedFontIntoView()
        {
            var selectedFontFamily = ViewModel.SelectedFontFamily;
            if (selectedFontFamily == null)
                return;

            FontFamilyListBox.ScrollIntoView(selectedFontFamily);
        }

        private void OkButton_Click(object? sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void CancelButton_Click(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }

        protected override void OnClosed(EventArgs e)
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            base.OnClosed(e);
        }
    }
}
