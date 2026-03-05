using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
#if !NET40
using System.Text.Json;
using System.Text.Json.Serialization;
#endif

namespace ALampy.XamlFontPicker.WPF
{
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Binary)]
#if !NET40
    [JsonConverter(typeof(PickedFontInfoJsonConverter))]
#endif
    public class PickedFontInfo : IEquatable<PickedFontInfo>, ISerializable
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

        public PickedFontInfo() : this(new FontFamily(), FontStretches.Normal, FontStyles.Normal, FontWeights.Normal, 12)
        {
        }

        public PickedFontInfo(FontFamily family, FontStretch stretch, FontStyle style, FontWeight weight, double size)
        {
            this.Family = family ?? new FontFamily();
            this.Stretch = stretch;
            this.Style = style;
            this.Weight = weight;
            this.Size = size;
        }

#if !NET40
        public string ToJson(JsonSerializerOptions options = null)
        {
            return JsonSerializer.Serialize(this, options);
        }

        public static PickedFontInfo FromJson(string json, JsonSerializerOptions options = null)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            var result = JsonSerializer.Deserialize<PickedFontInfo>(json, options);
            if (result == null)
                throw new JsonException("Failed to deserialize PickedFontInfo.");

            return result;
        }
#endif

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

        protected PickedFontInfo(SerializationInfo info, StreamingContext context)
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
            var pointSize = Size * 72.0 / 96.0;
            return string.Format(CultureInfo.CurrentCulture, "{0} ({1}), Size: {2:0.##} pt", FamilyName, TypefaceName, pointSize);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PickedFontInfo);
        }

        public bool Equals(PickedFontInfo other)
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

        public static bool operator ==(PickedFontInfo left, PickedFontInfo right)
        {
            return EqualityComparer<PickedFontInfo>.Default.Equals(left, right);
        }

        public static bool operator !=(PickedFontInfo left, PickedFontInfo right)
        {
            return !(left == right);
        }
    }
}
