using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public static class HistoricCustomerMetricStoreExtensions
    {
        public static async Task<IEnumerable<HistoricCustomerMetric>> ReadAllLatestMetrics(this IHistoricCustomerMetricStore store, Customer customer)
        {
            var metrics = await store.GetMetricsFor(customer);
            var results = new List<HistoricCustomerMetric>();
            foreach (var metric in metrics)
            {
                results.Add(await store.ReadCustomerMetric(customer, metric));
            }
            return results;
        }
    }
}