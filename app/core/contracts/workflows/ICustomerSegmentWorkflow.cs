using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ICustomerSegmentWorkflow
    {
        Task<CustomerSegment> AddToSegment(Segment segment, Customer customer);
    }
}