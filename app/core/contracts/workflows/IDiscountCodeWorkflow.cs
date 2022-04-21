using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IDiscountCodeWorkflow
    {
        Task<IEnumerable<DiscountCode>> GenerateDiscountCodes(RecommendableItem promotion);
    }
}