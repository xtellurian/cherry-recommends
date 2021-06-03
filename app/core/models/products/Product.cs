using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class Product : NamedEntity
    {
        public Product()
        { }

        public Product(string name, string productId) : base(name)
        {
            ProductId = productId;
        }

#nullable enable
        public string ProductId { get; set; }
        public string? Description { get; set; }

        [JsonIgnore]
        public Collection<Sku>? Skus { get; set; }
    }
}