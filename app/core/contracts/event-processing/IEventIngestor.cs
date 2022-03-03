using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IEventIngestor
    {
        bool CanIngest { get; }
        Task Ingest(IEnumerable<CustomerEventInput> inputs);
    }
}