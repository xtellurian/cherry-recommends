using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TrackedUserSystemMapTypeConfiguration : EntityTypeConfigurationBase<TrackedUserSystemMap>, IEntityTypeConfiguration<TrackedUserSystemMap>
    {
        public override void Configure(EntityTypeBuilder<TrackedUserSystemMap> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.UserId)
                .IsRequired();
            builder
                .HasIndex(_ => new { _.UserId, _.TrackedUserId, _.IntegratedSystemId })
                .IsUnique();
            builder.Ignore(_ => _.TrackedUser);
            builder.HasOne(_ => _.Customer).WithMany(_ => _.IntegratedSystemMaps).HasForeignKey(_ => _.TrackedUserId);
        }
    }
}