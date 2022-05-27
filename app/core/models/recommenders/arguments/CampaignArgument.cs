namespace SignalBox.Core.Campaigns
{
    public class CampaignArgument : Entity
    {
        protected CampaignArgument()
        { }

#nullable enable
        public CampaignArgument(string commonId, ArgumentTypes argumentType, bool isRequired)
        {
            CommonId = commonId;
            ArgumentType = argumentType;
            IsRequired = isRequired;
        }

        public OldRecommenderArgument ToOldArgument()
        {
            return new OldRecommenderArgument
            {
                CommonId = CommonId,
                ArgumentType = ArgumentType,
                IsRequired = IsRequired
            };
        }

        public long CampaignId { get; set; }
        public CampaignEntityBase? Campaign { get; set; }
        public string CommonId { get; set; } // use commonId for external API consistency
        public ArgumentTypes ArgumentType { get; set; }
        public bool IsRequired { get; set; } = false;

    }
}