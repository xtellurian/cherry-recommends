using System;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace SignalBox.Core.Serialization
{
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan?>
    {
        public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (TimeSpan.TryParse(s, out var t))
                {
                    return t;
                }
            }
            else
            {
                // something went wrong here
                reader.Skip();
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
        {
            var s = value?.ToString();
            writer.WriteStringValue(s);
        }
    }
}