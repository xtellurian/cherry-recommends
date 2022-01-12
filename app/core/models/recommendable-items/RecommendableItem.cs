using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public class RecommendableItem : CommonEntity
    {
        private const string defaultRecommendableItemCommonId = "default";
        private const long defaultRecommendableItemId = -1;
        public static RecommendableItem DefaultRecommendableItem => new RecommendableItem(defaultRecommendableItemCommonId, "Default Item")
        {
            Id = defaultRecommendableItemId,
            Description = "The default recommendable item. When this item is recommended, no action should be taken.",
            Discriminator = "RecommendableItem"
        };
        protected RecommendableItem()
        { }

#nullable enable
        protected RecommendableItem(string commonId, string name) : base(commonId, name)
        { }

        public RecommendableItem(string commonId, string name, double? listPrice, double? directCost, DynamicPropertyDictionary? properties)
        : base(commonId, name, properties)
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

        public double? ListPrice { get; set; }
        public double? DirectCost { get; set; }
        public string? Description { get; set; }
        public string? Discriminator { get; set; }
    }
}