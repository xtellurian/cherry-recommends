namespace SignalBox.Core.Recommenders
{
    public class ChoosePromotionArgumentRule : ArgumentRule
    {
        protected ChoosePromotionArgumentRule() { }
        public ChoosePromotionArgumentRule(RecommenderEntityBase campaign,
                                           CampaignArgument argument,
                                           RecommendableItem promotion,
                                           string argumentValue) : base(campaign, argument)
        {
            PromotionId = promotion.Id;
            Promotion = promotion;
            ArgumentValue = argumentValue;
        }

        // Value to match against the argument's value
        public string ArgumentValue { get; set; }
        public long PromotionId { get; set; }
        /// <summary>
        /// Rule will force the campaign to return this promotion.
        /// </summary>
        public RecommendableItem Promotion { get; set; }
    }
}