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
            _fontSize = 12;

            FontFamilies = BuildFontFamilies();
            _selectedFontFamily = FontFamilies.FirstOrDefault() ?? FontFamily.Default;

            FontSizeOptions = FontSizeOption.GetDefaultSizeOptions();
            _selectedTypefaceItem = BuildFamilyTypefaces(_selectedFontFamily).FirstOrDefault();
        }

        public IList<FontFamily> FontFamilies { get; }

        public IList<FontSizeOption> FontSizeOptions { get; }

        private static List<FontFamily> BuildFontFamilies()
        {
            var fontFamilies = FontManager.Current.SystemFonts
                .OrderBy(f => f.Name)
                .ToList();

            var hasDefaultFontFamily = fontFamilies.Any(fontFamily =>
                string.Equals(fontFamily.Name, FontFamily.Default.Name, StringComparison.CurrentCultureIgnoreCase));

            if (!hasDefaultFontFamily)
                fontFamilies.Insert(0, FontFamily.Default);

            return fontFamilies;
        }

        private static IList<TypefaceDisplayItem> BuildFamilyTypefaces(FontFamily? family)
        {
            if (family == null)
                return Array.Empty<TypefaceDisplayItem>();

            var typefaces = family.FamilyTypefaces
                .GroupBy(typeface => (typeface.Weight, typeface.Style, typeface.Stretch))
                .Select(group => group.First())
                .Select(typeface => new TypefaceDisplayItem(typeface))
                .OrderBy(item => item.Typeface.Weight)
                .ThenBy(item => item.Typeface.Style)
                .ThenBy(item => item.Typeface.Stretch)
                .ToList();

            if (typefaces.Count == 0)
                typefaces.Add(new TypefaceDisplayItem(new Typeface(family, FontStyle.Normal, FontWeight.Normal, FontStretch.Normal)));

            return typefaces;
        }

        private TypefaceDisplayItem? GetMatchingTypefaceItem(Typeface? previousTypeface)
        {
            var familyTypefaces = FamilyTypefaces;

            if (previousTypeface is Typeface previous)
            {
                var matchingTypeface = familyTypefaces.FirstOrDefault(item =>
                    item.Typeface.Weight == previous.Weight
                    && item.Typeface.Style == previous.Style
                    && item.Typeface.Stretch == previous.Stretch);

                if (matchingTypeface != null)
                    return matchingTypeface;
            }

            return familyTypefaces.FirstOrDefault();
        }

        public FontFamily SelectedFontFamily
        {
            get => _selectedFontFamily;
            set
            {
                _selectedFontFamily = value;
                OnPropertyChanged(nameof(SelectedFontFamily));
                OnPropertyChanged(nameof(FamilyTypefaces));

                var previousTypeface = _selectedTypefaceItem?.Typeface;
                SelectedTypefaceItem = GetMatchingTypefaceItem(previousTypeface);

                OnPropertyChanged(nameof(SelectedFontInfo));
            }
        }

        public IList<TypefaceDisplayItem> FamilyTypefaces => BuildFamilyTypefaces(SelectedFontFamily);

        public TypefaceDisplayItem? SelectedTypefaceItem
        {
            get => _selectedTypefaceItem;
            set
            {
                _selectedTypefaceItem = value;
                OnPropertyChanged(nameof(SelectedTypefaceItem));
                OnPropertyChanged(nameof(SelectedTypeface));
                OnPropertyChanged(nameof(SelectedFontStyle));
                OnPropertyChanged(nameof(SelectedFontWeight));
                OnPropertyChanged(nameof(SelectedFontStretch));
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
