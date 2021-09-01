using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class RecommenderTypeConfigurationBase
       : EntityTypeConfigurationBase<RecommenderEntityBase>, IEntityTypeConfiguration<RecommenderEntityBase>
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
                .WithOne(_ => _.Recommender);

            builder.Property(_ => _.ErrorHandling).HasJsonConversion();
        }
    }
}