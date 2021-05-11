using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public class RandomScorer : IScorer
    {
        public string Name => "Random";

        public Task<Score> ScoreOffer(Offer offer, IEnumerable<TrackedUser> accepted, IEnumerable<TrackedUser> rejected)
        {
            var value = (new System.Random()).NextDouble();
            return Task.FromResult(new Score
            {
                Value = value
            });
        }
    }
}