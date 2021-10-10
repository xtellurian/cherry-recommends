using System.Collections.Generic;
using static SignalBox.Core.Workflows.TrackedUserEventsWorkflows;

namespace SignalBox.Core.Adapters.Segment
{
    public static class Converter
    {
        private static IList<string> PossibleCorrelatorIdKeys => new List<string>
        {
            "correlatorId",
            "CorrelatorId",
            "recommendationCorrelatorId",
            "RecommendationCorrelatorId",
        };

        public static TrackedUserEventInput ToTrackedUserEventInput(this SegmentModel model, IntegratedSystem sys)
        {
            long? correlatorId = null;
            try
            {
                if (model.Properties != null)
                {
                    var p = model.Properties;
                    foreach (var k in PossibleCorrelatorIdKeys)
                    {
                        if (p.TryGetValue(k, out var v))
                        {
                            if (long.TryParse(v?.ToString(), out var correlator))
                            {
                                correlatorId = correlator;
                            }
                        }
                    }
                }
            }
            catch(System.Exception)
            {
                // swallow this exception
            }

            return new TrackedUserEventInput(
                model.UserId, model.MessageId, model.Timestamp, correlatorId, sys.Id, MapEventKind(model), model.Event ?? "Segment|Unknown", model.Properties);

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
