using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IScorer
    {
        string Name { get; }
        Task<OfferScore> ScoreOffer(Offer offer, IEnumerable<TrackedUser> accepted, IEnumerable<TrackedUser> rejected);
    }
}