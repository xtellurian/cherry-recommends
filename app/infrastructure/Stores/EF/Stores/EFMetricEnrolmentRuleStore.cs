using SignalBox.Core;
using SignalBox.Core.Segments;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFMetricEnrolmentRuleStore : EFEntityStoreBase<MetricEnrolmentRule>, IMetricEnrolmentRuleStore
    {
        public EFMetricEnrolmentRuleStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, _ => _.MetricEnrolmentRules)
        { }
    }
}