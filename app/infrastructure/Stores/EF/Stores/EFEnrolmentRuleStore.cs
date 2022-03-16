using SignalBox.Core;
using SignalBox.Core.Segments;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFEnrolmentRuleStore : EFEntityStoreBase<EnrolmentRule>, IEnrolmentRuleStore
    {
        public EFEnrolmentRuleStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, _ => _.EnrolmentRules)
        { }
    }
}