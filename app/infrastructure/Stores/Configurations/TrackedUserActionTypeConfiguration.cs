using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TrackedUserActionTypeConfiguration : EntityTypeConfigurationBase<TrackedUserAction>, IEntityTypeConfiguration<TrackedUserAction>
    {
        public override void Configure(EntityTypeBuilder<TrackedUserAction> builder)
        {
            base.Configure(builder);
            // use these two indexes to make finding the latest action for a tracked user fast.
            builder.HasIndex(_ => _.Timestamp);
            builder.HasIndex(_ => _.ActionName);
            builder.HasIndex(_ => _.CustomerId);
            builder.Property(_ => _.CustomerId).IsRequired();
            builder.Property(_ => _.CustomerId).HasColumnName("CommonUserId"); // backwards DB compat

            builder.Property(_ => _.ValueType).HasConversion<string>();

            builder.Ignore(_ => _.TrackedUser);
            builder.HasOne(_ => _.Customer).WithMany(_ => _.Actions).HasForeignKey(_ => _.TrackedUserId);
        }
    }
}