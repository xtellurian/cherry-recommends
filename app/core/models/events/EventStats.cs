using System;

namespace SignalBox.Core
{
    public class EventStats
    {
        public EventStats(int instances, double fractionOfKind, int customers)
        {
            Instances = instances;
            FractionOfKind = Math.Round(fractionOfKind, 2);
            Customers = customers;
        }

        public int Instances { get; set; }
        public int Customers { get; set; }
        public int TrackedUsers => Customers;
        public double FractionOfKind { get; set; }

    }
}