using System.Text.Json.Serialization;

namespace SignalBox.Core.Optimisers
{
#nullable enable
    /// <summary>
    /// Gives a weight to a Promotion for a given Optimiser and Segment.
    /// If the Segment is null, then the weight is the default for that Promotion.
    /// To begin with, let's ensure that the Segments are null, and we only have default.
    /// </summary>
    public class PromotionOptimiserWeight : OwnedEntity, IWeighted
    {
        protected PromotionOptimiserWeight() { }
        public PromotionOptimiserWeight(RecommendableItem item, PromotionOptimiser optimiser, double weight, long? segmentId = null)
        {
            Promotion = item;
            PromotionId = item.Id;
            Optimiser = optimiser;
            OptimiserId = optimiser.Id;
            Weight = weight;
            SegmentId = segmentId;
        }

        public double Weight { get; set; }

        public long? SegmentId { get; set; }
        [JsonIgnore]
        public Segment? Segment { get; set; }

        public long PromotionId { get; set; }
        [JsonIgnore]
        public RecommendableItem Promotion { get; set; } = null!;

        public long OptimiserId { get; set; }
        [JsonIgnore]
        public PromotionOptimiser Optimiser { get; set; } = null!;
    }
}