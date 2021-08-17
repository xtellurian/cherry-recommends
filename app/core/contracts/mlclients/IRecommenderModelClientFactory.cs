using System.Threading.Tasks;

namespace SignalBox.Core
{
    // these can't be registered in SignalBox.Core because they depend on the implentation details
    public interface IRecommenderModelClientFactory
    {
        Task<IRecommenderModelClient<TInput, TOutput>> GetClient<TInput, TOutput>(IRecommender recommender)
            where TInput : IModelInput
            where TOutput : IModelOutput;
        Task<IRecommenderModelRewardClient> GetRewardClient(IRecommender recommender);

        Task<IRecommenderModelClient<TInput, TOutput>> GetUnregisteredClient<TInput, TOutput>(IRecommender recommender)
            where TInput : IModelInput
            where TOutput : IModelOutput;
    }
}