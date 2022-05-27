using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class PromotionsCampaignTypeConfiguration
    : CampaignEntityBaseTypeConfigurationBase<PromotionsCampaign>, IEntityTypeConfiguration<PromotionsCampaign>
    {
        public override void Configure(EntityTypeBuilder<PromotionsCampaign> builder)
        {
            builder.Property(_ => _.OldArguments).HasJsonConversion();
            builder.Property(_ => _.TriggerCollection).HasJsonConversion();
            builder.Property(_ => _.TargetType)
                .HasConversion<string>()
                .HasDefaultValue(PromotionCampaignTargetTypes.Customer); // TODO: remove default value after first migration

            builder
                .HasMany(_ => _.Items)
                .WithMany(_ => _.Recommenders)
                .UsingEntity(join => join.ToTable("ItemsRecommenderRecommendableItem"));

            builder
                .HasOne(_ => _.BaselineItem)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(_ => _.Recommendations)
                .WithOne(_ => _.Recommender)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(_ => _.UseOptimiser).HasDefaultValue(false);

            builder.HasDiscriminator()
                .HasValue<PromotionsCampaign>("ItemsRecommender"); // backwards compat old discrimintors
        }
    }
}