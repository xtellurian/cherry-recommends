using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFMetricGeneratorStore : EFEntityStoreBase<MetricGenerator>, IMetricGeneratorStore
    {
        public EFMetricGeneratorStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.MetricGenerators)
        { }
    }
}