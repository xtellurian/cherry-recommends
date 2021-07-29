using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Recommenders
{
    public class RecommendingContext
    {
        public RecommendingContext(string version, RecommendationCorrelator correlator)
        {
            Version = version;
            Correlator = correlator;
        }

        public string Version { get; set; }
        public RecommendationCorrelator Correlator { get; set; }
    }
}