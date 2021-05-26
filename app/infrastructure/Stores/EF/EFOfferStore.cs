using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFOfferStore : EFEntityStoreBase<Offer>, IOfferStore
    {
        public EFOfferStore(SignalBoxDbContext context) 
        : base(context, (c) => c.Offers)
        {
        }
    }
}