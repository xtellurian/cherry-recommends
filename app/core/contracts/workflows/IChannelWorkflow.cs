using System.Threading.Tasks;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public interface IChannelWorkflow
    {
        Task<bool> CanSend(ChannelBase channel, RecommendationEntity recommendation);
        Task<ChannelBase> CreateChannel(string name, ChannelTypes type, IntegratedSystem integratedSystem);
        Task<ChannelBase> UpdateChannelEndpoint(ChannelBase channel, string endpoint);
        Task<ChannelBase> UpdateWebChannelProperties(ChannelBase channel, string host, bool? popupAskForEmail = null, int? popupDelay = null, string popupHeader = "", string popupSubheader = "", long? recommenderId = null, string customerIdPrefix = "");
        Task<ChannelBase> UpdateEmailChannelListTrigger(ChannelBase channel, string listId, string listName);
    }
}