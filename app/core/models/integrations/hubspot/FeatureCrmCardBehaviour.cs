using System.Collections.Generic;

namespace SignalBox.Core.Adapters.Hubspot
{
#nullable enable
    public class FeatureCrmCardBehaviour
    {
        public HashSet<string>? ExcludedFeatures { get; set; }
        public HashSet<string>? IncludedFeatures { get; set; }
        public long? ProductRecommenderId { get; set; }
        public long? ParameterSetRecommenderId { get; set; }

        public bool HasRecommender()
        {
            return ParameterSetRecommenderId != null || ProductRecommenderId != null;
        }
    }
}