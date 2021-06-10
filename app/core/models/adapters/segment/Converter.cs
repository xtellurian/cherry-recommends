using SignalBox.Core;
using static SignalBox.Core.Workflows.TrackedUserEventsWorkflows;

namespace SignalBox.Core.Adapters.Segment
{
    public static class Converter
    {
        public static TrackedUserEvent ToTrackedUserEvent(this SegmentModel model, IntegratedSystem sys)
        {
            return new TrackedUserEvent(model.UserId, model.MessageId, model.Timestamp, sys, model.Type, model.Event, model.Properties);
        }
    }
}