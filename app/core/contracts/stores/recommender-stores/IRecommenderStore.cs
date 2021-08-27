using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IRecommenderStore<T> : ICommonEntityStore<T> where T : RecommenderEntityBase
    {
        Task<Paginated<TrackedUserAction>> QueryAssociatedActions(T recommender, int page, bool revenueOnly);
        Task<Paginated<InvokationLogEntry>> QueryInvokationLogs(long id, int page);
    }
}