using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Media;

namespace ALampy.XamlFontPicker.WPF
{
    public class FontDialogViewModel : INotifyPropertyChanged
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("ALampy.XamlFontPicker.WPF.Resources.Strings", typeof(FontDialogViewModel).Assembly);

        private const double PixelsPerPoint = 96.0 / 72.0;

        private FontFamily _selectedFontFamily;
        private FamilyTypeface _selectedTypeface;
        private double _fontSize;

        public FontDialogViewModel()
        {
            _selectedFontFamily = new FontFamily();
            _selectedTypeface = null;
            _fontSize = 12;

            FontSizeOptions = new List<FontSizeOption>
            {
                new FontSizeOption(72, PixelsPerPoint),
                new FontSizeOption(48, PixelsPerPoint),
                new FontSizeOption(36, PixelsPerPoint),
                new FontSizeOption(28, PixelsPerPoint),
                new FontSizeOption(26, PixelsPerPoint),
                new FontSizeOption(24, PixelsPerPoint),
                new FontSizeOption(22, PixelsPerPoint),
                new FontSizeOption(20, PixelsPerPoint),
                new FontSizeOption(18, PixelsPerPoint),
                new FontSizeOption(16, PixelsPerPoint),
                new FontSizeOption(14, PixelsPerPoint),
                new FontSizeOption(12, PixelsPerPoint),
                new FontSizeOption(11, PixelsPerPoint),
                new FontSizeOption(10, PixelsPerPoint),
                new FontSizeOption(9, PixelsPerPoint),
                new FontSizeOption(8, PixelsPerPoint)
            };
        }

        public IList<FontSizeOption> FontSizeOptions { get; }

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

        // Internal logical pixel size (WPF FontSize unit).
        public double FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
                OnPropertyChanged(nameof(FontSizePt));
                OnPropertyChanged(nameof(SelectedFontInfo));
            }
        }

        // UI-facing point size.
        public double FontSizePt
        {
            get => FontSize / PixelsPerPoint;
            set => FontSize = value * PixelsPerPoint;
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

        public string DialogTitle => GetString(nameof(DialogTitle), "FontDialog_Title");
        public string LabelFamily => GetString(nameof(LabelFamily), "FontDialog_Label_Family");
        public string LabelTypeface => GetString(nameof(LabelTypeface), "FontDialog_Label_Typeface");
        public string LabelSize => GetString(nameof(LabelSize), "FontDialog_Label_Size");
        public string OkText => GetString(nameof(OkText), "Common_Ok");
        public string CancelText => GetString(nameof(CancelText), "Common_Cancel");

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static string GetString(string fallback, string key)
        {
            return ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? fallback;
        }
    }

    public class FontSizeOption
    {
        public FontSizeOption(double pointSize, double pixelsPerPoint)
        {
            PointSize = pointSize;
            PixelSize = pointSize * pixelsPerPoint;
        }

        public double PointSize { get; }

        public double PixelSize { get; }

        public override string ToString()
        {
            return PointSize.ToString("0.##", CultureInfo.CurrentCulture);
        }
    }
}
