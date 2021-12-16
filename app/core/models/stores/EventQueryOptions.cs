using System;
using System.Linq.Expressions;

namespace SignalBox.Core
{
#nullable enable
    public class EventQueryOptions
    {
        public Expression<Func<CustomerEvent, bool>>? Filter { get; set; }
        public bool NoTracking { get; set; } = true;
    }
}