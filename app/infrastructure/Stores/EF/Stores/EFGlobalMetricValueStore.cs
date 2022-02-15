using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Metrics;

namespace SignalBox.Infrastructure.EntityFramework
{
#nullable enable
    public class EFGlobalMetricValueStore : EFEntityStoreBase<GlobalMetricValue>, IGlobalMetricValueStore
    {
        public EFGlobalMetricValueStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, c => c.GlobalMetrics)
        { }

        public async Task<GlobalMetricValue?> LatestMetricValue(Metric metric)
        {
            var latest = await QuerySet
                .Where(_ => _.MetricId == metric.Id)
                .OrderByDescending(_ => _.Version)
                .FirstOrDefaultAsync();
            return latest;
        }
    }
}