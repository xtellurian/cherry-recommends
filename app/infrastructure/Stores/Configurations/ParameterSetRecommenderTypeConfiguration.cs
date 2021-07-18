using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ParameterSetRecommenderTypeConfiguration : RecommenderTypeConfigurationBase<ParameterSetRecommender>, IEntityTypeConfiguration<ParameterSetRecommender>
    {
        public override void Configure(EntityTypeBuilder<ParameterSetRecommender> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.ParameterBounds).HasJsonConversion();
            builder.Property(_ => _.Arguments).HasJsonConversion();
            builder.HasMany(_ => _.Recommendations).WithOne(_ => _.Recommender).OnDelete(DeleteBehavior.SetNull);
        }
    }
}