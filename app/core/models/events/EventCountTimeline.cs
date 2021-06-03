using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core
{
    public class EventCountTimeline
    {
        public EventCountTimeline(IEnumerable<MomentCount> moments)
        {
            Moments = moments.OrderByDescending(_ => _.Timestamp);
            Categories = moments.Select(_ => _.Category).Where(_ => _ != null).Distinct().ToList();
            CategoricalMoments = new MomentCategoryCount(moments);
        }

        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<MomentCount> Moments { get; set; }
        public MomentCategoryCount CategoricalMoments { get; set; }
    }
}