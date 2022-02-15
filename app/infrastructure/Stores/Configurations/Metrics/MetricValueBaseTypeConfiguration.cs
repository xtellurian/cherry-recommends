using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Metrics;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class MetricValueBaseTypeConfiguration<T> : EntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : MetricValueBase
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.ToTable("HistoricTrackedUserFeatures");

            builder.Ignore(_ => _.Feature);
        }
    }

    internal class ActualMetricValueBaseTypeConfiguration : MetricValueBaseTypeConfiguration<MetricValueBase>, IEntityTypeConfiguration<MetricValueBase>
    {
        public override void Configure(EntityTypeBuilder<MetricValueBase> builder)
        {
            base.Configure(builder);
        }
    }
}
