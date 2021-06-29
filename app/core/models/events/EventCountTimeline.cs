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
            if (Categories.Any())
            {
                CategoricalMoments = new MomentCategoryCount(moments, Categories);
            }
        }

        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<MomentCount> Moments { get; set; }
        public MomentCategoryCount CategoricalMoments { get; set; } = new MomentCategoryCount();
    }
}