using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ProductRecommenderTypeConfiguration : CommonEntityTypeConfigurationBase<ProductRecommender>, IEntityTypeConfiguration<ProductRecommender>
    {
        public override void Configure(EntityTypeBuilder<ProductRecommender> builder)
        {
            builder.Property(_ => _.Arguments).HasJsonConversion();

            builder
                .HasMany(_ => _.Products)
                .WithMany(_ => _.ProductRecommenders);

            builder
                .HasOne(_ => _.DefaultProduct)
                .WithMany()
                .HasForeignKey(_ => _.DefaultProductId);

            builder
                .HasMany(_ => _.ProductRecommendations)
                .WithOne(_ => _.Recommender)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}