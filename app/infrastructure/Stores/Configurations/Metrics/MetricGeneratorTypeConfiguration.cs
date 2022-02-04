using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class MetricGeneratorTypeConfiguration : EntityTypeConfigurationBase<MetricGenerator>, IEntityTypeConfiguration<MetricGenerator>
    {
        public override void Configure(EntityTypeBuilder<MetricGenerator> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.GeneratorType).HasConversion<string>();
            builder.Property(_ => _.FilterSelectAggregateSteps).HasJsonConversion();

            builder.Ignore(_ => _.Feature);

            builder.ToTable("FeatureGenerators");

            builder.HasData(MetricGenerator.TotalEventsGenerator);
        }
    }
}