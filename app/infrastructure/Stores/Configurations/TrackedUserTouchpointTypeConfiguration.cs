using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TrackedUserTouchpointTypeConfiguration : EntityTypeConfigurationBase<TrackedUserTouchpoint>, IEntityTypeConfiguration<TrackedUserTouchpoint>
    {
        public override void Configure(EntityTypeBuilder<TrackedUserTouchpoint> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.Values).HasJsonConversion();

            builder.Ignore(_ => _.TrackedUser);
            builder.HasOne(_ => _.Customer).WithMany(_ => _.TrackedUserTouchpoints).HasForeignKey(_ => _.TrackedUserId);
        }
    }
}