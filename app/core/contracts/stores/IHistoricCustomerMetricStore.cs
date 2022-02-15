using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IHistoricCustomerMetricStore : IEntityStore<HistoricCustomerMetric>
    {
        // Should return the latest metric if version = null
        Task<HistoricCustomerMetric> ReadCustomerMetric(Customer customer, Metric metric, int? version = null);
        Task<bool> MetricExists(Customer customer, Metric metric, int? version = null);
        Task<int> CurrentMaximumCustomerMetricVersion(Customer customer, Metric metric);
        Task<IEnumerable<Metric>> GetMetricsFor(Customer customer);
        Task<IEnumerable<CustomerMetricWeeklyNumericAggregate>> GetAggregateMetricValuesNumeric(Metric metric, int weeksAgo = 11);
        Task<IEnumerable<CustomerMetricWeeklyStringAggregate>> GetAggregateMetricValuesString(Metric metric, int weeksAgo = 11);
    }
}