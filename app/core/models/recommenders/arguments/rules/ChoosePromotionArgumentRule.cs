namespace SignalBox.Core.Campaigns
{
    public class ChoosePromotionArgumentRule : ArgumentRule
    {
        protected ChoosePromotionArgumentRule() { }
        public ChoosePromotionArgumentRule(CampaignEntityBase campaign,
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

        public override void Validate()
        {
            base.Validate();
            // check if numeric value can be parsed
            if (Argument.ArgumentType == ArgumentTypes.Numerical)
            {
                if (!double.TryParse(ArgumentValue, out var _))
                {
                    // not a numeric value
                    throw new BadRequestException($"The value {ArgumentValue} cannot match numerical arguments as it in not numeric");
                }
            }
        }
    }
}