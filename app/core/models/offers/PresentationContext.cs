using System.Collections.Generic;

namespace SignalBox.Core
{
    // This class is provided to the scientist when deciding what offer to show
    public class PresentationContext : RecommendationRequestArguments
    {
        public PresentationContext()
        {
        }
#nullable enable
        public PresentationContext(TrackedUser? trackedUser, Experiment experiment, Dictionary<string, object>? features): base(features)
        {
            Experiment = experiment;
            TrackedUser = trackedUser;
        }


        public Experiment? Experiment { get; set; }
        public TrackedUser? TrackedUser { get; set; }
    }
}