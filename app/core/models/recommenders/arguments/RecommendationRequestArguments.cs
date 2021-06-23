using System.Collections.Generic;

namespace SignalBox.Core
{
#nullable enable
    public class RecommendationRequestArguments
    {
        public RecommendationRequestArguments()
        { }
        public RecommendationRequestArguments(Dictionary<string, object>? features)
        {
            Features = features ?? new Dictionary<string, object>();
        }

        public Dictionary<string, object>? Features { get; set; }

    }
}