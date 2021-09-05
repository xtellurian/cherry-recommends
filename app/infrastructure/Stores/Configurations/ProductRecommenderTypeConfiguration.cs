using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ProductRecommenderTypeConfiguration : CommonEntityTypeConfigurationBase<ProductRecommender>, IEntityTypeConfiguration<ProductRecommender>
    {
        public override void Configure(EntityTypeBuilder<ProductRecommender> builder)
        {
            builder
                .HasMany(_ => _.Products)
                .WithMany(_ => _.ProductRecommenders);

            builder
                .HasOne(_ => _.DefaultProduct)
                .WithMany()
                .HasForeignKey(_ => _.DefaultProductId);

            builder.HasOne(_ => _.Touchpoint).WithMany();

            builder
                .HasMany(_ => _.ProductRecommendations)
                .WithOne(_ => _.Recommender)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}