using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public class RecommendableItem : CommonEntity
    {
        protected RecommendableItem()
        { }

#nullable enable
        public RecommendableItem(string commonId, string name, double listPrice, double? directCost = null) : base(commonId, name)
        {
            ListPrice = listPrice;
            DirectCost = directCost;
        }

        // required property for a many to many relationship
        [JsonIgnore]
        public ICollection<ItemsRecommender> Recommenders { get; set; } = null!;
        // required property for a many to many relationship
        [JsonIgnore]
        public ICollection<ItemsRecommendation> Recommendations { get; set; } = null!;

        public double ListPrice { get; set; }
        public double? DirectCost { get; set; }
        public string? Description { get; set; }
    }
}