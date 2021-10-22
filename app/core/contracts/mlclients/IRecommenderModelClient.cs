using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IRecommenderModelClient<TOutput> where TOutput : IModelOutput
    {
        Task<TOutput> Invoke(IRecommender recommender, RecommendingContext context, IModelInput input);
        Task Reward(IRecommender recommender, RewardingContext context, TrackedUserAction action);
    }
}