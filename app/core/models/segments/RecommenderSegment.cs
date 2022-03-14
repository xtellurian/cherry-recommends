using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public class RecommenderSegment
    {
        public long SegmentId { get; set; }
        public long RecommenderId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Segment Segment { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RecommenderEntityBase Recommender { get; set; }

        protected RecommenderSegment()
        { }

        public RecommenderSegment(long recommenderId, long segmentId)
        {
            this.RecommenderId = recommenderId;
            this.SegmentId = segmentId;
        }

        public RecommenderSegment(RecommenderEntityBase recommender, Segment segment)
        {
            this.RecommenderId = recommender.Id;
            this.SegmentId = segment.Id;
            this.Recommender = recommender;
            this.Segment = segment;
        }
    }
}