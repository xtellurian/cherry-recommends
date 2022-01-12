using System.Collections.Generic;
using static SignalBox.Core.Workflows.CustomerEventsWorkflows;

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

        public static CustomerEventInput ToTrackedUserEventInput(this SegmentModel model, IntegratedSystem sys)
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
            if(string.IsNullOrEmpty(model.UserId))
            {
                model.Properties.TryAdd("anonymousId", model.AnonymousId);
            }

            return new CustomerEventInput(
                model.UserId ?? Customer.AnonymousCommonId, 
                eventId: model.MessageId,
                timestamp: model.Timestamp,
                environmentId: sys.EnvironmentId, 
                recommendationCorrelatorId: correlatorId, 
                sourceSystemId: sys.Id, 
                MapEventKind(model), 
                model.Event ?? "Segment|Unknown", 
                model.Properties);

        }

        public static EventKinds MapEventKind(SegmentModel model)
        {
            return model.Type switch
            {
                "track" => EventKinds.Behaviour,
                "page" => EventKinds.Behaviour,
                _ => EventKinds.Custom,
            };
        }
    }
}
