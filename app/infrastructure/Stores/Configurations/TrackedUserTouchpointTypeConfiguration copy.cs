using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TrackedUserFeatureTypeConfiguration : EntityTypeConfigurationBase<TrackedUserFeature>, IEntityTypeConfiguration<TrackedUserFeature>
    {
        public override void Configure(EntityTypeBuilder<TrackedUserFeature> builder)
        {
            base.Configure(builder);
            // ensure the uniqueness of a feature value
            builder.HasIndex(f => new { f.FeatureId, f.TrackedUserId, f.Version })
                .IsUnique();
        }
    }
}