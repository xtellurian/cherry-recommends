using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ISegmentStore : IEntityStore<Segment>
    {
        Task<bool> ExistsInSegment(long segmentId, long customerId);
        Task<CustomerSegment> AddCustomer(Segment segment, Customer customer);
        Task<CustomerSegment> RemoveCustomer(Segment segment, Customer customer);
        Task<IEnumerable<Segment>> GetSegmentsByCustomer(Customer customer);
    }
}