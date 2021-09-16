using System.Collections.Generic;
using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Web.Dto
{
    public class ParameterSetRecommendationDto : IModelOutput
    {
        public ParameterSetRecommendationDto(ParameterSetRecommendation recommendation, Dictionary<string, object> parameters)
        {
            this.RecommendedParameters = parameters;
            this.CorrelatorId = recommendation.RecommendationCorrelatorId;
            this.CommonUserId = recommendation.TrackedUser.CommonUserId;
        }

        public long? CorrelatorId { get; set; }
        public IDictionary<string, object> RecommendedParameters { get; set; }
        public string CommonUserId { get; private set; }
    }
}