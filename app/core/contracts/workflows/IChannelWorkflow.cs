using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IChannelWorkflow
    {
        Task<ChannelBase> CreateChannel(string name, ChannelTypes type, IntegratedSystem integratedSystem);
        Task<ChannelBase> UpdateChannelEndpoint(ChannelBase channel, string endpoint);
    }
}