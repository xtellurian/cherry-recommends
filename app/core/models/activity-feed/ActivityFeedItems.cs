using System.Collections.Generic;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public class ActivityFeedEntity
    {
        public ActivityKinds ActivityKind { get; set; }
        public Paginated<object> ActivityItems { get; set; }
    }
}