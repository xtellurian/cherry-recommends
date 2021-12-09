using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TrackedUserTypeConfiguration : CommonEntityTypeConfigurationBase<TrackedUser>, IEntityTypeConfiguration<TrackedUser>
    {
        public override void Configure(EntityTypeBuilder<TrackedUser> builder)
        {
            base.Configure(builder);

            builder.Property(_ => _.CommonId) // fixes legacy column name
                .HasColumnName("CommonUserId");

            builder.HasData(TrackedUser.Anonymous);
        }
    }
}