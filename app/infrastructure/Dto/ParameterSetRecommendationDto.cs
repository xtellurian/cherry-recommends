using System.Collections.Generic;
using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.Dto
{
    // This is a public API in the webhook destination
    public class ParameterSetRecommendationDto : IRecommendationDto
    {
        public ParameterSetRecommendationDto(ParameterSetRecommendation recommendation, Dictionary<string, object> parameters)
        {
            this.Customer = recommendation.Customer;
            this.RecommendedParameters = parameters;
            this.CorrelatorId = recommendation.RecommendationCorrelatorId;
            this.CommonUserId = recommendation.Customer?.CommonUserId;
            this.Created = recommendation.Created;
            this.Trigger = recommendation.Trigger;
        }

        public System.DateTimeOffset Created { get; set; }
        public long? CorrelatorId { get; set; }
        public IDictionary<string, object> RecommendedParameters { get; set; }
        public string CommonUserId { get; private set; }
        public string CustomerId => Customer?.CustomerId;
        public Customer Customer { get; set; }
        public string Trigger { get; set; }
    }
}