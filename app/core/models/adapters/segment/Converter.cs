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

        public static CustomerEventInput ToTrackedUserEventInput(this SegmentModel model, ITenantProvider tenantProvider, IntegratedSystem sys)
        {
            long? correlatorId = ExtractCorrelatorId(model);
            if (string.IsNullOrEmpty(model.UserId))
            {
                model.Properties.TryAdd("anonymousId", model.AnonymousId);
            }

            return new CustomerEventInput
            (
                tenantName: tenantProvider.RequestedTenantName,
                model.UserId ?? Customer.AnonymousCommonId,
                eventId: model.MessageId,
                timestamp: model.Timestamp,
                environmentId: sys.EnvironmentId,
                recommendationCorrelatorId: correlatorId,
                sourceSystemId: sys.Id,
                kind: MapEventKind(model),
                eventType: model.Event ?? $"Segment|{model.Type}" ?? "Segment|Unknown",
                properties: model.Properties ?? model.Traits
            );

        }

        private static long? ExtractCorrelatorId(SegmentModel model)
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
            catch (System.Exception)
            {
                // swallow all exceptions
            }

            return correlatorId;
        }

        public static EventKinds MapEventKind(SegmentModel model)
        {
            return model.Type switch
            {
                "track" => EventKinds.Behaviour,
                "page" => EventKinds.PageView,
                "screen" => EventKinds.PageView,
                "identify" => EventKinds.Identify,
                _ => EventKinds.Custom,
            };
        }
    }
}
