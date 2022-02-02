using System.Collections.Generic;
namespace SignalBox.Core.Recommenders
{
#nullable enable
    public abstract class PerformanceReportBase : EnvironmentScopedEntity, IHierarchyBase
    {
        protected PerformanceReportBase()
        {
            Recommender = null!;
        }
        protected PerformanceReportBase(RecommenderEntityBase recommender)
        {
            RecommenderId = recommender.Id;
            Recommender = recommender;
        }

        public long RecommenderId { get; protected set; }
        public RecommenderEntityBase Recommender { get; protected set; }
        public string Discriminator { get; set; } = null!;
    }
}