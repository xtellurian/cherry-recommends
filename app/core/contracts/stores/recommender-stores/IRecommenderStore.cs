using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IRecommenderStore<T> : ICommonEntityStore<T> where T : RecommenderEntityBase
    {
        Task<Paginated<InvokationLogEntry>> QueryInvokationLogs(IPaginate paginte, long id);
    }
}