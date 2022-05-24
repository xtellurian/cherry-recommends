using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IDiscountCodeStore : IEntityStore<DiscountCode>
    {
        Task<EntityResult<DiscountCode>> ReadByCode(string code);
        Task<EntityResult<DiscountCode>> ReadLatestByPromotion(RecommendableItem promotion);
    }
}