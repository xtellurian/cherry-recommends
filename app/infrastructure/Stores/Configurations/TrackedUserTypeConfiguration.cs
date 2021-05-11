using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TrackedUserTypeConfiguration : EntityTypeConfigurationBase<TrackedUser>, IEntityTypeConfiguration<TrackedUser>
    {
        public override void Configure(EntityTypeBuilder<TrackedUser> builder)
        {
            base.Configure(builder);

            builder.HasIndex(u => u.ExternalId)
                .IsUnique();
        }
    }
}