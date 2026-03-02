using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace ALampy.XamlFontPicker.WPF
{
    public class FontDialogViewModel : INotifyPropertyChanged
    {
        private FontFamily _selectedFontFamily;
        private FamilyTypeface _selectedTypeface;
        private double _fontSize;

        public FontDialogViewModel()
        {
            _selectedFontFamily = new FontFamily();
            _selectedTypeface = null;
            _fontSize = 12;
        }

        public FontFamily SelectedFontFamily
        {
            get => _selectedFontFamily;
            set
            {
                _selectedFontFamily = value;
                OnPropertyChanged(nameof(SelectedFontFamily));
                OnPropertyChanged(nameof(SelectedFontInfo));
            }
        }

        public FamilyTypeface SelectedTypeface
        {
            get => _selectedTypeface;
            set
            {
                _selectedTypeface = value;
                OnPropertyChanged(nameof(SelectedTypeface));
                OnPropertyChanged(nameof(SelectedFontInfo));
            }
        }

        public double FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
                OnPropertyChanged(nameof(SelectedFontInfo));
            }
        }

        public PickedFontInfo SelectedFontInfo
        {
            get
            {
                return new PickedFontInfo(
                    SelectedFontFamily ?? new FontFamily(),
                    SelectedTypeface?.Stretch ?? FontStretches.Normal,
                    SelectedTypeface?.Style ?? FontStyles.Normal,
                    SelectedTypeface?.Weight ?? FontWeights.Normal,
                    FontSize);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
