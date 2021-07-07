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
            builder.HasIndex(_ => _.CommonUserId);

            builder.Property(_ => _.ValueType).HasConversion<string>();
            builder.Property(_ => _.CommonUserId).IsRequired();
        }
    }
}