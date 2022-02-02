using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ItemsRecommenderPerformanceReportTypeConfiguration : 
        RecommenderPerformanceReportTypeConfigurationBase<ItemsRecommenderPerformanceReport>, IEntityTypeConfiguration<ItemsRecommenderPerformanceReport>
    {
        public override void Configure(EntityTypeBuilder<ItemsRecommenderPerformanceReport> builder)
        {
            base.Configure(builder);

            builder.Property(_ => _.PerformanceByItem).HasJsonConversion();
            // builder.Ignore(_ => _.PerformanceByItem);

            builder.Ignore(_ => _.ItemsByCommonId);
            builder.Ignore(_ => _.ItemsById);
            builder.Ignore(_ => _.ItemsRecommender);
        }
    }
}