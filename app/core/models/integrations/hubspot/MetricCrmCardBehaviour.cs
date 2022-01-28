using System.Collections.Generic;

namespace SignalBox.Core.Adapters.Hubspot
{
#nullable enable
    public class MetricCrmCardBehaviour
    {
        public HashSet<string>? ExcludedFeatures
        {
            get => IncludedMetrics; set
            {
                if (value != null)
                {
                    ExcludedMetrics = value;
                }
            }
        }
        public HashSet<string>? ExcludedMetrics { get; set; }
        public HashSet<string>? IncludedFeatures
        {
            get => IncludedMetrics; set
            {
                if (value != null)
                {
                    IncludedMetrics = value;
                }
            }
        }
        public HashSet<string>? IncludedMetrics { get; set; }
        public long? ItemsRecommenderId { get; set; }
        public long? ParameterSetRecommenderId { get; set; }

        public bool HasRecommender()
        {
            return ParameterSetRecommenderId != null || ItemsRecommenderId != null;
        }
    }
}