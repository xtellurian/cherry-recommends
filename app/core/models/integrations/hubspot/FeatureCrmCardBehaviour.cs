using System.Collections.Generic;

namespace SignalBox.Core.Adapters.Hubspot
{
#nullable enable
    public class FeatureCrmCardBehaviour
    {
        public HashSet<string>? ExcludedFeatures { get; set; }
        public HashSet<string>? IncludedFeatures { get; set; }
    }
}