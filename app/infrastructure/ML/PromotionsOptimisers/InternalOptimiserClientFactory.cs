using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Infrastructure.ML;

namespace SignalBox.Infrastructure
{
    public class InternalOptimiserClientFactory : IInternalOptimiserClientFactory
    {
        private readonly IItemsRecommenderStore itemsRecommenderStore;
        private readonly ILogger<InternalOptimiserClientFactory> logger;

        public InternalOptimiserClientFactory(IItemsRecommenderStore itemsRecommenderStore,
                                              ILogger<InternalOptimiserClientFactory> logger)
        {
            this.itemsRecommenderStore = itemsRecommenderStore;
            this.logger = logger;
        }
        public Task<IRecommenderModelClient<ItemsRecommenderModelOutputV1>> GetInternalOptimiserClient()
        {
            logger.LogInformation("Creating internal optimiser client");
            return Task.FromResult<IRecommenderModelClient<ItemsRecommenderModelOutputV1>>(
                new PromotionsOptimiserClient(itemsRecommenderStore));
        }
    }
}
