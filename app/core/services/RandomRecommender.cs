using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public class RandomRecommender : IRecommender
    {
        public static List<T> GetRandomElements<T>(IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }

        public Task<OfferRecommendation> Recommend(PresentationContext context)
        {
            var orderedResults = GetRandomElements(context.Experiment.Offers, context.Experiment.ConcurrentOffers);
            return Task.FromResult(new OfferRecommendation(orderedResults, context.TrackedUser, context.Experiment, context.Features));
        }
    }
}
