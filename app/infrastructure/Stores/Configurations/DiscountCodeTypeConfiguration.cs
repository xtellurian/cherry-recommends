using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class DiscountCodeTypeConfiguration : EnvironmentScopedEntityTypeConfigurationBase<DiscountCode>, IEntityTypeConfiguration<DiscountCode>
    {
        public override void Configure(EntityTypeBuilder<DiscountCode> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(_ => _.Promotion)
                .WithMany()
                .HasForeignKey(_ => _.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(_ => _.Recommendations)
                .WithMany(_ => _.DiscountCodes);

            builder
                .HasMany(_ => _.GeneratedAt)
                .WithMany(_ => _.GeneratedDiscountCodes);

            builder.Property(_ => _.Code).HasMaxLength(15);
        }
    }
}