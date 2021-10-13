using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure
{
    public class SimpleParameterSetRecommendationCache : SimpleRecommendationCache<ParameterSetRecommender, ParameterSetRecommendation>
    {
        public SimpleParameterSetRecommendationCache(IParameterSetRecommendationStore store, IDateTimeProvider dateTimeProvider)
       : base(store, dateTimeProvider)
        { }
    }
}