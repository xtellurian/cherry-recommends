using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IRecommendableItemStore : ICommonEntityStore<RecommendableItem>
    {
        Task<Paginated<RecommendableItem>> QueryForRecommender(long recommenderId, int page);
    }
}