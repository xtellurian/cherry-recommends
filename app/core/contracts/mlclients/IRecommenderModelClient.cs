using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IRecommenderModelClient<TInput, TOutput> where TInput : IModelInput where TOutput : IModelOutput
    {
        Task<TOutput> Invoke(IRecommender recommender, RecommendingContext context, TInput input);
        Task Reward(IRecommender recommender, RewardingContext context, TrackedUserAction action);
    }
}