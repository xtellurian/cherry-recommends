using System;
using System.Collections.Generic;

namespace SignalBox.Core
{
    public static class TrackedUserActionExtensions
    {
        public static ICollection<TrackedUserAction> ToActions(this TrackedUserEvent e)
        {
            var category = $"{e.Kind}|{e.EventType}";
            var actions = new List<TrackedUserAction>();
            if (e.Properties == null || e.Properties.Count == 0)
            {
                actions.Add(new TrackedUserAction(e.CommonUserId,
                                                      e.EventId,
                                                      e.Timestamp,
                                                      e.RecommendationCorrelatorId,
                                                      e.Source?.Id,
                                                      category,
                                                      e.EventType,
                                                      null));
            }
            foreach (var kvp in e.Properties)
            {
                actions.Add(ToAction(e.CommonUserId,
                                    e.EventId,
                                    e.Timestamp,
                                    e.RecommendationCorrelatorId,
                                    e.Source?.Id,
                                    category, kvp));
            }
            return actions;
        }

        public static ICollection<TrackedUserAction> ToActions(this IEnumerable<TrackedUserEvent> events)
        {
            var actions = new List<TrackedUserAction>();
            foreach (var e in events)
            {
                actions.AddRange(e.ToActions());
            }
            return actions;
        }

        public static ICollection<TrackedUserAction> ToActions(this DynamicPropertyDictionary properties,
                                                               string commonUserId,
                                                               DateTimeOffset timestamp,
                                                               long? integratedSystemId)
        {
            var eventId = System.Guid.NewGuid().ToString();
            var actions = new List<TrackedUserAction>();
            foreach (var kvp in properties)
            {
                actions.Add(ToAction(commonUserId, eventId, timestamp, null, integratedSystemId, "TrackedUser|Properties", kvp));
            }
            return actions;
        }

        private static TrackedUserAction ToAction(string commonUserId,
                                     string eventId,
                                     DateTimeOffset timestamp,
                                     long? recommendationCorrelatorId,
                                     long? integratedSystemId,
                                     string category,
                                     KeyValuePair<string, object> kvp)
        {
            if (kvp.Value == null)
            {
                return new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, category, kvp.Key, (string)null);
            }
            else if (kvp.Value is double f)
            {
                return new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, category, kvp.Key, f);
            }
            else if (kvp.Value is int n)
            {
                return new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, category, kvp.Key, n);
            }
            else if (kvp.Value is string s)
            {
                return new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, category, kvp.Key, s);
            }
            else if (kvp.Value is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    return new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, category, kvp.Key, jsonElement.GetString());
                }
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    if (jsonElement.TryGetInt32(out var i))
                    {
                        return new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, category, kvp.Key, i);
                    }
                    else if (jsonElement.TryGetDouble(out var d))
                    {
                        return new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, category, kvp.Key, d);
                    }
                    else
                    {
                        throw new System.ArgumentException($"{kvp.Value} JsonElement of ValueKind {jsonElement.ValueKind} is an unknown action value type");
                    }
                }
                else
                {
                    return new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, category, kvp.Key, $"{kvp.Value}");
                }
            }
            else
            {
                throw new System.ArgumentException($"{kvp.Value} of type {kvp.Value.GetType()} is an unknown action value type");
            }
        }
    }
}