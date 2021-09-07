using static SignalBox.Core.Workflows.TrackedUserEventsWorkflows;

namespace SignalBox.Core.Adapters.Segment
{
    public static class Converter
    {
        public static TrackedUserEvent ToTrackedUserEvent(this SegmentModel model, TrackedUser trackedUser, IntegratedSystem sys)
        {
            return new TrackedUserEvent(
                trackedUser, model.MessageId, model.Timestamp, sys, $"Segment|{model.Type}", model.Event, model.Properties);
        }

        public static TrackedUserEventInput ToTrackedUserEventInput(this SegmentModel model, IntegratedSystem sys)
        {
            return new TrackedUserEventInput(
                model.UserId, model.MessageId, model.Timestamp, null, sys.Id, $"Segment|{model.Type}", model.Event ?? "Unknown", model.Properties);
        }
    }
}
