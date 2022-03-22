using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IRecommendableItemStore : ICommonEntityStore<RecommendableItem>
    {
        Task<Paginated<RecommendableItem>> QueryForRecommender(IPaginate paginate, long recommenderId);
    }
}