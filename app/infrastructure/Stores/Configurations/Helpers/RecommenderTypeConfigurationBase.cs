using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class RecommenderEntityBaseTypeConfigurationBase<T>
       : CommonEntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : RecommenderEntityBase
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder
                .HasMany(_ => _.TargetVariableValues)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(_ => _.RecommenderInvokationLogs)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(_ => _.RecommendationCorrelators)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(_ => _.Arguments).IsRequired(false).HasJsonConversion();
            builder.Property(_ => _.ErrorHandling).HasJsonConversion();
            builder.Property(_ => _.TriggerCollection).HasJsonConversion();
            builder.Property(_ => _.Settings).HasJsonConversion();
        }
    }

    internal class RecommenderEntityBaseTypeConfiguration
       : RecommenderEntityBaseTypeConfigurationBase<RecommenderEntityBase>, IEntityTypeConfiguration<RecommenderEntityBase>
    {
        public override void Configure(EntityTypeBuilder<RecommenderEntityBase> builder)
        {
            base.Configure(builder);
        }
    }
}