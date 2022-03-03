using System;
using System.IO;
using Newtonsoft.Json.Linq;
using SignalBox.Core.Adapters.Segment;

namespace SignalBox.Test
{
    public static class DataLoader
    {
        // https://segment.com/docs/connections/destinations/catalog/webhooks/
        public static SegmentModel LoadSegmentWebhookJson(string key)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "segmentEvents.json");
            var json = File.ReadAllText(filePath);
            var jObj = JObject.Parse(json);
            if (jObj.ContainsKey(key))
            {
                return jObj[key].ToObject<SegmentModel>();
            }
            else
            {
                throw new ArgumentException($"{key} was not found in json");
            }
        }
    }
}