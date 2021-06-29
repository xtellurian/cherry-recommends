using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public partial class AzureMLModelInput : IModelInput
    {
        public string Version { get; set; } = "default";
        [JsonPropertyName("data")]
        public Datum[] Data { get; set; }
    }

    public class Datum : Dictionary<string, object>
    { }
}