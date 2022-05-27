using System.Threading.Tasks;

namespace SignalBox.Core
{
    // these can't be registered in SignalBox.Core because they depend on the implentation details
    public interface IRecommenderModelClientFactory
    {
        Task<IRecommenderModelClient<TOutput>> GetClient<TOutput>(ICampaign campaign) where TOutput : IModelOutput;
        Task<IRecommenderModelRewardClient> GetRewardClient(ICampaign campaign);
        Task<IRecommenderModelClient<TOutput>> GetUnregisteredClient<TOutput>(ICampaign campaign) where TOutput : IModelOutput;
        Task<IRecommenderModelClient<TOutput>> GetUnregisteredItemsRecommenderClient<TOutput>(ICampaign campaign) where TOutput : IModelOutput;
    }
}