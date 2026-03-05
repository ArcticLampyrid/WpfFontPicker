using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Media;

namespace ALampy.XamlFontPicker.Avalonia
{
    public class PickedFontInfo : IEquatable<PickedFontInfo>
    {
        public FontFamily Family { get; }
        public FontStretch Stretch { get; }
        public FontStyle Style { get; }
        public FontWeight Weight { get; }
        public double Size { get; }

        public string FamilyName => Family?.Name ?? string.Empty;

        public string TypefaceName
        {
            get
            {
                var typeface = new Typeface(Family ?? FontFamily.Default, Style, Weight, Stretch);
                return typeface.ToString() ?? string.Empty;
            }
        }

        public PickedFontInfo() : this(FontFamily.Default, FontStretch.Normal, FontStyle.Normal, FontWeight.Normal, 12)
        {
        }

        public PickedFontInfo(FontFamily family, FontStretch stretch, FontStyle style, FontWeight weight, double size)
        {
            this.Family = family ?? FontFamily.Default;
            this.Stretch = stretch;
            this.Style = style;
            this.Weight = weight;
            this.Size = size;
        }

        public override string ToString()
        {
            var pointSize = Size * 72.0 / 96.0;
            return string.Format(CultureInfo.CurrentCulture, "{0} ({1}), Size: {2:0.##} pt", FamilyName, TypefaceName, pointSize);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PickedFontInfo);
        }

        public bool Equals(PickedFontInfo? other)
        {
            return other != null &&
                   EqualityComparer<FontFamily>.Default.Equals(Family, other.Family) &&
                   EqualityComparer<FontStretch>.Default.Equals(Stretch, other.Stretch) &&
                   EqualityComparer<FontStyle>.Default.Equals(Style, other.Style) &&
                   EqualityComparer<FontWeight>.Default.Equals(Weight, other.Weight) &&
                   Size == other.Size;
        }

        public override int GetHashCode()
        {
            var hashCode = 706081142;
            hashCode = hashCode * -1521134295 + EqualityComparer<FontFamily>.Default.GetHashCode(Family);
            hashCode = hashCode * -1521134295 + Stretch.GetHashCode();
            hashCode = hashCode * -1521134295 + Style.GetHashCode();
            hashCode = hashCode * -1521134295 + Weight.GetHashCode();
            hashCode = hashCode * -1521134295 + Size.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(PickedFontInfo? left, PickedFontInfo? right)
        {
            return EqualityComparer<PickedFontInfo?>.Default.Equals(left, right);
        }

        public static bool operator !=(PickedFontInfo? left, PickedFontInfo? right)
        {
            return !(left == right);
        }
    }
}
