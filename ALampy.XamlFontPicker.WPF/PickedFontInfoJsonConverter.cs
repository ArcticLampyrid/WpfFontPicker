#if !NET40
using System;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;

namespace ALampy.XamlFontPicker.WPF
{
    internal sealed class PickedFontInfoJsonConverter : JsonConverter<PickedFontInfo>
    {
        public override PickedFontInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of JSON object for PickedFontInfo.");

            string family = null;
            string stretch = null;
            string style = null;
            string weight = null;
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
                ParseFontFamily(family),
                ParseFontStretch(stretch),
                ParseFontStyle(style),
                ParseFontWeight(weight),
                size);
        }

        public override void Write(Utf8JsonWriter writer, PickedFontInfo value, JsonSerializerOptions options)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();
            writer.WriteString(nameof(PickedFontInfo.Family), ConvertToInvariantString(typeof(FontFamily), value.Family));
            writer.WriteString(nameof(PickedFontInfo.Stretch), ConvertToInvariantString(typeof(FontStretch), value.Stretch));
            writer.WriteString(nameof(PickedFontInfo.Style), ConvertToInvariantString(typeof(FontStyle), value.Style));
            writer.WriteString(nameof(PickedFontInfo.Weight), ConvertToInvariantString(typeof(FontWeight), value.Weight));
            writer.WriteNumber(nameof(PickedFontInfo.Size), value.Size);
            writer.WriteEndObject();
        }

        private static FontFamily ParseFontFamily(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new FontFamily();

            var converted = TypeDescriptor.GetConverter(typeof(FontFamily)).ConvertFromInvariantString(value) as FontFamily;
            return converted ?? new FontFamily();
        }

        private static FontStretch ParseFontStretch(string value)
        {
            if (string.IsNullOrEmpty(value))
                return FontStretches.Normal;

            var converted = TypeDescriptor.GetConverter(typeof(FontStretch)).ConvertFromInvariantString(value);
            return converted is FontStretch fontStretch ? fontStretch : FontStretches.Normal;
        }

        private static FontStyle ParseFontStyle(string value)
        {
            if (string.IsNullOrEmpty(value))
                return FontStyles.Normal;

            var converted = TypeDescriptor.GetConverter(typeof(FontStyle)).ConvertFromInvariantString(value);
            return converted is FontStyle fontStyle ? fontStyle : FontStyles.Normal;
        }

        private static FontWeight ParseFontWeight(string value)
        {
            if (string.IsNullOrEmpty(value))
                return FontWeights.Normal;

            var converted = TypeDescriptor.GetConverter(typeof(FontWeight)).ConvertFromInvariantString(value);
            return converted is FontWeight fontWeight ? fontWeight : FontWeights.Normal;
        }

        private static string ConvertToInvariantString(Type type, object value)
        {
            return TypeDescriptor.GetConverter(type).ConvertToInvariantString(value) ?? string.Empty;
        }
    }
}
#endif
