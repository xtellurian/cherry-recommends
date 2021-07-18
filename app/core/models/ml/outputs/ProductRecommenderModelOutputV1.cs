using System.Text.Json.Serialization;

namespace SignalBox.Core
{
#nullable enable
    public class ProductRecommenderModelOutputV1 : IModelOutput
    {
        public ProductRecommenderModelOutputV1()
        { }

        public long? ProductId { get; set; }
        public string? ProductCommonId { get; set; }
        public Product? Product { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? CorrelatorId { get; set; }
    }
}