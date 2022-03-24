using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core
{
    public class WeightedRandomSelector<T> where T : IWeighted
    {
        public WeightedRandomSelector(IEnumerable<T> items)
        {
            Items = items;
            TotalWeight = items.Sum(i => i.Weight);
        }

        public IEnumerable<T> Items { get; }
        public double TotalWeight { get; }

        private readonly Random random = new Random();

        public T Choose()
        {
            double rnd = random.NextDouble() * TotalWeight; // just incase we aren't normalised.
            return Items.First(i => (rnd -= i.Weight) < 0);
        }
    }
}