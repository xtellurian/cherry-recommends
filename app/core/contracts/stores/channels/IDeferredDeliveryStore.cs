using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IDeferredDeliveryStore : IEntityStore<DeferredDelivery>
    {
        Task<IEnumerable<DeferredDelivery>> QueryForCustomer(long customerId);
    }
}
