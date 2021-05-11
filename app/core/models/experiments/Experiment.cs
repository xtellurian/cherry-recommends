using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class Experiment : NamedEntity
    {
        public Experiment()
        {
        }

#nullable enable
        public Experiment(string name, IEnumerable<Offer> offers, int concurrentOffers) : base(name)
        {
            Offers = offers.ToList();
            ConcurrentOffers = concurrentOffers;
            Iterations = new Collection<Iteration>
            {
                Iteration.First
            };
        }

        public Iteration NextIteration()
        {
            var iteration = new Iteration
            {
                Order = Iterations.Max(_ => _.Order) + 1
            };
            Iterations.Add(iteration);
            return iteration;

        }

        [JsonIgnore]
        public ICollection<Offer> Offers { get; set; }
        public int ConcurrentOffers { get; set; } = 1; // defaults to one
        public ICollection<Iteration> Iterations { get; set; }

        public Iteration CurrentIteration => Iterations.First(i => i.Order == Iterations.Max(_ => _.Order));

    }
}