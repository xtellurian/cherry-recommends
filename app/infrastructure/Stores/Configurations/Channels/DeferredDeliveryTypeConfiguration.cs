using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class DeferredDeliveryTypeConfiguration : EntityTypeConfigurationBase<DeferredDelivery>, IEntityTypeConfiguration<DeferredDelivery>
    {
        public override void Configure(EntityTypeBuilder<DeferredDelivery> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(_ => _.Channel)
                .WithMany()
                .HasForeignKey(_ => _.ChannelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(_ => _.Recommendation)
                .WithMany()
                .HasForeignKey(_ => _.RecommendationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(_ => new { _.ChannelId, _.RecommendationId }).IsUnique();
        }
    }
}
