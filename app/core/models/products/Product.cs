using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public class Product : RecommendableItem
    {
        protected Product()
        { }

        public Product(string commonId, string name, double listPrice, double? directCost = null)
         : base(commonId, name, listPrice, directCost)
        { }

        // required property for a many to many relationship
        [JsonIgnore]
        public ICollection<ProductRecommender> ProductRecommenders { get; set; }
    }
}