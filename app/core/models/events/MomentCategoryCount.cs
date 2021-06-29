using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core
{
    public class MomentCategoryCount : List<Dictionary<string, object>>
    {
        public MomentCategoryCount()
        { }
        // This should take moments that have already been grouped into buckets of timestamps
        public MomentCategoryCount(IEnumerable<MomentCount> moments, IEnumerable<string> categories)
        {
            var timestamps = moments.Select(_ => _.Timestamp).Distinct().ToList();
            timestamps.Sort();
            foreach (var t in timestamps)
            {
                var momentsInTime = moments.Where(_ => _.Timestamp == t);
                var d = new Dictionary<string, object>();
                foreach (var cat in categories)
                {
                    d[cat] = momentsInTime.Where(_ => _.Category == cat).Sum(_ => _.Count);
                    d["timestamp"] = t; // lowercase to match case client side
                    d["unixTime"] = t.ToUnixTimeMilliseconds();
                }
                this.Add(d);
            }
        }
    }
}