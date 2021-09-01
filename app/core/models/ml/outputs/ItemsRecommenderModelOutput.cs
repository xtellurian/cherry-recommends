using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
#nullable enable
    public class ItemsRecommenderModelOutputV1 : IModelOutput
    {
#nullable enable
        public ItemsRecommenderModelOutputV1()
        { }

        public List<ScoredRecommendableItem> ScoredItems { get; set; } = new List<ScoredRecommendableItem>();
        public long? CorrelatorId { get; set; }
    }
}