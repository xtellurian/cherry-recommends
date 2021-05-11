using System.Threading.Tasks;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class OfferWorkflows
    {
        private readonly IStorageContext storageContext;
        private readonly IOfferStore offerStore;

        public OfferWorkflows(IStorageContext storageContext, IOfferStore offerStore)
        {
            this.storageContext = storageContext;
            this.offerStore = offerStore;
        }

        public async Task<Offer> CreateOffer(string name,
                                             double? price,
                                             double? cost,
                                             string? currency = "USD",
                                             string? discountCode = null)
        {
            if (currency == null)
            {
                throw new WorkflowException($"An offer required a name");
            }

            var offer = await offerStore.Create(new Offer(name, currency, price ?? 0, cost, discountCode));
            await storageContext.SaveChanges();
            return offer;
        }
    }
}