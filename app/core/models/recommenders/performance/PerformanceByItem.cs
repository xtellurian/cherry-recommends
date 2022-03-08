using System.Text.Json.Serialization;

namespace SignalBox.Core.Recommenders
{
    public class PerformanceByItem
    {
        [JsonConstructor]
        public PerformanceByItem()
        { }
        // weirdly - having this constructor allows this to work as a JSON converted table cell.
        // otherwise, EF core complains this entity doest have a key, as it tried to put it into a tabls
        public PerformanceByItem(long itemId, int weekIndex, double targetMetricSum, int? customerCount, int? businessCount, int recommendationCount)
        {
            ItemId = itemId;
            WeekIndex = weekIndex;
            TargetMetricSum = targetMetricSum;
            CustomerCount = customerCount;
            BusinessCount = businessCount;
            RecommendationCount = recommendationCount;
        }

        public long ItemId { get; set; }
        public int WeekIndex { get; set; }
        public double TargetMetricSum { get; set; }
        public int? CustomerCount { get; set; }
        public int? BusinessCount { get; set; }
        public int RecommendationCount { get; set; }
    }
}