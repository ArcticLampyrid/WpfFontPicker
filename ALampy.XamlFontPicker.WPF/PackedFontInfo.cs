using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;

namespace ALampy.XamlFontPicker.WPF
{
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Binary)]
    public class PackedFontInfo : IEquatable<PackedFontInfo>, ISerializable
    {
        public FontFamily Family { get; }
        public FontStretch Stretch { get; }
        public FontStyle Style { get; }
        public FontWeight Weight { get; }
        public double Size { get; }
        public string FamilyName => LanguageSpecificStringConverter.GetValue(Family.FamilyNames);
        public string TypefaceName => LanguageSpecificStringConverter.GetValue(new FamilyTypeface()
        {
            Stretch = Stretch,
            Style = Style,
            Weight = Weight
        }.AdjustedFaceNames);

        public void ApplyTo(dynamic control)
        {
            control.FontFamily = this.Family;
            control.FontStretch = this.Stretch;
            control.FontStyle = this.Style;
            control.FontWeight = this.Weight;
            control.FontSize = this.Size;
        }

        public static PackedFontInfo From(dynamic control)
        {
            return new PackedFontInfo(
                control.FontFamily,
                control.FontStretch,
                control.FontStyle,
                control.FontWeight,
                control.FontSize);
        }

        public PackedFontInfo() : this(new FontFamily(), FontStretches.Normal, FontStyles.Normal, FontWeights.Normal, 12)
        {
        }

        public PackedFontInfo(FontFamily family, FontStretch stretch, FontStyle style, FontWeight weight, double size)
        {
            this.Family = family;
            this.Stretch = stretch;
            this.Style = style;
            this.Weight = weight;
            this.Size = size;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(Family),
                TypeDescriptor.GetConverter(typeof(FontFamily)).ConvertToString(Family), typeof(string));
            info.AddValue(nameof(Stretch),
                TypeDescriptor.GetConverter(typeof(FontStretch)).ConvertToString(Stretch), typeof(string));
            info.AddValue(nameof(Style),
                TypeDescriptor.GetConverter(typeof(FontStyle)).ConvertToString(Style), typeof(string));
            info.AddValue(nameof(Weight),
                TypeDescriptor.GetConverter(typeof(FontWeight)).ConvertToString(Weight), typeof(string));
            info.AddValue(nameof(Size), Size);
        }

        protected PackedFontInfo(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            Family = (FontFamily)TypeDescriptor.GetConverter(typeof(FontFamily))
                .ConvertFromString((string)info.GetValue(nameof(Family), typeof(string)));

            Stretch = (FontStretch)TypeDescriptor.GetConverter(typeof(FontStretch))
                .ConvertFromString((string)info.GetValue(nameof(Stretch), typeof(string)));

            Style = (FontStyle)TypeDescriptor.GetConverter(typeof(FontStyle))
                .ConvertFromString((string)info.GetValue(nameof(Style), typeof(string)));

            Weight = (FontWeight)TypeDescriptor.GetConverter(typeof(FontWeight))
                .ConvertFromString((string)info.GetValue(nameof(Weight), typeof(string)));

            Size = info.GetDouble(nameof(Size));
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0} ({1}), Size: {2} ({3} pt)", FamilyName, TypefaceName, Size, Size * 0.75);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PackedFontInfo);
        }

        public bool Equals(PackedFontInfo other)
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

        public static bool operator ==(PackedFontInfo left, PackedFontInfo right)
        {
            return EqualityComparer<PackedFontInfo>.Default.Equals(left, right);
        }

        public static bool operator !=(PackedFontInfo left, PackedFontInfo right)
        {
            return !(left == right);
        }
    }
}
