using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class HistoricCustomerMetricTypeConfiguration : MetricValueBaseTypeConfiguration<HistoricCustomerMetric>, IEntityTypeConfiguration<HistoricCustomerMetric>
    {
        public override void Configure(EntityTypeBuilder<HistoricCustomerMetric> builder)
        {
            base.Configure(builder);
            // ensure the uniqueness of a metric value
            builder.HasIndex(f => new { f.MetricId, f.TrackedUserId, f.Version })
                .IsUnique();

            builder.Ignore(_ => _.TrackedUser);

            builder.Property(_ => _.Discriminator).HasDefaultValue("HistoricCustomerMetric"); // compat

            builder.HasOne(_ => _.Metric).WithMany(_ => _.HistoricTrackedUserFeatures).HasForeignKey(_ => _.MetricId);

            builder.HasOne(_ => _.Customer)
                .WithMany(_ => _.HistoricCustomerMetrics)
                .HasForeignKey(_ => _.TrackedUserId);
        }
    }
}