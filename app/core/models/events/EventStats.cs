using System;

namespace SignalBox.Core
{
    public class EventStats
    {
        public EventStats(int instances, double fractionOfKind, int trackedUsers)
        {
            Instances = instances;
            FractionOfKind = Math.Round(fractionOfKind, 2);
            TrackedUsers = trackedUsers;
        }

        public int Instances { get; set; }
        public int TrackedUsers { get; set; }
        public double FractionOfKind { get; set; }

    }
}