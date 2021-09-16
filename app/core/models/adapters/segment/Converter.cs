using static SignalBox.Core.Workflows.TrackedUserEventsWorkflows;

namespace SignalBox.Core.Adapters.Segment
{
    public static class Converter
    {
        public static TrackedUserEventInput ToTrackedUserEventInput(this SegmentModel model, IntegratedSystem sys)
        {
            return new TrackedUserEventInput(
                model.UserId, model.MessageId, model.Timestamp, null, sys.Id, MapEventKind(model), model.Event ?? "Segment|Unknown", model.Properties);
        }

        public static EventKinds MapEventKind(SegmentModel model)
        {
            switch (model.Type)
            {
                case "track":
                    return EventKinds.Behaviour;
                case "page":
                    return EventKinds.Behaviour;
                default:
                    return EventKinds.Custom;
            }
        }
    }
}
