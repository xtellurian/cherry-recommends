using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class FeatureGeneratorTypeConfiguration : EntityTypeConfigurationBase<FeatureGenerator>, IEntityTypeConfiguration<FeatureGenerator>
    {
        public override void Configure(EntityTypeBuilder<FeatureGenerator> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.GeneratorType).HasConversion<string>();
            builder.Property(_ => _.FilterSelectAggregateSteps).HasJsonConversion();
        }
    }
}