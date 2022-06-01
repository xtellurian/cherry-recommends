using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ICustomerHasUpdatedIngestor
    {
        bool CanIngest { get; }
        Task Ingest(IEnumerable<CustomerHasUpdated> inputs);
    }
}
