using System.Threading.Tasks;
using SignalBox.Core.Metrics;

namespace SignalBox.Core
{
#nullable enable
    public interface IBusinessMetricValueStore : IEntityStore<BusinessMetricValue>
    {
        Task<BusinessMetricValue?> LatestMetricValue(Business business, Metric metric);
    }
}