using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFCDeferredDeliveryStore : EFEntityStoreBase<DeferredDelivery>, IDeferredDeliveryStore
    {
        public EFCDeferredDeliveryStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.DeferredDeliveries)
        { }
    }
}
