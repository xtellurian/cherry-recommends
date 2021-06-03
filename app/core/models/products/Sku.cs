using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class Sku : NamedEntity
    {
        public Sku()
        { }

        public Sku(string name, string skuId, double price, string description = null) : base(name)
        {
            SkuId = skuId;
            Price = price;
            Description = description;
        }

        [JsonIgnore]
        public Product Product { get; set; }
        public string SkuId { get; set; }
        public double Price { get; set; }
#nullable enable
        public string? Description { get; set; }
    }
}