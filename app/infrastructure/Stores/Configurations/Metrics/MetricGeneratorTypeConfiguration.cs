using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Metrics;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class MetricGeneratorTypeConfiguration : EntityTypeConfigurationBase<MetricGenerator>, IEntityTypeConfiguration<MetricGenerator>
    {
        public override void Configure(EntityTypeBuilder<MetricGenerator> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.GeneratorType).HasConversion<string>();
            builder.Property(_ => _.FilterSelectAggregateSteps).HasJsonConversion();
            builder.Property(_ => _.TimeWindow).HasConversion<string>();

            builder.Ignore(_ => _.Feature);

            builder.ToTable("FeatureGenerators");

            builder.OwnsOne(_ => _.AggregateCustomerMetric, (b2) =>
            {
                b2.Navigation(_ => _.Metric).AutoInclude();
                b2.Property(_ => _.AggregationType).HasConversion<string>();

                b2.HasOne(_ => _.Metric)
                    .WithMany()
                    .HasForeignKey(_ => _.MetricId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            builder.OwnsOne(_ => _.JoinTwoMetrics, b2 =>
            {
                b2.Navigation(_ => _.Metric1).AutoInclude();
                b2.Navigation(_ => _.Metric2).AutoInclude();
                b2.Property(_ => _.JoinType).HasConversion<string>();

                b2
                    .HasOne(_ => _.Metric1)
                    .WithMany()
                    .HasForeignKey(_ => _.Metric1Id)
                    .OnDelete(DeleteBehavior.NoAction);
                b2
                    .HasOne(_ => _.Metric2)
                    .WithMany()
                    .HasForeignKey(_ => _.Metric2Id)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.HasData(MetricGenerator.TotalEventsGenerator);
        }
    }
}
