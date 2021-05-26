using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFRuleStore : EFEntityStoreBase<Rule>, IRuleStore
    {
        public EFRuleStore(SignalBoxDbContext context)
        : base(context, (c) => c.Rules)
        {
        }
    }
}