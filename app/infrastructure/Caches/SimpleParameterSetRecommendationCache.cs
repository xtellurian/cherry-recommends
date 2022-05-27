using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure
{
    public class SimpleParameterSetRecommendationCache : SimpleRecommendationCache<ParameterSetCampaign, ParameterSetRecommendation>
    {
        public SimpleParameterSetRecommendationCache(IParameterSetRecommendationStore store, IDateTimeProvider dateTimeProvider)
       : base(store, dateTimeProvider)
        { }
    }
}