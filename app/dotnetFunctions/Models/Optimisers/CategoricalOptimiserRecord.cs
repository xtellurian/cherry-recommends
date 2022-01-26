using System.Text.Json.Serialization;
using SignalBox.Core;

namespace SignalBox.Functions
{
    public class CategoricalOptimiserRecord
    {
        [JsonPropertyName("PartitionKey")]
        public string PartitionKey { get; set; }
        [JsonPropertyName("RowKey")]
        public string RowKey { get; set; }
        public string Tenant { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int NItemsToRecommend { get; set; }
        public int NPopulations { get; set; }
        public RecommendableItem DefaultItem { get; set; }
        public string DefaultItemCommonId { get; set; }

    }
}