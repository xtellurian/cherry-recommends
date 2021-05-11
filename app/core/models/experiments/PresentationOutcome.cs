namespace SignalBox.Core
{
    public class PresentationOutcome : Entity
    {
        public PresentationOutcome()
        {
        }

        public PresentationOutcome(Experiment experiment,
                                   OfferRecommendation recommendation,
                                   string iterationId,
                                   int iterationOrder,
                                   Offer offer,
                                   string outcome)
        {
            Experiment = experiment;
            Recommendation = recommendation;
            IterationId = iterationId;
            IterationOrder = iterationOrder;
            Offer = offer;
            Outcome = outcome;
        }

        public Experiment Experiment { get; set; }
        public Offer Offer { get; set; }
        public OfferRecommendation Recommendation { get; set; }
        public string IterationId { get; set; }
        public int IterationOrder { get; set; }
        public string Outcome { get; set; }

    }
}