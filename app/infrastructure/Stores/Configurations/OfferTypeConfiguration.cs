using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class OfferTypeConfiguration : EnvironmentScopedEntityTypeConfigurationBase<Offer>, IEntityTypeConfiguration<Offer>
    {
        public override void Configure(EntityTypeBuilder<Offer> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(_ => _.Recommendation)
                .WithOne(_ => _.Offer)
                .HasForeignKey<Offer>(_ => _.RecommendationId);

            builder
                .HasOne(_ => _.RedeemedPromotion)
                .WithMany()
                .HasForeignKey(_ => _.RedeemedPromotionId);

            builder.Property(_ => _.State).HasConversion<string>();
            builder.Property(_ => _.GrossRevenue).HasDefaultValue(1.0d);
        }
    }
}