using System.Threading.Tasks;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
    public interface ICampaignStore<T> : ICommonEntityStore<T> where T : CampaignEntityBase
    {
        Task<Paginated<InvokationLogEntry>> QueryInvokationLogs(IPaginate paginte, long id);
    }
}