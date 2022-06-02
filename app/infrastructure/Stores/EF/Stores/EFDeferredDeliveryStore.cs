using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFCDeferredDeliveryStore : EFEntityStoreBase<DeferredDelivery>, IDeferredDeliveryStore
    {
        public EFCDeferredDeliveryStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.DeferredDeliveries)
        { }

        public async Task<IEnumerable<DeferredDelivery>> QueryForCustomer(long customerId)
        {
            var results = await QuerySet
                .Where(_ => _.Recommendation.CustomerId == customerId)
                .Include(_ => _.Channel)
                .Include(_ => _.Recommendation)
                .ThenInclude(_ => _.Customer)
                .Include(_ => _.Recommendation)
                .ThenInclude(_ => _.Items)
                .Include(_ => _.Recommendation)
                .ThenInclude(_ => _.DiscountCodes)
                .ToListAsync();
            return results;
        }
    }
}
