namespace SignalBox.Core.Campaigns
{
#nullable enable
    public class CampaignErrorHandling : CampaignSettings
    {
        public CampaignErrorHandling() { }
        public CampaignErrorHandling(CampaignSettings? other)
        {
            this.ThrowOnBadInput = other?.ThrowOnBadInput;
            this.RequireConsumptionEvent = other?.RequireConsumptionEvent;
        }
    }
}