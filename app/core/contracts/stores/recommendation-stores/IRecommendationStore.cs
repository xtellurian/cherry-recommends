using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public interface IRecommendationStore<T> : IEntityStore<T> where T : RecommendationEntity
    {
        Task<Paginated<T>> QueryForRecommender(int page, long recommenderId);
        Task<T> GetRecommendationFromCorrelator(long correlatorId);
        Task<bool> CorrelationExists(long? correlatorId);
        Task<IEnumerable<T>> RecommendationsSince(long recommenderId, Customer customer, DateTimeOffset since);
    }
}