using System.Collections.Generic;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public class Dashboard
    {
        public Dashboard(IEnumerable<TrackedUserEvent> events,
                         IEnumerable<TrackedUserAction> actions,
                         IEnumerable<RecommendationEntity> recommendations)
        {
            this.Actions = actions;
            this.Events = events;
            this.Recommendations = recommendations;
        }


        public IEnumerable<TrackedUserEvent> Events { get; set; }
        public IEnumerable<TrackedUserAction> Actions { get; set; }
        public IEnumerable<RecommendationEntity> Recommendations { get; }
    }
}