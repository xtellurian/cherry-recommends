using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class HistoricTrackedUserFeatureTypeConfiguration : EntityTypeConfigurationBase<HistoricTrackedUserFeature>, IEntityTypeConfiguration<HistoricTrackedUserFeature>
    {
        public override void Configure(EntityTypeBuilder<HistoricTrackedUserFeature> builder)
        {
            base.Configure(builder);
            // ensure the uniqueness of a feature value
            builder.HasIndex(f => new { f.FeatureId, f.TrackedUserId, f.Version })
                .IsUnique();

            builder.ToTable("HistoricTrackedUserFeatures");

            builder.Ignore(_ => _.TrackedUser);
            builder.HasOne(_ => _.Customer).WithMany(_ => _.HistoricTrackedUserFeatures).HasForeignKey(_ => _.TrackedUserId);
        }
    }
}