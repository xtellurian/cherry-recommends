using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
#nullable enable
    public partial class AzureMLModelInput : IModelInput
    {
        public string Version { get; set; } = "default";
        [JsonPropertyName("data")]
        public Datum[] Data { get; set; } = null!;
        public string? CommonUserId { get; set; }
        public IDictionary<string, object>? Arguments { get; set; }
        public IDictionary<string, object>? Features { get; set; }
    }

    public class Datum : Dictionary<string, object>
    { }
}