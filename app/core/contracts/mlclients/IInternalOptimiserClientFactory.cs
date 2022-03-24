using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IInternalOptimiserClientFactory
    {
        Task<IRecommenderModelClient<ItemsRecommenderModelOutputV1>> GetInternalOptimiserClient();
    }
}