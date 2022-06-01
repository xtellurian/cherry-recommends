using System.Threading.Tasks;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public interface IChannelDeliveryWorkflow
    {
        Task SendToChannel(ChannelBase channel, RecommendationEntity recommendation);
        Task OnCustomerUpdated(long customerId);
    }
}