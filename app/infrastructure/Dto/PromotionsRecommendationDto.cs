
using System.Collections.Generic;
using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.Dto
{
    // This is a public API in the webhook destination
    public class PromotionsRecommendationDto : IRecommendationDto
    {
        public PromotionsRecommendationDto(ItemsRecommendation recommendation)
        {
            this.Created = recommendation.Created;
            this.ScoredItems = recommendation.ScoredItems;
            this.CorrelatorId = recommendation.RecommendationCorrelatorId;
            this.CommonUserId = recommendation.Customer?.CommonUserId;
            this.Customer = recommendation.Customer;
            this.Trigger = recommendation.Trigger;
        }

        public System.DateTimeOffset Created { get; set; }
        public long? CorrelatorId { get; set; }
        public string CommonUserId { get; private set; }
        public string CustomerId => Customer?.CustomerId;
        public IEnumerable<ScoredRecommendableItem> ScoredItems { get; set; }
        public Customer Customer { get; set; }
        public string Trigger { get; set; }
    }
}
