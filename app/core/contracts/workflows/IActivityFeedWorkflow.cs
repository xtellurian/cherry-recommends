using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IActivityFeedWorkflow
    {
        Task<IEnumerable<ActivityFeedEntity>> GetActivityFeedEntities(IPaginate paginate);
    }
}