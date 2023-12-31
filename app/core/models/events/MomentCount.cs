using System;

namespace SignalBox.Core
{
    public class MomentCount
    {
        public MomentCount(DateTimeOffset timestamp, long count)
        {
            Timestamp = timestamp;
            Count = count;
        }

        public MomentCount(string category, DateTimeOffset timestamp, long count)
        {
            Category = category;
            Timestamp = timestamp;
            Count = count;
        }

        public string Category { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public long UnixTime => Timestamp.ToUnixTimeMilliseconds();
        public long Count { get; set; }
    }
}