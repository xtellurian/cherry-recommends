using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public class ExpectedValueScorer : IScorer
    {
        public ExpectedValueScorer()
        { }

        public string Name => "Expected Value";

        public Task<Score> ScoreOffer(Offer offer, IEnumerable<TrackedUser> accepted, IEnumerable<TrackedUser> rejected)
        {
            var countAccepted = accepted.Count();
            var countRejected = rejected.Count();
            var total = countAccepted + countRejected;
            var maxValue = total * offer.Price;
            if (total == 0)
            {
                return Task.FromResult(Score.DefaultScore);
            }
            return Task.FromResult(new Score
            {
                Value = (countAccepted - countRejected) * offer.Price / total
            });
        }
    }
}