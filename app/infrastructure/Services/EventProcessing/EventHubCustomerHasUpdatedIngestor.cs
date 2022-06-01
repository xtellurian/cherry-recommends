using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Infrastructure.Services
{
    public class EventHubCustomerHasUpdatedIngestor : AzureEventHubEventIngestorBase<CustomerHasUpdated>, ICustomerHasUpdatedIngestor
    {
        public EventHubCustomerHasUpdatedIngestor(IOptionsMonitor<CustomerHasUpdatedEventHubConfig> config) : base(config.CurrentValue)
        { }
    }
}
