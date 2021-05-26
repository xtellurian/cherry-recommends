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

        public Task<OfferScore> ScoreOffer(Offer offer, IEnumerable<TrackedUser> accepted, IEnumerable<TrackedUser> rejected)
        {
            var countAccepted = accepted.Count();
            var countRejected = rejected.Count();
            var total = countAccepted + countRejected;
            var maxValue = total * offer.Price;
            if (total == 0)
            {
                return Task.FromResult(OfferScore.DefaultScore);
            }
            return Task.FromResult(new OfferScore
            {
                Value = (countAccepted - countRejected) * offer.Price / total
            });
        }
    }
}