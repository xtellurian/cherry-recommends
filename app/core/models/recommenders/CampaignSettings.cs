using System;

namespace SignalBox.Core.Campaigns
{
    public class CampaignSettings
    {
        public bool Enabled { get; set; } = true; // default to enabled
        public bool? ThrowOnBadInput { get; set; }
        public bool? RequireConsumptionEvent { get; set; }
        public System.TimeSpan? RecommendationCacheTime { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
    }
}