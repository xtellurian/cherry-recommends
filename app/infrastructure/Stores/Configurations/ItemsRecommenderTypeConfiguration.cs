using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ItemsRecommenderTypeConfiguration
    : RecommenderEntityBaseTypeConfigurationBase<ItemsRecommender>, IEntityTypeConfiguration<ItemsRecommender>
    {
        public override void Configure(EntityTypeBuilder<ItemsRecommender> builder)
        {
            builder.Property(_ => _.Arguments).HasJsonConversion();
            builder.Property(_ => _.TriggerCollection).HasJsonConversion();
            builder.Property(_ => _.TargetType)
                .HasConversion<string>()
                .HasDefaultValue(PromotionRecommenderTargetTypes.Customer); // TODO: remove default value after first migration

            builder
                .HasMany(_ => _.Items)
                .WithMany(_ => _.Recommenders);

            builder
                .HasOne(_ => _.BaselineItem)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(_ => _.Recommendations)
                .WithOne(_ => _.Recommender)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(_ => _.UseOptimiser).HasDefaultValue(false);
        }
    }
}