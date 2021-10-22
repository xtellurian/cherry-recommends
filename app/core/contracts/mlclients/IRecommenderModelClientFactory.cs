using System.Threading.Tasks;

namespace SignalBox.Core
{
    // these can't be registered in SignalBox.Core because they depend on the implentation details
    public interface IRecommenderModelClientFactory
    {
        Task<IRecommenderModelClient<TOutput>> GetClient<TOutput>(IRecommender recommender) where TOutput : IModelOutput;
        Task<IRecommenderModelRewardClient> GetRewardClient(IRecommender recommender);
        Task<IRecommenderModelClient<TOutput>> GetUnregisteredClient<TOutput>(IRecommender recommender) where TOutput : IModelOutput;
        Task<IRecommenderModelClient<TOutput>> GetUnregisteredItemsRecommenderClient<TOutput>(IRecommender recommender) where TOutput : IModelOutput;
    }
}