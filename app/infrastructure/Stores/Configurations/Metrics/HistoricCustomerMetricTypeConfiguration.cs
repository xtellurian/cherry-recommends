using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class HistoricCustomerMetricTypeConfiguration : EntityTypeConfigurationBase<HistoricCustomerMetric>, IEntityTypeConfiguration<HistoricCustomerMetric>
    {
        public override void Configure(EntityTypeBuilder<HistoricCustomerMetric> builder)
        {
            base.Configure(builder);
            // ensure the uniqueness of a metric value
            builder.HasIndex(f => new { f.MetricId, f.TrackedUserId, f.Version })
                .IsUnique();

            builder.ToTable("HistoricTrackedUserFeatures");

            builder.Ignore(_ => _.TrackedUser);
            builder.Ignore(_ => _.Feature);
            builder.HasOne(_ => _.Customer)
                .WithMany(_ => _.HistoricCustomerMetrics)
                .HasForeignKey(_ => _.TrackedUserId);
        }
    }
}