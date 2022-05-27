using System.Threading.Tasks;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
    public interface IRecommenderModelClient<TOutput> where TOutput : IModelOutput
    {
        Task<TOutput> Invoke(ICampaign campaign, RecommendingContext context, IModelInput input);
    }
}