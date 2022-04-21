using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IDiscountCodeStore : IEntityStore<DiscountCode>
    {
        Task<EntityResult<DiscountCode>> GetFromCode(string code);
        Task<EntityResult<DiscountCode>> GetLatestByPromotion(RecommendableItem promotion);
    }
}