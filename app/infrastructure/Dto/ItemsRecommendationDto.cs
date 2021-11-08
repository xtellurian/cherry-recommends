
using System.Collections.Generic;
using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.Dto
{
    // This is a public API in the webhook destination
    public class ItemsRecommendationDto : IRecommendationDto
    {
        public ItemsRecommendationDto(ItemsRecommendation recommendation)
        {
            this.Created = recommendation.Created;
            this.ScoredItems = recommendation.ScoredItems;
            this.CorrelatorId = recommendation.RecommendationCorrelatorId;
            this.CommonUserId = recommendation.TrackedUser.CommonUserId;
            this.Customer = recommendation.TrackedUser;
            this.Trigger = recommendation.Trigger;
        }

        public System.DateTimeOffset Created { get; set; }
        public long? CorrelatorId { get; set; }
        public string CommonUserId { get; private set; }
        public IEnumerable<ScoredRecommendableItem> ScoredItems { get; set; }
        public TrackedUser Customer { get; set; }
        public string Trigger { get; set; }
    }
}
