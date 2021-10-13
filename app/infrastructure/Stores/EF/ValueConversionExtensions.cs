using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalBox.Core.Serialization;

namespace SignalBox.Infrastructure.EntityFramework
{
    public static class ValueConversionExtensions
    {
        private static JsonSerializerOptions _options;
        private static JsonSerializerOptions GetOptions()
        {
            if (_options == null)
            {
                _options = new JsonSerializerOptions();
                _options.Converters.Add(new TimeSpanJsonConverter()); // required to put timespans in JSON in the db
            }

            return _options;
        }

        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class, new()
        {
            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                v => JsonSerializer.Serialize(v, typeof(T), GetOptions()),
                v => JsonSerializer.Deserialize<T>(v, GetOptions()) ?? new T()
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                (l, r) => JsonSerializer.Serialize(l, typeof(T), GetOptions()) == JsonSerializer.Serialize(r, typeof(T), GetOptions()),
                v => v == null ? 0 : JsonSerializer.Serialize(v, typeof(T), GetOptions()).GetHashCode(),
                v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, typeof(T), GetOptions()), GetOptions())
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            // propertyBuilder.HasColumnType("NVARCHAR");

            return propertyBuilder;
        }

        public static PropertyBuilder<string> HasLowercaseConversion(this PropertyBuilder<string> propertyBuilder)
        {
            var converter = new ValueConverter<string, string>(
                v => v,
                v => v.ToLowerInvariant());

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);

            return propertyBuilder;
        }
    }
}