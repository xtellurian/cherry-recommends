using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ProductRecommenderTypeConfiguration : RecommenderTypeConfigurationBase<ProductRecommender>, IEntityTypeConfiguration<ProductRecommender>
    {
        public override void Configure(EntityTypeBuilder<ProductRecommender> builder)
        {
            base.Configure(builder);
            builder.HasMany(_ => _.Products).WithMany(_ => _.ProductRecommenders);
            builder.HasOne(_ => _.Touchpoint).WithMany();
            builder.HasMany(_ => _.Recommendations).WithOne(_ => _.Recommender).OnDelete(DeleteBehavior.SetNull);
        }
    }
}