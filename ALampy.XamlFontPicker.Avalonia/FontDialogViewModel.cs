using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Avalonia.Media;

namespace ALampy.XamlFontPicker.Avalonia
{
    public class FontDialogViewModel : INotifyPropertyChanged
    {
        private const double PixelsPerPoint = 96.0 / 72.0;

        private FontFamily _selectedFontFamily;
        private Typeface? _selectedTypeface;
        private double _fontSize;

        public FontDialogViewModel()
        {
            _selectedFontFamily = FontFamily.Default;
            _selectedTypeface = null;
            _fontSize = 12;

            FontFamilies = FontManager.Current.SystemFonts
                .OrderBy(f => f.Name)
                .ToList();

            FontSizeOptions = FontSizeOption.GetDefaultSizeOptions();
        }

        public IList<FontFamily> FontFamilies { get; }

        public IList<FontSizeOption> FontSizeOptions { get; }

        public FontFamily SelectedFontFamily
        {
            get => _selectedFontFamily;
            set
            {
                _selectedFontFamily = value;
                OnPropertyChanged(nameof(SelectedFontFamily));
                OnPropertyChanged(nameof(SelectedFontInfo));
                OnPropertyChanged(nameof(FamilyTypefaces));
            }
        }

        public IList<Typeface> FamilyTypefaces
        {
            get
            {
                var family = SelectedFontFamily;
                if (family == null)
                    return Array.Empty<Typeface>();

                var typefaces = new List<Typeface>();
                foreach (var weight in new[] { FontWeight.Thin, FontWeight.ExtraLight, FontWeight.Light, FontWeight.Normal, FontWeight.Medium, FontWeight.SemiBold, FontWeight.Bold, FontWeight.ExtraBold, FontWeight.Black })
                {
                    foreach (var style in new[] { FontStyle.Normal, FontStyle.Italic })
                    {
                        foreach (var stretch in new[] { FontStretch.UltraCondensed, FontStretch.ExtraCondensed, FontStretch.Condensed, FontStretch.SemiCondensed, FontStretch.Normal, FontStretch.SemiExpanded, FontStretch.Expanded, FontStretch.ExtraExpanded, FontStretch.UltraExpanded })
                        {
                            typefaces.Add(new Typeface(family, style, weight, stretch));
                        }
                    }
                }
                return typefaces;
            }
        }

        public Typeface? SelectedTypeface
        {
            get => _selectedTypeface;
            set
            {
                _selectedTypeface = value;
                OnPropertyChanged(nameof(SelectedTypeface));
                OnPropertyChanged(nameof(SelectedFontStyle));
                OnPropertyChanged(nameof(SelectedFontWeight));
                OnPropertyChanged(nameof(SelectedFontStretch));
                OnPropertyChanged(nameof(SelectedFontInfo));
            }
        }

        public FontStyle SelectedFontStyle => SelectedTypeface?.Style ?? FontStyle.Normal;

        public FontWeight SelectedFontWeight => SelectedTypeface?.Weight ?? FontWeight.Normal;

        public FontStretch SelectedFontStretch => SelectedTypeface?.Stretch ?? FontStretch.Normal;

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
                    SelectedFontFamily ?? FontFamily.Default,
                    SelectedTypeface?.Stretch ?? FontStretch.Normal,
                    SelectedTypeface?.Style ?? FontStyle.Normal,
                    SelectedTypeface?.Weight ?? FontWeight.Normal,
                    FontSize);
            }
        }

        public string DialogTitle => "Choose Font";
        public string LabelFamily => "Family";
        public string LabelTypeface => "Typeface";
        public string LabelSize => "Size";
        public string OkText => "OK";
        public string CancelText => "Cancel";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
