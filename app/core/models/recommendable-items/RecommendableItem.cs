using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
    public class RecommendableItem : CommonEntity
    {
        private const string defaultRecommendableItemCommonId = "default";
        private const long defaultRecommendableItemId = -1;
        public static RecommendableItem DefaultRecommendableItem => new RecommendableItem(defaultRecommendableItemCommonId, "Default Promotion")
        {
            Id = defaultRecommendableItemId,
            Description = "The default promotion. When this promotion is recommended, no action should be taken.",
            Discriminator = "RecommendableItem"
        };
        protected RecommendableItem()
        { }

#nullable enable
        protected RecommendableItem(string commonId, string name) : base(commonId, name)
        { }

        public RecommendableItem(string commonId, string name, double? directCost,
            int numberOfRedemptions, BenefitType benefitType, double benefitValue, PromotionType promotionType, DynamicPropertyDictionary? properties)
        : base(commonId, name, properties)
        {
            DirectCost = directCost;
            NumberOfRedemptions = numberOfRedemptions;
            BenefitType = benefitType;
            BenefitValue = benefitValue;
            PromotionType = promotionType;
        }

        // required property for a many to many relationship
        [JsonIgnore]
        public ICollection<PromotionsCampaign> Recommenders { get; set; } = null!;
        // required property for a many to many relationship
        [JsonIgnore]
        public ICollection<ItemsRecommendation> Recommendations { get; set; } = null!;

        public double? DirectCost { get; set; }
        public string? Description { get; set; }
        public BenefitType BenefitType { get; set; }
        public double BenefitValue { get; set; }
        public PromotionType PromotionType { get; set; }
        public int NumberOfRedemptions { get; set; }
        public string? Discriminator { get; set; }
    }
}