using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SignalBox.Infrastructure.EntityFramework
{
    public static class ValueConversionExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class, new()
        {
            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                v => JsonSerializer.Serialize(v, typeof(T), null),
                v => JsonSerializer.Deserialize<T>(v, null) ?? new T()
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                (l, r) => JsonSerializer.Serialize(l, typeof(T), null) == JsonSerializer.Serialize(r, typeof(T), null),
                v => v == null ? 0 : JsonSerializer.Serialize(v, typeof(T), null).GetHashCode(),
                v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, typeof(T), null), null)
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