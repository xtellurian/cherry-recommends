using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class RecommendableItemTypeConfiguration : CommonEntityTypeConfigurationBase<RecommendableItem>, IEntityTypeConfiguration<RecommendableItem>
    {
        protected override DeleteBehavior OnEnvironmentDelete => DeleteBehavior.SetNull;
        public override void Configure(EntityTypeBuilder<RecommendableItem> builder)
        {
            base.Configure(builder);

            builder
                .HasMany(_ => _.Recommenders)
                .WithMany(_ => _.Items);

            builder.Property(_ => _.PromotionType).HasDefaultValue(PromotionType.Other).HasConversion<string>();
            builder.Property(_ => _.BenefitType).HasDefaultValue(BenefitType.Fixed).HasConversion<string>();
            builder.Property(_ => _.BenefitValue).HasDefaultValue(1d);
            builder.Property(_ => _.NumberOfRedemptions).HasDefaultValue(1);
            builder.Property("Discriminator")
                .HasDefaultValue("Product")
                .IsRequired(false);

            builder.HasData(RecommendableItem.DefaultRecommendableItem);
        }
    }
}