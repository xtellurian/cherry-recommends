using System.Collections.Generic;

namespace SignalBox.Core
{
    // This class is provided to the scientist when deciding what offer to show
    public class PresentationContext
    {
        public PresentationContext()
        {
        }
#nullable enable
        public PresentationContext(TrackedUser? trackedUser, Experiment experiment, Dictionary<string, object>? features)
        {
            Experiment = experiment;
            TrackedUser = trackedUser;
            Features = features;
        }


        public Experiment? Experiment { get; set; }
        public TrackedUser? TrackedUser { get; set; }
        public Dictionary<string, object>? Features { get; set; }
    }
}