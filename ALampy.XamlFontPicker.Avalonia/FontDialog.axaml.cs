using System;
using System.ComponentModel;
using System.Linq;
using Avalonia;
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
        private string _searchTextBeforeChanging = string.Empty;
        private int _selectionStartBeforeChanging;
        private int _selectionEndBeforeChanging;

        public FontDialog()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            FontSearchTextBox.PropertyChanged += FontSearchTextBox_PropertyChanged;
            CaptureSearchTextState();
        }

        private void Window_Loaded(object? sender, RoutedEventArgs e)
        {
            ScrollSelectedFontIntoView();
            SyncSearchTextWithCurrentSelection();
            CaptureSearchTextState();
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(FontDialogViewModel.SelectedFontFamily))
                return;

            if (_isSelectingFromSearch)
                return;

            SyncSearchTextWithCurrentSelection();
        }

        private void FontSearchTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (_isUpdatingSearchText)
                return;

            if (e.Property != TextBox.SelectionStartProperty
                && e.Property != TextBox.SelectionEndProperty)
            {
                return;
            }

            UpdateSelectionSnapshotIfTextMatchesCurrentSnapshot();
        }

        private void CaptureSearchTextState()
        {
            _searchTextBeforeChanging = FontSearchTextBox.Text ?? string.Empty;
            _selectionStartBeforeChanging = FontSearchTextBox.SelectionStart;
            _selectionEndBeforeChanging = FontSearchTextBox.SelectionEnd;
        }

        private void UpdateSelectionSnapshotIfTextMatchesCurrentSnapshot()
        {
            var currentText = FontSearchTextBox.Text ?? string.Empty;
            if (!string.Equals(currentText, _searchTextBeforeChanging, StringComparison.InvariantCulture))
                return;

            _selectionStartBeforeChanging = FontSearchTextBox.SelectionStart;
            _selectionEndBeforeChanging = FontSearchTextBox.SelectionEnd;
        }

        private void FontSearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            try
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

                    if (ShouldApplyAutoCompletion(searchText, matchedName))
                    {
                        _isUpdatingSearchText = true;
                        try
                        {
                            FontSearchTextBox.Text += matchedName[searchText.Length..];
                            FontSearchTextBox.SelectionStart = searchText.Length;
                            FontSearchTextBox.SelectionEnd = matchedName.Length;
                        }
                        finally
                        {
                            _isUpdatingSearchText = false;
                        }
                    }
                }
                finally
                {
                    _isSelectingFromSearch = false;
                }

                FontFamilyListBox.ScrollIntoView(matchedFontFamily);
            }
            finally
            {
                CaptureSearchTextState();
            }
        }

        private bool GetMatchedFontFamilyName(string searchText, out FontFamily matchedFontFamily, out string matchedName)
        {
            foreach (var fontFamily in ViewModel.FontFamilies)
            {
                foreach (var name in GetFontFamilySearchNames(fontFamily))
                {
                    if (name.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase) != true)
                        continue;

                    matchedFontFamily = fontFamily;
                    matchedName = name;
                    return true;
                }
            }

            matchedFontFamily = FontFamily.Default;
            matchedName = string.Empty;
            return false;
        }

        private static string[] GetFontFamilySearchNames(FontFamily fontFamily)
        {
            var names = fontFamily.FamilyNames
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToList();

            if (!string.IsNullOrWhiteSpace(fontFamily.Name)
                && names.All(name => !string.Equals(name, fontFamily.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                names.Insert(0, fontFamily.Name);
            }

            return names.ToArray();
        }

        private bool ShouldApplyAutoCompletion(string searchText, string matchedName)
        {
            if (matchedName.Length <= searchText.Length)
                return false;

            var beforeText = _searchTextBeforeChanging;
            var selectionStart = _selectionStartBeforeChanging;
            var selectionEnd = _selectionEndBeforeChanging;

            var appendedAtTail = selectionStart == beforeText.Length
                && selectionEnd == beforeText.Length
                && searchText.StartsWith(beforeText, StringComparison.CurrentCulture)
                && searchText.Length > beforeText.Length;

            if (appendedAtTail)
                return true;

            var replacedSelectedTail = selectionStart < selectionEnd
                && selectionEnd == beforeText.Length
                && searchText.StartsWith(beforeText.Substring(0, selectionStart), StringComparison.CurrentCulture)
                && searchText.Length > selectionStart;

            return replacedSelectedTail;
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
            FontSearchTextBox.PropertyChanged -= FontSearchTextBox_PropertyChanged;
            base.OnClosed(e);
        }
    }
}
