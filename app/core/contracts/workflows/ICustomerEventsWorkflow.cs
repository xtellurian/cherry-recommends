using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ICustomerEventsWorkflow
    {
        Task<EventLoggingResponse> Ingest(IEnumerable<CustomerEventInput> input);
        Task<EventLoggingResponse> ProcessEvents(IEnumerable<CustomerEventInput> input);
    }
}