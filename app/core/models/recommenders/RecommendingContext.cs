using SignalBox.Core.Recommendations;

#nullable enable
namespace SignalBox.Core.Recommenders
{
    public class RecommendingContext
    {
        public RecommendingContext(InvokationLogEntry invokationLogEntry)
        {
            InvokationLog = invokationLogEntry;
        }

        public RecommendingContext(RecommendationCorrelator correlator, InvokationLogEntry invokationLogEntry)
        {
            Correlator = correlator;
            InvokationLog = invokationLogEntry;
        }


        public TrackedUser? TrackedUser { get; set; }
        public RecommendationCorrelator? Correlator { get; set; }
        public InvokationLogEntry InvokationLog { get; }
    }
}