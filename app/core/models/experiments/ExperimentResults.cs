using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core
{
    public class ExperimentResults
    {
        public ExperimentResults()
        {
        }

        public ExperimentResults(Experiment experiment,
                                 IEnumerable<Offer> offers,
                                 IEnumerable<OfferStats> offerStats,
                                 double significantEventCount,
                                 double benefit)
        {
            this.Experiment = experiment;
            this.Offers = offers;
            this.OfferStats = offerStats.ToList();
            this.SignificantEventCount = significantEventCount;
            this.Benefit = benefit;
        }

        public Experiment Experiment { get; set; }
        public IEnumerable<Offer> Offers { get; set; }
        public IEnumerable<OfferStats> OfferStats { get; set; } = new List<OfferStats>();
        public double SignificantEventCount { get; set; }
        public double Benefit { get; set; }
    }
}