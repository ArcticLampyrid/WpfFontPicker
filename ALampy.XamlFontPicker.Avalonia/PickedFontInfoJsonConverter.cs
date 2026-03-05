using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia.Media;

namespace ALampy.XamlFontPicker.Avalonia
{
    internal sealed class PickedFontInfoJsonConverter : JsonConverter<PickedFontInfo>
    {
        public override PickedFontInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of JSON object for PickedFontInfo.");

            string? family = null;
            string? stretch = null;
            string? style = null;
            string? weight = null;
            double size = 12;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Expected property name token while reading PickedFontInfo.");

                var propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case nameof(PickedFontInfo.Family):
                        family = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    case nameof(PickedFontInfo.Stretch):
                        stretch = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    case nameof(PickedFontInfo.Style):
                        style = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    case nameof(PickedFontInfo.Weight):
                        weight = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                        break;
                    case nameof(PickedFontInfo.Size):
                        if (!reader.TryGetDouble(out size))
                            throw new JsonException("Property 'Size' must be a number.");
                        break;
                    default:
                        using (JsonDocument.ParseValue(ref reader))
                        {
                        }
                        break;
                }
            }

            return new PickedFontInfo(
                string.IsNullOrWhiteSpace(family) ? FontFamily.Default : new FontFamily(family),
                ParseEnumOrDefault(stretch, FontStretch.Normal),
                ParseEnumOrDefault(style, FontStyle.Normal),
                ParseEnumOrDefault(weight, FontWeight.Normal),
                size);
        }

        public override void Write(Utf8JsonWriter writer, PickedFontInfo value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString(nameof(PickedFontInfo.Family), value.Family?.Name ?? string.Empty);
            writer.WriteString(nameof(PickedFontInfo.Stretch), value.Stretch.ToString());
            writer.WriteString(nameof(PickedFontInfo.Style), value.Style.ToString());
            writer.WriteString(nameof(PickedFontInfo.Weight), value.Weight.ToString());
            writer.WriteNumber(nameof(PickedFontInfo.Size), value.Size);
            writer.WriteEndObject();
        }

        private static TEnum ParseEnumOrDefault<TEnum>(string? value, TEnum fallback)
            where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(value))
                return fallback;

            return Enum.TryParse(value, true, out TEnum parsed) ? parsed : fallback;
        }
    }
}
