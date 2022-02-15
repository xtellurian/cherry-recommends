using System.Threading.Tasks;
using SignalBox.Core.Metrics;

namespace SignalBox.Core
{
#nullable enable
    public interface IGlobalMetricValueStore : IEntityStore<GlobalMetricValue>
    {
        Task<GlobalMetricValue?> LatestMetricValue(Metric metric);
    }
}