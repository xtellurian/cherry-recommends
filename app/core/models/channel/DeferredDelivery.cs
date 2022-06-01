using System;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public class DeferredDelivery : Entity
    {
        protected DeferredDelivery() { }
        public DeferredDelivery(ChannelBase channel, ItemsRecommendation recommendation)
        {
            Channel = channel;
            ChannelId = channel.Id;
            Recommendation = recommendation;
            RecommendationId = recommendation.Id;
        }
        public bool? Sending { get; set; }
        public long ChannelId { get; set; }
        public ChannelBase Channel { get; set; }
        public long RecommendationId { get; set; }
        public ItemsRecommendation Recommendation { get; set; } // only works for an items recommendation
    }
}