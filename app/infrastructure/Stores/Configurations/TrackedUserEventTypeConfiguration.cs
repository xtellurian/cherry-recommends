using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TrackedUserEventTypeConfiguration : EntityTypeConfigurationBase<TrackedUserEvent>, IEntityTypeConfiguration<TrackedUserEvent>
    {
        public override void Configure(EntityTypeBuilder<TrackedUserEvent> builder)
        {
            base.Configure(builder);

            builder.HasIndex(_ => _.EventId).IsUnique();
            builder.Property(_ => _.EventType).IsRequired();
            builder.Property(_ => _.Properties).HasJsonConversion();
        }
    }
}