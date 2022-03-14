using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Metrics;

namespace SignalBox.Core
{
#nullable enable
    public interface IBusinessMetricValueStore : IEntityStore<BusinessMetricValue>
    {
        Task<BusinessMetricValue?> LatestMetricValue(Business business, Metric metric);
        Task<IEnumerable<Metric>> GetMetricsFor(Business business);
        Task<BusinessMetricValue> ReadBusinessMetric(Business business, Metric metric, int? version = null);
    }
}