
using System.Collections.Generic;
using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Web.Dto
{
    public class ItemsRecommendationDto : IModelOutput
    {
        public ItemsRecommendationDto(ItemsRecommendation recommendation)
        {
            this.ScoredItems = recommendation.ScoredItems;
            this.CorrelatorId = recommendation.RecommendationCorrelatorId;
            this.CommonUserId = recommendation.TrackedUser.CommonUserId;
            this.Created = recommendation.Created;
        }

        public System.DateTimeOffset? Created { get; set; }
        public long? CorrelatorId { get; set; }
        public string CommonUserId { get; private set; }
        public IEnumerable<ScoredRecommendableItem> ScoredItems { get; set; }
    }
}
