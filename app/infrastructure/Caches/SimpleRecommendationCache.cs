using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure
{
    public abstract class SimpleRecommendationCache<TRecommender, TRecommendation> : IRecommendationCache<TRecommender, TRecommendation>
        where TRecommender : RecommenderEntityBase
        where TRecommendation : RecommendationEntity
    {
        private readonly IRecommendationStore<TRecommendation> recommendationStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public SimpleRecommendationCache(IRecommendationStore<TRecommendation> recommendationStore, IDateTimeProvider dateTimeProvider)
        {
            this.recommendationStore = recommendationStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        private async Task<IEnumerable<TRecommendation>> GetPossibleCachedRecommendations(TRecommender recommender,
                                                                                              Customer customer,
                                                                                              System.TimeSpan cacheTime)
        {
            var since = dateTimeProvider.Now.Subtract(cacheTime);
            var recommendations = await recommendationStore.RecommendationsSince(recommender.Id, customer, since);
            return recommendations;
        }

        public async Task<bool> HasCached(TRecommender recommender, Customer customer)
        {
            var cacheTime = recommender.Settings?.RecommendationCacheTime;
            if (cacheTime == null)
            {
                return false;
            }
            var recommendations = await GetPossibleCachedRecommendations(recommender, customer, cacheTime.Value);

            return recommendations.Any();
        }

        public async Task<TRecommendation> GetCached(TRecommender recommender, Customer customer)
        {
            var cacheTime = recommender.Settings?.RecommendationCacheTime;
            if (cacheTime == null)
            {
                throw new BadRequestException("Cannot read cache for recommendations without a cache time.");
            }
            var recommendations = await GetPossibleCachedRecommendations(recommender, customer, cacheTime.Value);
            var rec = recommendations.OrderByDescending(_ => _.Created).First();
            rec.IsFromCache = true;
            return rec;
        }

    }
}