using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Optimisers;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class PromotionOptimiserTypeConfiguration : EnvironmentScopedEntityTypeConfigurationBase<PromotionOptimiser>, IEntityTypeConfiguration<PromotionOptimiser>
    {
        public override void Configure(EntityTypeBuilder<PromotionOptimiser> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(_ => _.Recommender)
                .WithOne(_ => _.Optimiser)
                .HasForeignKey<PromotionOptimiser>(_ => _.RecommenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .OwnsMany(_ => _.Weights, weight =>
                {
                    weight
                        .WithOwner(x => x.Optimiser)
                        .HasForeignKey(x => x.OptimiserId);

                    weight
                        .HasOne(_ => _.Segment)
                        .WithMany()
                        .HasForeignKey(_ => _.SegmentId)
                        .OnDelete(DeleteBehavior.Cascade);

                    weight
                        .HasOne(_ => _.Promotion)
                        .WithMany()
                        .HasForeignKey(_ => _.PromotionId)
                        .OnDelete(DeleteBehavior.Cascade);
                });

        }
    }
}