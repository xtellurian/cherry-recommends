using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ICustomerEventIngestor
    {
        bool CanIngest { get; }
        Task Ingest(IEnumerable<CustomerEventInput> inputs);
    }
}