using System.Threading.Tasks;
using SignalBox.Core;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core.Recommendations;
using System.Collections.Generic;
using System.Linq;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFArgumentRuleStore : EFEntityStoreBase<ArgumentRule>, IArgumentRuleStore
    {
        public EFArgumentRuleStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.ArgumentRules)
        { }
    }
}