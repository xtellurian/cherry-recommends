using System.Collections.Generic;
using System.Threading.Tasks;
using static SignalBox.Core.Workflows.CustomerEventsWorkflows;

namespace SignalBox.Core
{
    public interface IEventIngestor
    {
        bool CanIngest { get; }
        Task Ingest(IEnumerable<CustomerEventInput> inputs);
    }
}