using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core
{
    public static class TrackedUserActionExtensions
    {
        public static ICollection<TrackedUserAction> ToActions(this CustomerEvent e)
        {
            var category = $"Event|{e.Kind}";
            var actions = new List<TrackedUserAction>();
            if (e.Properties == null || e.Properties.Count == 0)
            {
                actions.Add(new TrackedUserAction(e.Customer,
                                                      e,
                                                      e.Timestamp,
                                                      e.RecommendationCorrelatorId,
                                                      e.Source?.Id,
                                                      category,
                                                      e.EventType,
                                                      null));
            }
            foreach (var kvp in e.Properties)
            {
                actions.Add(ToAction(e.Customer,
                                    e,
                                    e.Timestamp,
                                    e.RecommendationCorrelatorId,
                                    e.Source?.Id,
                                    category, kvp));
            }
            return actions;
        }

        public static ICollection<TrackedUserAction> ToActions(this IEnumerable<CustomerEvent> events)
        {
            var actions = new List<TrackedUserAction>();
            foreach (var e in events)
            {
                actions.AddRange(e.ToActions());
            }
            return actions;
        }

        public static IList<TrackedUserAction> ActionsFromChanges(this Customer user,
                                                           IDictionary<string, object> nextProperties,
                                                           DateTimeOffset now,
                                                           long? recommendationCorrelatorId,
                                                           long? integratedSystemId)
        {
            var result = new List<TrackedUserAction>();
            user.Properties ??= new DynamicPropertyDictionary();
            if ((user.Properties.Count == 0) && (nextProperties == null || nextProperties.Count == 0))
            {
                // current and next are both empty or null
                return result;
            }
            else
            {
                foreach (var k in user.Properties.Keys.Where(_ => nextProperties.ContainsKey(_)))
                {
                    // existing properties, keys in both
                    // then the value may be updated or the same
                    if (!string.Equals(user.Properties[k]?.ToString(), nextProperties[k]?.ToString()))
                    {
                        // they are NOT the same

                        result.Add(new TrackedUserAction(user, null, timestamp: now, recommendationCorrelatorId, integratedSystemId, "System|PropertyUpdated", k, nextProperties[k]?.ToString()));
                    }
                }
                foreach (var k in user.Properties.Keys.Where(_ => !nextProperties.ContainsKey(_)))
                {
                    // deleted properties
                    result.Add(new TrackedUserAction(user, null, timestamp: now, recommendationCorrelatorId, integratedSystemId, "System|PropertyDeleted", k, null));
                }
                foreach (var k in nextProperties.Keys.Where(_ => !user.Properties.ContainsKey(_)))
                {
                    // new properties

                    result.Add(new TrackedUserAction(user, null, timestamp: now, recommendationCorrelatorId, integratedSystemId, "System|PropertyCreated", k, nextProperties[k]?.ToString()));
                }
            }
            return result;
        }

        private static TrackedUserAction ToAction(Customer customer,
                                     CustomerEvent trackedUserEvent,
                                     DateTimeOffset timestamp,
                                     long? recommendationCorrelatorId,
                                     long? integratedSystemId,
                                     string category,
                                     KeyValuePair<string, object> kvp)
        {
            // handle internal
            if (kvp.Key.StartsWith(CustomerEvent.FOUR2_INTERNAL_PREFIX))
            {
                // is internal
                if (kvp.Key == CustomerEvent.FEEDBACK && kvp.Value is int n)
                {
                    var a = new TrackedUserAction(customer, trackedUserEvent, timestamp, trackedUserEvent.RecommendationCorrelatorId, integratedSystemId, category, kvp.Key, n);
                    a.FeedbackScore = n;
                    return a;
                }
            }

            if (kvp.Value == null)
            {
                return new TrackedUserAction(customer, trackedUserEvent, timestamp, trackedUserEvent.RecommendationCorrelatorId, integratedSystemId, category, kvp.Key, (string)null);
            }
            else if (kvp.Value is double f)
            {
                return new TrackedUserAction(customer, trackedUserEvent, timestamp, trackedUserEvent.RecommendationCorrelatorId, integratedSystemId, category, kvp.Key, f);
            }
            else if (kvp.Value is int n)
            {
                return new TrackedUserAction(customer, trackedUserEvent, timestamp, trackedUserEvent.RecommendationCorrelatorId, integratedSystemId, category, kvp.Key, n);
            }
            else if (kvp.Value is string s)
            {
                return new TrackedUserAction(customer, trackedUserEvent, timestamp, trackedUserEvent.RecommendationCorrelatorId, integratedSystemId, category, kvp.Key, s);
            }
            else if (kvp.Value is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    return new TrackedUserAction(customer, trackedUserEvent, timestamp, trackedUserEvent.RecommendationCorrelatorId, integratedSystemId, category, kvp.Key, jsonElement.GetString());
                }
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    if (jsonElement.TryGetInt32(out var i))
                    {
                        return new TrackedUserAction(customer, trackedUserEvent, timestamp, trackedUserEvent.RecommendationCorrelatorId, integratedSystemId, category, kvp.Key, i);
                    }
                    else if (jsonElement.TryGetDouble(out var d))
                    {
                        return new TrackedUserAction(customer, trackedUserEvent, timestamp, trackedUserEvent.RecommendationCorrelatorId, integratedSystemId, category, kvp.Key, d);
                    }
                    else
                    {
                        throw new System.ArgumentException($"{kvp.Value} JsonElement of ValueKind {jsonElement.ValueKind} is an unknown action value type");
                    }
                }
                else
                {
                    return new TrackedUserAction(customer, trackedUserEvent, timestamp, trackedUserEvent.RecommendationCorrelatorId, integratedSystemId, category, kvp.Key, $"{kvp.Value}");
                }
            }
            else
            {
                throw new System.ArgumentException($"{kvp.Value} of type {kvp.Value.GetType()} is an unknown action value type");
            }
        }
    }
}