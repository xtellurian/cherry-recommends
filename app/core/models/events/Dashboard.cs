using System.Collections.Generic;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public class Dashboard
    {
        public Dashboard(int totalCustomers,
                         IEnumerable<CustomerEvent> events,
                         IEnumerable<RecommendationEntity> recommendations)
        {
            TotalCustomers = totalCustomers;
            this.Events = events;
            this.Recommendations = recommendations;
        }

        public int TotalCustomers { get; private set; }
        public int TotalTrackedUsers => TotalCustomers;
        public IEnumerable<CustomerEvent> Events { get; set; }
        public IEnumerable<RecommendationEntity> Recommendations { get; }
    }
}