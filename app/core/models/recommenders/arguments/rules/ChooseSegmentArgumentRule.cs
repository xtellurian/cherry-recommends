namespace SignalBox.Core.Campaigns
{
    public class ChooseSegmentArgumentRule : ArgumentRule
    {
        protected ChooseSegmentArgumentRule() { }
        public ChooseSegmentArgumentRule(CampaignEntityBase campaign,
                                           CampaignArgument argument,
                                           Segment segment,
                                           string argumentValue) : base(campaign, argument)
        {
            SegmentId = segment.Id;
            Segment = segment;
            ArgumentValue = argumentValue;
        }

        // Value to match against the argument's value
        public string ArgumentValue { get; set; }
        public long SegmentId { get; set; }

        /// <summary>
        /// Rule will put the Customer into this segment.
        /// </summary>
        public Segment Segment { get; set; }

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