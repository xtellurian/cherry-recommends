using System.Threading.Tasks;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public interface IProductRecommendationStore : IEntityStore<ProductRecommendation>
    {
        Task<Paginated<ProductRecommendation>> QueryForRecommender(int page, long recommenderId);
    }
}