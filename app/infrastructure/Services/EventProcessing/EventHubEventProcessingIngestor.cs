using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Infrastructure.Services
{
    public class EventHubEventProcessingIngestor : AzureEventHubEventIngestorBase<CustomerEventInput>, ICustomerEventIngestor
    {
        public EventHubEventProcessingIngestor(IOptionsMonitor<EventProcessingEventHubConfig> config) : base(config.CurrentValue)
        { }
    }
}
