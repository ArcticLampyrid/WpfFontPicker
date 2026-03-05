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
                ViewModel.SelectedTypeface = ViewModel.FamilyTypefaces
                    .FirstOrDefault(x => x.Stretch == value.Stretch && x.Style == value.Style && x.Weight == value.Weight);
            }
        }

        public FontDialogViewModel ViewModel { get; } = new FontDialogViewModel();

        private bool _isUpdatingSearchText;
        private bool _isSelectingFromSearch;

        private TextBox FontSearchTextBoxControl => this.FindControl<TextBox>("FontSearchTextBox")!;
        private ListBox FontFamilyListBoxControl => this.FindControl<ListBox>("FontFamilyListBox")!;

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

            var searchText = FontSearchTextBoxControl.Text;
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            var current = ViewModel.SelectedFontFamily;
            if (current != null && IsFontFamilyMatch(current, searchText))
                return;

            var match = ViewModel.FontFamilies
                .FirstOrDefault(fontFamily => !ReferenceEquals(fontFamily, current) && IsFontFamilyMatch(fontFamily, searchText));

            if (match == null)
                return;

            _isSelectingFromSearch = true;
            try
            {
                ViewModel.SelectedFontFamily = match;
            }
            finally
            {
                _isSelectingFromSearch = false;
            }

            FontFamilyListBoxControl.ScrollIntoView(match);
        }

        private static bool IsFontFamilyMatch(FontFamily fontFamily, string searchText)
        {
            return fontFamily.Name?.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void SyncSearchTextWithCurrentSelection()
        {
            var selectedFontFamily = ViewModel.SelectedFontFamily;
            if (selectedFontFamily == null)
                return;

            var fullName = selectedFontFamily.Name ?? string.Empty;
            if (string.Equals(FontSearchTextBoxControl.Text, fullName, StringComparison.CurrentCulture))
                return;

            _isUpdatingSearchText = true;
            try
            {
                FontSearchTextBoxControl.Text = fullName;
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

            FontFamilyListBoxControl.ScrollIntoView(selectedFontFamily);
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
