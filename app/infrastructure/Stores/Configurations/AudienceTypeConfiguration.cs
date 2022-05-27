using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class AudienceTypeConfiguration : EntityTypeConfigurationBase<Audience>
    {
        public override void Configure(EntityTypeBuilder<Audience> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(left => left.Recommender)
                .WithOne()
                .HasForeignKey<Audience>(_ => _.RecommenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(left => left.Segments)
                .WithMany(right => right.InAudience);
        }
    }
}