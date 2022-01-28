using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IMetricStore : ICommonEntityStore<Metric>
    {
        Task<Paginated<Customer>> QueryCustomers(int page, long metricId);
    }
}