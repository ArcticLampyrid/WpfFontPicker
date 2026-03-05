using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
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
            DisplayName = TypefaceDisplayNameHelper.GetDisplayName(typeface.Weight, typeface.Style, typeface.Stretch);
        }

        public override string ToString() => DisplayName;
    }

    public class FontDialogViewModel : INotifyPropertyChanged
    {
        private static readonly ResourceManager ResourceManager =
            new("ALampy.XamlFontPicker.Avalonia.Resources.Strings", typeof(FontDialogViewModel).Assembly);

        private const double PixelsPerPoint = 96.0 / 72.0;

        private FontFamily _selectedFontFamily;
        private IList<TypefaceDisplayItem> _familyTypefaces;
        private TypefaceDisplayItem? _selectedTypefaceItem;
        private double _fontSize;

        public FontDialogViewModel()
        {
            _fontSize = 12;

            FontFamilies = BuildFontFamilies();
            _selectedFontFamily = FontFamilies.FirstOrDefault() ?? FontFamily.Default;

            _familyTypefaces = BuildFamilyTypefaces(_selectedFontFamily);
            _selectedTypefaceItem = _familyTypefaces.FirstOrDefault();

            FontSizeOptions = FontSizeOption.GetDefaultSizeOptions();
        }

        public IList<FontFamily> FontFamilies { get; }

        public IList<FontSizeOption> FontSizeOptions { get; }

        public FontFamily SelectedFontFamily
        {
            get => _selectedFontFamily;
            set
            {
                _selectedFontFamily = value ?? FontFamily.Default;
                OnPropertyChanged(nameof(SelectedFontFamily));

                var previousTypeface = _selectedTypefaceItem?.Typeface;
                _familyTypefaces = BuildFamilyTypefaces(_selectedFontFamily);
                OnPropertyChanged(nameof(FamilyTypefaces));

                SelectedTypefaceItem = GetMatchingTypefaceItem(previousTypeface);
                OnPropertyChanged(nameof(SelectedFontInfo));
            }
        }

        public IList<TypefaceDisplayItem> FamilyTypefaces => _familyTypefaces;

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

        public PickedFontInfo SelectedFontInfo =>
            new(
                SelectedFontFamily ?? FontFamily.Default,
                SelectedTypeface?.Stretch ?? FontStretch.Normal,
                SelectedTypeface?.Style ?? FontStyle.Normal,
                SelectedTypeface?.Weight ?? FontWeight.Normal,
                FontSize);

        public string DialogTitle => GetString(nameof(DialogTitle), "FontDialog_Title");
        public string LabelFamily => GetString(nameof(LabelFamily), "FontDialog_Label_Family");
        public string LabelTypeface => GetString(nameof(LabelTypeface), "FontDialog_Label_Typeface");
        public string LabelSize => GetString(nameof(LabelSize), "FontDialog_Label_Size");
        public string OkText => GetString(nameof(OkText), "Common_Ok");
        public string CancelText => GetString(nameof(CancelText), "Common_Cancel");

        public event PropertyChangedEventHandler? PropertyChanged;

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

        private static IList<TypefaceDisplayItem> BuildFamilyTypefaces(FontFamily family)
        {
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
            if (previousTypeface is Typeface previous)
            {
                var matchingTypeface = _familyTypefaces.FirstOrDefault(item =>
                    item.Typeface.Weight == previous.Weight
                    && item.Typeface.Style == previous.Style
                    && item.Typeface.Stretch == previous.Stretch);

                if (matchingTypeface != null)
                    return matchingTypeface;
            }

            return _familyTypefaces.FirstOrDefault();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static string GetString(string fallback, string key)
        {
            return ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? fallback;
        }
    }
}
