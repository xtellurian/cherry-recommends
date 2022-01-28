using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class MetricTypeConfiguration : CommonEntityTypeConfigurationBase<Metric>, IEntityTypeConfiguration<Metric>
    {
        public override void Configure(EntityTypeBuilder<Metric> builder)
        {
            base.Configure(builder);

            builder.ToTable("Features"); // backwards compat with database

            builder
                .HasMany(_ => _.Recommenders)
                .WithMany(_ => _.LearningFeatures)
                .UsingEntity(join => join.ToTable("FeatureRecommenderEntityBase"));

            builder
                .HasMany(_ => _.Destinations)
                .WithOne(_ => _.Metric);
        }
    }
}