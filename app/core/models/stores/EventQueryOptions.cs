using System;
using System.Linq.Expressions;

namespace SignalBox.Core
{
#nullable enable
    public class EventQueryOptions
    {
        public Expression<Func<TrackedUserEvent, bool>>? Filter { get; set; }
        public bool NoTracking { get; set; } = true;
    }
}