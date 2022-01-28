using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
#nullable enable
    public partial class AzureMLModelInput : IModelInput
    {
        public string Version { get; set; } = "default";
        [JsonPropertyName("data")]
        public Datum[] Data { get; set; } = null!;
        public string? CustomerId { get; set; }
        public string? CommonUserId { get; set; }
        public IDictionary<string, object>? Arguments { get; set; }
        public IDictionary<string, object>? Metrics { get; set; }
        public IDictionary<string, object>? Features
        {
            get => Metrics; set
            {
                if (value != null)
                {
                    Metrics = value;
                }
            }
        }
        public IEnumerable<ParameterBounds>? ParameterBounds { get; set; }

        public string GetCustomerId()
        {
            return CustomerId ?? CommonUserId ?? throw new BadRequestException("Customer ID and CommonUserId were null");
        }
    }

    public class Datum : Dictionary<string, object>
    { }
}