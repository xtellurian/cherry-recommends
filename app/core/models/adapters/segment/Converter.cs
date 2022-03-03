using System;
using System.Collections.Generic;

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

        public static CustomerEventInput ToCustomerEventInput(this SegmentModel model, ITenantProvider tenantProvider, IntegratedSystem sys)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            long? correlatorId = ExtractCorrelatorId(model);
            if (string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.AnonymousId) && model.Type != "group")
            {
                model.Properties ??= new Dictionary<string, object>();
                model.Properties.TryAdd("anonymousId", model.AnonymousId);
            }
            // because we want the business ID in the properties
            if (!string.IsNullOrEmpty(model.GroupId))
            {
                model.Traits ??= new Dictionary<string, object>();
                model.Traits.TryAdd("businessId", model.GroupId);
            }

            return new CustomerEventInput
            (
                tenantName: tenantProvider.RequestedTenantName,
                customerId: model.UserId ?? Customer.AnonymousCommonId,
                businessCommonId: model.GroupId,
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
                SegmentModelTypes.Track => EventKinds.Behaviour,
                SegmentModelTypes.Page => EventKinds.PageView,
                SegmentModelTypes.Screen => EventKinds.PageView,
                SegmentModelTypes.Identify => EventKinds.Identify,
                SegmentModelTypes.Group => EventKinds.AddToBusiness,
                _ => EventKinds.Custom,
            };
        }
    }
}
