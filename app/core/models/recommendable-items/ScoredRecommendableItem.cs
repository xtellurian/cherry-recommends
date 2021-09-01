namespace SignalBox.Core
{
    public class ScoredRecommendableItem
    {
        protected ScoredRecommendableItem()
        { }
#nullable enable
        public ScoredRecommendableItem(RecommendableItem item, double? score)
        {
            this.Item = item;
            this.ItemId = item?.Id;
            this.ItemCommonId = item?.CommonId;
            this.Score = score;
        }
        public long? ItemId { get; set; }
        public string? ItemCommonId { get; set; }
        public RecommendableItem Item { get; set; }
        public double? Score { get; set; }
    }
}