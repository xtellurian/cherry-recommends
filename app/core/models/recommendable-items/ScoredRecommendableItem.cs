using System.Collections.Generic;

namespace SignalBox.Core
{
    public class ScoredRecommendableItem
    {
        protected ScoredRecommendableItem()
        { }
#nullable enable
        public ScoredRecommendableItem(RecommendableItem item, double? score, IEnumerable<DiscountCode>? discountCodes = null)
        {
            this.Item = item;
            this.ItemId = item?.Id;
            this.ItemCommonId = item?.CommonId;
            this.CommonId = item?.CommonId;
            this.Score = score;
            this.DiscountCodes = discountCodes;
        }
        public long? ItemId { get; set; }
        public string? ItemCommonId { get; set; }
        public string? CommonId { get; set; }
        public RecommendableItem Item { get; set; }
        public IEnumerable<DiscountCode>? DiscountCodes { get; set; }
        public double? Score { get; set; }
    }
}