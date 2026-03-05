using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia.Media;

namespace ALampy.XamlFontPicker.Avalonia
{
    [Serializable]
    [JsonConverter(typeof(PickedFontInfoJsonConverter))]
    public class PickedFontInfo : IEquatable<PickedFontInfo>, ISerializable
    {
        public FontFamily Family { get; }
        public FontStretch Stretch { get; }
        public FontStyle Style { get; }
        public FontWeight Weight { get; }
        public double Size { get; }

        public string FamilyName => Family?.Name ?? string.Empty;

        public string TypefaceName => TypefaceDisplayNameHelper.GetDisplayName(Weight, Style, Stretch);

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

        public string ToJson(JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Serialize(this, options);
        }

        public static PickedFontInfo FromJson(string json, JsonSerializerOptions? options = null)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            var result = JsonSerializer.Deserialize<PickedFontInfo>(json, options);
            if (result == null)
                throw new JsonException("Failed to deserialize PickedFontInfo.");

            return result;
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(Family), Family.Name, typeof(string));
            info.AddValue(nameof(Stretch), Stretch.ToString(), typeof(string));
            info.AddValue(nameof(Style), Style.ToString(), typeof(string));
            info.AddValue(nameof(Weight), Weight.ToString(), typeof(string));
            info.AddValue(nameof(Size), Size);
        }

        protected PickedFontInfo(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            var familyName = info.GetString(nameof(Family)) ?? string.Empty;
            this.Family = string.IsNullOrEmpty(familyName) ? FontFamily.Default : new FontFamily(familyName);

            var stretchStr = info.GetString(nameof(Stretch)) ?? FontStretch.Normal.ToString();
            this.Stretch = Enum.Parse<FontStretch>(stretchStr);

            var styleStr = info.GetString(nameof(Style)) ?? FontStyle.Normal.ToString();
            this.Style = Enum.Parse<FontStyle>(styleStr);

            var weightStr = info.GetString(nameof(Weight)) ?? FontWeight.Normal.ToString();
            this.Weight = Enum.Parse<FontWeight>(weightStr);

            Size = info.GetDouble(nameof(Size));
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
