using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class RecommenderPerformanceReportTypeConfigurationBase<T>
        : EnvironmentScopedEntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : PerformanceReportBase
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder
                .HasOne(_ => _.Recommender)
                .WithMany()
                .HasForeignKey(_ => _.RecommenderId);
        }
    }
}