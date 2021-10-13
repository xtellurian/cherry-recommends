using SignalBox.Core.Recommenders;
using SignalBox.Web.Dto;

#nullable enable
namespace SignalBox.Web
{
    public static class RecommenderDtoConversionExtensions
    {
        private static T? DefaultIfNull<T>(T defaultValue, bool? writeNull)
        {
            if (writeNull == true)
            {
                return default(T);
            }
            else return defaultValue;
        }
        public static RecommenderSettings ToCoreRepresentation(this RecommenderSettingsDto? dto, RecommenderSettings? current = null, bool? writeNulls = null)
        {
            return new RecommenderSettings
            {
                RecommendationCacheTime = dto?.RecommendationCacheTime ?? DefaultIfNull(current?.RecommendationCacheTime, writeNulls),
                RequireConsumptionEvent = dto?.RequireConsumptionEvent ?? DefaultIfNull(current?.RequireConsumptionEvent, writeNulls),
                ThrowOnBadInput = dto?.ThrowOnBadInput ?? DefaultIfNull(current?.ThrowOnBadInput, writeNulls)
            };
        }
    }
}