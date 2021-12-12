using System.Text.Json;

namespace SignalBox.Core.Serialization
{
#nullable enable
    public static class Serializer
    {
        private static JsonSerializerOptions defaultOptions => new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static string Serialize<T>(T value, JsonSerializerOptions? serializerOptions = null)
        {
            return JsonSerializer.Serialize(value, typeof(T), serializerOptions ?? defaultOptions);
        }

        public static T? Deserialize<T>(string? value, JsonSerializerOptions? serializerOptions = null) where T : class
        {
            if (value == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(value, serializerOptions ?? defaultOptions);
        }
    }
}
