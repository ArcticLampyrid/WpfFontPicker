using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia.Media;

namespace ALampy.XamlFontPicker.Avalonia
{
    public class TypefaceDisplayItem
    {
        public Typeface Typeface { get; }
        public string DisplayName { get; }

        public TypefaceDisplayItem(Typeface typeface)
        {
            Typeface = typeface;
            DisplayName = GetReadableName(typeface);
        }

        private static string GetReadableName(Typeface typeface)
        {
            return TypefaceDisplayNameHelper.GetDisplayName(typeface.Weight, typeface.Style, typeface.Stretch);
        }

        public override string ToString() => DisplayName;
    }

    public class FontDialogViewModel : INotifyPropertyChanged
    {
        private const double PixelsPerPoint = 96.0 / 72.0;

        private FontFamily _selectedFontFamily;
        private TypefaceDisplayItem? _selectedTypefaceItem;
        private double _fontSize;

        public FontDialogViewModel()
        {
            _selectedFontFamily = FontFamily.Default;
            _selectedTypefaceItem = null;
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
                OnPropertyChanged(nameof(FamilyTypefaces));
                OnPropertyChanged(nameof(SelectedFontInfo));
            }
        }

        public IList<TypefaceDisplayItem> FamilyTypefaces
        {
            get
            {
                var family = SelectedFontFamily;
                if (family == null)
                    return Array.Empty<TypefaceDisplayItem>();

                var typefaces = new List<TypefaceDisplayItem>();
                foreach (var weight in new[] { FontWeight.Thin, FontWeight.ExtraLight, FontWeight.Light, FontWeight.Normal, FontWeight.Medium, FontWeight.SemiBold, FontWeight.Bold, FontWeight.ExtraBold, FontWeight.Black })
                {
                    foreach (var style in new[] { FontStyle.Normal, FontStyle.Italic })
                    {
                        foreach (var stretch in new[] { FontStretch.UltraCondensed, FontStretch.ExtraCondensed, FontStretch.Condensed, FontStretch.SemiCondensed, FontStretch.Normal, FontStretch.SemiExpanded, FontStretch.Expanded, FontStretch.ExtraExpanded, FontStretch.UltraExpanded })
                        {
                            typefaces.Add(new TypefaceDisplayItem(new Typeface(family, style, weight, stretch)));
                        }
                    }
                }
                return typefaces;
            }
        }

        public TypefaceDisplayItem? SelectedTypefaceItem
        {
            get => _selectedTypefaceItem;
            set
            {
                _selectedTypefaceItem = value;
                OnPropertyChanged(nameof(SelectedTypefaceItem));
                OnPropertyChanged(nameof(SelectedTypeface));
                OnPropertyChanged(nameof(SelectedFontInfo));
            }
        }

        public Typeface? SelectedTypeface => SelectedTypefaceItem?.Typeface;

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

        public string DialogTitle => LocalizationService.GetString("FontDialog_Title", "Choose Font");
        public string LabelFamily => LocalizationService.GetString("FontDialog_Label_Family", "Family");
        public string LabelTypeface => LocalizationService.GetString("FontDialog_Label_Typeface", "Typeface");
        public string LabelSize => LocalizationService.GetString("FontDialog_Label_Size", "Size");
        public string OkText => LocalizationService.GetString("Common_Ok", "OK");
        public string CancelText => LocalizationService.GetString("Common_Cancel", "Cancel");

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
