using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IMetricStore : ICommonEntityStore<Metric>
    {
        Task<Paginated<Customer>> QueryCustomers(IPaginate paginate, long metricId);
    }
}