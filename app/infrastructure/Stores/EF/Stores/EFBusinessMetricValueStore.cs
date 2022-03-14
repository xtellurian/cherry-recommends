using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Metrics;

namespace SignalBox.Infrastructure.EntityFramework
{
#nullable enable
    public class EFBusinessMetricValueStore : EFEntityStoreBase<BusinessMetricValue>, IBusinessMetricValueStore
    {
        public EFBusinessMetricValueStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, c => c.BusinessMetrics)
        { }

        public async Task<BusinessMetricValue?> LatestMetricValue(Business business, Metric metric)
        {
            var latest = await QuerySet
                .Where(_ => _.BusinessId == business.Id && _.MetricId == metric.Id)
                .OrderByDescending(_ => _.Version)
                .FirstOrDefaultAsync();
            return latest;
        }
    }
}