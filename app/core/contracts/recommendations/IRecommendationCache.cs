using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IRecommendationCache<TRecommender, TRecommendation>
        where TRecommender : RecommenderEntityBase
        where TRecommendation : RecommendationEntity
    {
        Task<bool> HasCached(TRecommender recommender, Customer customer);
        Task<TRecommendation> GetCached(TRecommender recommender, Customer customer);
    }
}