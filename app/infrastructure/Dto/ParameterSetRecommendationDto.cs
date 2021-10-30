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
            this.RecommendedParameters = parameters;
            this.CorrelatorId = recommendation.RecommendationCorrelatorId;
            this.CommonUserId = recommendation.TrackedUser.CommonUserId;
            this.Customer = recommendation.TrackedUser;
        }

        public long? CorrelatorId { get; set; }
        public IDictionary<string, object> RecommendedParameters { get; set; }
        public string CommonUserId { get; private set; }
        public TrackedUser Customer { get; set; }
    }
}