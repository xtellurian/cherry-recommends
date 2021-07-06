using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public class Product : CommonEntity
    {
        protected Product()
        { }

        public Product(string commonId, string name, double listPrice, double? directCost = null) : base(commonId, name)
        {
            ListPrice = listPrice;
            DirectCost = directCost;
        }

        // required property for a many to many relationship
        [JsonIgnore]
        public ICollection<ProductRecommender> ProductRecommenders { get; set; }

        public double ListPrice { get; set; }
        public double? DirectCost { get; set; }
#nullable enable
        public string? Description { get; set; }
    }
}