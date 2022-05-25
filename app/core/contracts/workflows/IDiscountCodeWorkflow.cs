using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IDiscountCodeWorkflow
    {
        /// <summary>
        /// Returns an empty list if no promotions can produce discount codes.
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        Task<IEnumerable<DiscountCode>> GenerateDiscountCodes(RecommendableItem promotion);
        Task LoadGeneratedAt(IEnumerable<DiscountCode> discountCodes);
    }
}