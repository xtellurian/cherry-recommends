using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core.Recommenders
{
#nullable enable
    public class ItemsRecommenderPerformanceReport : PerformanceReportBase
    {
        protected ItemsRecommenderPerformanceReport()
        { }

        public ItemsRecommenderPerformanceReport(ItemsRecommender recommender, List<PerformanceByItem> performanceByItem)
        : base(recommender)
        {
            PerformanceByItem = performanceByItem;
        }

        public Dictionary<long, RecommendableItem> ItemsById => GetItemsById();
        public Dictionary<string, RecommendableItem> ItemsByCommonId => GetItemsByCommonId();
        public ItemsRecommender? ItemsRecommender => Recommender as ItemsRecommender;
        public Metric? TargetMetric => (Recommender as ItemsRecommender)?.TargetMetric;
        public List<PerformanceByItem>? PerformanceByItem { get; set; } // store as JSON

        private Dictionary<long, RecommendableItem> GetItemsById()
        {
            var kvps = ItemsRecommender?.Items?.Select(_ => new KeyValuePair<long, RecommendableItem>(_.Id, _));
            if (kvps != null)
            {
                return new Dictionary<long, RecommendableItem>(kvps);
            }
            else
            {
                return new Dictionary<long, RecommendableItem>();
            }
        }
        private Dictionary<string, RecommendableItem> GetItemsByCommonId()
        {
            var kvps = ItemsRecommender?.Items?.Select(_ => new KeyValuePair<string, RecommendableItem>(_.CommonId, _));
            if (kvps != null)
            {
                return new Dictionary<string, RecommendableItem>(kvps);
            }
            else
            {
                return new Dictionary<string, RecommendableItem>();
            }
        }
    }
}
