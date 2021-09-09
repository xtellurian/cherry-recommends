using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ItemsRecommenderTypeConfiguration
    : CommonEntityTypeConfigurationBase<ItemsRecommender>, IEntityTypeConfiguration<ItemsRecommender>
    {
        public override void Configure(EntityTypeBuilder<ItemsRecommender> builder)
        {
            builder.Property(_ => _.Arguments).HasJsonConversion();

            builder
                .HasMany(_ => _.Items)
                .WithMany(_ => _.Recommenders);

            builder
                .HasOne(_ => _.DefaultItem)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(_ => _.Recommendations)
                .WithOne(_ => _.Recommender)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}