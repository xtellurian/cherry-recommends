using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class RecommenderTypeConfigurationBase
       : CommonEntityTypeConfigurationBase<RecommenderEntityBase>, IEntityTypeConfiguration<RecommenderEntityBase>
    {
        public override void Configure(EntityTypeBuilder<RecommenderEntityBase> builder)
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
                .WithOne(_ => _.Recommender)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(_ => _.Arguments).IsRequired(false).HasJsonConversion();
            builder.Property(_ => _.ErrorHandling).HasJsonConversion();
            builder.Property(_ => _.Settings).HasJsonConversion();

            builder
                .HasMany(_ => _.RecommendationDestinations)
                .WithOne(_ => _.Recommender)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}