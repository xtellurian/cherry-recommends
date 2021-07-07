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
                if (kvp.Value is double f)
                {
                    actions.Add(new TrackedUserAction(e.CommonUserId,
                                                      e.EventId,
                                                      e.Timestamp,
                                                      e.RecommendationCorrelatorId,
                                                      e.Source?.Id,
                                                      category,
                                                      kvp.Key,
                                                      f));
                }
                else if (kvp.Value is int n)
                {
                    actions.Add(new TrackedUserAction(e.CommonUserId,
                                                      e.EventId,
                                                      e.Timestamp,
                                                      e.RecommendationCorrelatorId,
                                                      e.Source?.Id,
                                                      category,
                                                      kvp.Key,
                                                      n));
                }
                else if (kvp.Value is string s)
                {
                    actions.Add(new TrackedUserAction(e.CommonUserId,
                                                      e.EventId,
                                                      e.Timestamp,
                                                      e.RecommendationCorrelatorId,
                                                      e.Source?.Id,
                                                      category,
                                                      kvp.Key,
                                                      s));
                }
                else
                {
                    throw new System.ArgumentException($"{kvp.Value} is an unknown action value type");
                }
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
                if (kvp.Value is double f)
                {
                    actions.Add(new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, "TrackedUser|Properties", kvp.Key, f));
                }
                else if (kvp.Value is int n)
                {
                    actions.Add(new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, "TrackedUser|Properties", kvp.Key, n));
                }
                else if (kvp.Value is string s)
                {
                    actions.Add(new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, "TrackedUser|Properties", kvp.Key, s));
                }
                else if (kvp.Value is System.Text.Json.JsonElement jsonElement)
                {
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                    {
                        actions.Add(new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, "TrackedUser|Properties", kvp.Key, jsonElement.GetString()));
                    }
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                    {
                        if (jsonElement.TryGetInt32(out var i))
                        {
                            actions.Add(new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, "TrackedUser|Properties", kvp.Key, i));
                        }
                        else if (jsonElement.TryGetDouble(out var d))
                        {
                            actions.Add(new TrackedUserAction(commonUserId, eventId, timestamp, null, integratedSystemId, "TrackedUser|Properties", kvp.Key, d));
                        }
                        else
                        {
                            throw new System.ArgumentException($"{kvp.Value} JsonElement of ValueKind {jsonElement.ValueKind} is an unknown action value type");
                        }
                    }
                }
                else
                {
                    throw new System.ArgumentException($"{kvp.Value} of type {kvp.Value.GetType()} is an unknown action value type");
                }
            }
            return actions;
        }
    }
}