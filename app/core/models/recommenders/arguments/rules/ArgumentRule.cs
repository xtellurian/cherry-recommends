namespace SignalBox.Core.Campaigns
{
    public abstract class ArgumentRule : Entity, IHierarchyBase
    {
        protected ArgumentRule() { }
        protected ArgumentRule(CampaignEntityBase campaign, CampaignArgument argument)
        {
            Argument = argument;
            ArgumentId = argument.Id;
            CampaignId = campaign.Id;
            Campaign = campaign;
        }
        public string Discriminator { get; set; }

        public long CampaignId { get; set; }
        public CampaignEntityBase Campaign { get; set; }
        public long ArgumentId { get; set; }
        public CampaignArgument Argument { get; set; }
    }
}