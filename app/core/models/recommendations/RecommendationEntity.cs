using System.Text.Json.Serialization;

namespace SignalBox.Core.Recommendations
{
#nullable enable
    public class RecommendationEntity : Entity
    {
        public RecommendationEntity()
        { }
        public RecommendationEntity(RecommendationCorrelator correlator)
        {
            this.RecommendationCorrelator = correlator;
        }

        public long? RecommendationCorrelatorId { get; set; }
        [JsonIgnore]
        public RecommendationCorrelator? RecommendationCorrelator { get; set; } // nullable for backwards compat
    }
}