using System.Collections.Generic;

namespace SignalBox.Core.Recommendations
{
    public class OfferRecommendation : RecommendationEntity
    {
        // used once in a test.
        public OfferRecommendation()
        { }

        public OfferRecommendation(RecommendationCorrelator correlator, IList<Offer> offers, TrackedUser user, Experiment experiment, Dictionary<string, object> features = null)
        : base(correlator, "v0")
        {
            CommonUserId = user.CommonUserId;
            Offers = offers;
            ExperimentId = experiment.Id;
            var current = experiment.CurrentIteration;
            IterationId = current.Id;
            IterationOrder = current.Order;
            Features = features ?? new Dictionary<string, object>();
        }

        public string CommonUserId { get; set; }
        public IList<Offer> Offers { get; set; } // the order matters
        public long ExperimentId { get; set; }
        public string IterationId { get; set; }
        public int IterationOrder { get; set; }
        public Dictionary<string, object> Features { get; set; }
        public long RecommendationId => this.Id; // this gets serialised and returned by clients
    }
}