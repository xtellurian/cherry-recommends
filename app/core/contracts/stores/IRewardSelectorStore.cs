using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IRewardSelectorStore : IEntityStore<RewardSelector>
    {
        Task<IEnumerable<RewardSelector>> GetSelectorsForActionName(string actionName);
    }
}