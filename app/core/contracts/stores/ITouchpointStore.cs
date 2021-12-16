using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITouchpointStore : ICommonEntityStore<Touchpoint>
    {
        Task<Paginated<Customer>> QueryTrackedUsers(int page, long touchpointId);
    }
}