using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFRuleStore : EFEntityStoreBase<Rule>, IRuleStore
    {
        public EFRuleStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.Rules)
        { }
    }
}