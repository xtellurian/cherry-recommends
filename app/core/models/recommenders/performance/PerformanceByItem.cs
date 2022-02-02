using System.Text.Json.Serialization;

namespace SignalBox.Core.Recommenders
{
    public class PerformanceByItem
    {
        [JsonConstructor]
        public PerformanceByItem()
        { }
        // weirdly - having this constructor allows this to work as a JSON converted table cell.
        // otherwise, EF core complains this entity doest have a key, as it tried to put it into a tabl
        public PerformanceByItem(long itemId, int weekIndex, double targetMetricSum, double customerCount, double recommendationCount)
        {
            ItemId = itemId;
            WeekIndex = weekIndex;
            TargetMetricSum = targetMetricSum;
            CustomerCount = customerCount;
            RecommendationCount = recommendationCount;
        }

        public long ItemId { get; set; }
        public int WeekIndex { get; set; }
        public double TargetMetricSum { get; set; }
        public double CustomerCount { get; set; }
        public double RecommendationCount { get; set; }
    }
}