using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Metrics;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class BusinessMetricValueTypeConfiguration : MetricValueBaseTypeConfiguration<BusinessMetricValue>, IEntityTypeConfiguration<BusinessMetricValue>
    {
        public override void Configure(EntityTypeBuilder<BusinessMetricValue> builder)
        {
            base.Configure(builder);

            builder.HasIndex(f => new { f.MetricId, f.BusinessId, f.Version })
                .HasFilter($"[{nameof(BusinessMetricValue.BusinessId)}] IS NOT NULL")
                .IsUnique();
        }
    }
}