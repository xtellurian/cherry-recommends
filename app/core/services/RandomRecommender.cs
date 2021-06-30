using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public class RandomRecommender : IRecommender<OfferRecommendation>
    {
        private readonly IRecommendationCorrelatorStore correlatorStore;

        public RandomRecommender(IRecommendationCorrelatorStore correlatorStore)
        {
            this.correlatorStore = correlatorStore;
        }
        public static List<T> GetRandomElements<T>(IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }

        public async Task<OfferRecommendation> Recommend(PresentationContext context)
        {
            var orderedResults = GetRandomElements(context.Experiment.Offers, context.Experiment.ConcurrentOffers);
            var correlation = await correlatorStore.Create(new RecommendationCorrelator());
            return new OfferRecommendation(correlation, orderedResults, context.TrackedUser, context.Experiment, context.Features);
        }

        public Task<OfferRecommendation> Recommend(RecommendationRequestArguments context)
        {
            return this.Recommend((PresentationContext)context);
        }
    }
}
