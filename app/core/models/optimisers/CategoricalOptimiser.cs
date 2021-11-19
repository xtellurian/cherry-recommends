using System.Collections.Generic;

namespace SignalBox.Core.Optimisers
{
    public class CategoricalOptimiser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int NItemsToRecommend { get; set; }
        public RecommendableItem DefaultItem { get; set; }
        public IEnumerable<RecommendableItem> Items { get; set; }

    }
}