using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    [Obsolete("This entity or table is obsolete.")]
    public class EFRewardSelectorStore : EFEntityStoreBase<RewardSelector>, IRewardSelectorStore
    {
        public EFRewardSelectorStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.RewardSelectors)
        { }

        public async Task<IEnumerable<RewardSelector>> GetSelectorsForActionName(string actionName)
        {
            return await Set
                .Where(_ => _.ActionName == actionName)
                .ToListAsync();
        }
    }
}