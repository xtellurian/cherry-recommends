using System.Threading.Tasks;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public interface IRecommendationStore<T> : IEntityStore<T> where T : RecommendationEntity
    {
        Task<Paginated<T>> QueryForRecommender(int page, long recommenderId);
    }
}