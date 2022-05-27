using System.Collections.Generic;
namespace SignalBox.Core.Campaigns
{
#nullable enable
    public abstract class PerformanceReportBase : EnvironmentScopedEntity, IHierarchyBase
    {
        protected PerformanceReportBase()
        {
            Recommender = null!;
        }
        protected PerformanceReportBase(CampaignEntityBase recommender)
        {
            RecommenderId = recommender.Id;
            Recommender = recommender;
        }

        public long RecommenderId { get; protected set; }
        public CampaignEntityBase Recommender { get; protected set; }
        public string Discriminator { get; set; } = null!;
    }
}