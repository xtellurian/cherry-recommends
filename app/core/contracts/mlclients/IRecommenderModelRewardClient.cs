using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IRecommenderModelRewardClient
    {
        Task Reward(IRecommender recommender, RewardingContext context, TrackedUserAction action);
    }
}