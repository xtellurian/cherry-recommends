using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class FeatureTypeConfiguration : CommonEntityTypeConfigurationBase<Feature>, IEntityTypeConfiguration<Feature>
    {
        public override void Configure(EntityTypeBuilder<Feature> builder)
        {
            base.Configure(builder);

            builder
                .HasMany(_ => _.Recommenders)
                .WithMany(_ => _.LearningFeatures);
           
            builder
                .HasMany(_ => _.Destinations)
                .WithOne(_ => _.Feature);
        }
    }
}