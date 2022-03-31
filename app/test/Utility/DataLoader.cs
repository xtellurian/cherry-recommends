using System;
using System.IO;
using Newtonsoft.Json.Linq;
using SignalBox.Core.Adapters.Segment;
using SignalBox.Core.Adapters.Shopify;

namespace SignalBox.Test
{
    public static class DataLoader
    {
        // https://segment.com/docs/connections/destinations/catalog/webhooks/
        // https://shopify.dev/api/admin-rest/2022-01/resources/webhook#top
        public static T LoadFromJsonData<T>(string fileName, string key)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", fileName);
            var json = File.ReadAllText(filePath);
            var jObj = JObject.Parse(json);
            if (jObj.ContainsKey(key))
            {
                return jObj[key].ToObject<T>();
            }
            else
            {
                throw new ArgumentException($"{key} was not found in json");
            }
        }
    }
}