using System.Collections.Generic;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public class GeneralSummary
    {
        public GeneralSummary(int totalCustomers,
                              int eventCount24Hour,
                              double recommendationCount24Hour)
        {
            TotalCustomers = totalCustomers;
            EventCount24Hour = eventCount24Hour;
            RecommendationCount24Hour = recommendationCount24Hour;
        }

        public int TotalCustomers { get; private set; }
        public int EventCount24Hour { get; private set; }
        public double RecommendationCount24Hour { get; private set; }
    }
}