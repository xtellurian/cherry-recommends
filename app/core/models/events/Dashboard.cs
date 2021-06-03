using System.Collections.Generic;

namespace SignalBox.Core
{
    public class Dashboard
    {
        public Dashboard(IEnumerable<MomentCount> moments)
        {
            EventTimeline = new EventCountTimeline(moments);
        }

        public Dashboard()
        { }

        public EventCountTimeline EventTimeline { get; set; }
    }
}