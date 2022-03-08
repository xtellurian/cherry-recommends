using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;

#nullable enable
namespace SignalBox.Core.Recommenders
{
    public class RecommendingContext
    {
        public RecommendingContext(InvokationLogEntry invokationLogEntry, string? trigger)
        {
            InvokationLog = invokationLogEntry;
            this.Trigger = trigger;
        }

        public RecommendingContext(RecommendationCorrelator correlator, InvokationLogEntry invokationLogEntry, string? trigger)
        {
            Correlator = correlator;
            InvokationLog = invokationLogEntry;
            this.Trigger = trigger;
        }

        public void SetLogger(ILogger logger)
        {
            this.logger = logger;
        }
        private ILogger? logger;

        public void LogMessage(string message)
        {
            logger?.LogInformation(message);
            this.InvokationLog?.LogMessage(message);
        }

        public string? Trigger { get; set; }
        public Customer? Customer { get; set; }
        public Business? Business { get; set; }
        public RecommendationCorrelator? Correlator { get; set; }
        public InvokationLogEntry InvokationLog { get; }
    }
}