using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFRewardSelectorStore : EFEntityStoreBase<RewardSelector>, IRewardSelectorStore
    {
        public EFRewardSelectorStore(SignalBoxDbContext context)
        : base(context, (c) => c.RewardSelectors)
        {
        }

        public async Task<IEnumerable<RewardSelector>> GetSelectorsForActionName(string actionName)
        {
            return await Set
                .Where(_ => _.ActionName == actionName)
                .ToListAsync();
        }
    }
}