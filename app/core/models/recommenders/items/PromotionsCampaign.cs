using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Campaigns
{
    public class PromotionsCampaign : CampaignEntityBase, ICampaign
    {
        protected PromotionsCampaign()
        { }

#nullable enable
        public PromotionsCampaign(string commonId,
                                string? name,
                                RecommendableItem? baselineItem,
                                ICollection<RecommendableItem> promotions,
                                IEnumerable<CampaignArgument>? arguments,
                                CampaignSettings? settings,
                                Metric? targetMetric,
                                int numberOfItemsToRecommend = 1) : base(commonId, name, arguments, settings)
        {
            Items = promotions ?? throw new System.ArgumentNullException(nameof(promotions));
            BaselineItem = baselineItem;
            BaselineItemId = baselineItem?.Id;
            TargetMetric = targetMetric;
            TargetMetricId = targetMetric?.Id;
            NumberOfItemsToRecommend = numberOfItemsToRecommend;

            if (numberOfItemsToRecommend > 9)
            {
                throw new BadRequestException("Maximum number of promotions to recommend cannot be greater than 9");
            }
            if (Items.Any() && numberOfItemsToRecommend > Items.Count)
            {
                throw new BadRequestException($"Maximum number of promotions cannot be greater than the number of promotions provided ({Items.Count})");
            }
        }

        public long? BaselineItemId { get; set; }
        public RecommendableItem? BaselineItem { get; set; }
        public RecommendableItem? DefaultItem => BaselineItem;
        public int? NumberOfItemsToRecommend { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<RecommendableItem> Items { get; set; }
        [JsonIgnore]
        public ICollection<ItemsRecommendation> Recommendations { get; set; } = null!;
        public long? TargetMetricId { get; set; }
        public Metric? TargetMetric { get; set; }
        public PromotionCampaignTargetTypes TargetType { get; set; }
        public PromotionOptimiser? Optimiser { get; set; }
        public bool UseOptimiser { get; set; }
    }
}