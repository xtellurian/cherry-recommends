using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class CustomerEventTypeConfiguration : EntityTypeConfigurationBase<CustomerEvent>, IEntityTypeConfiguration<CustomerEvent>
    {
        public override void Configure(EntityTypeBuilder<CustomerEvent> builder)
        {
            base.Configure(builder);

            builder.Property(_ => _.EventType).IsRequired();
            builder.Property(_ => _.EventKind).HasConversion<string>();
            builder.HasIndex(_ => new { _.EventId, _.EnvironmentId }).IsUnique();
            builder.HasIndex(_ => _.Timestamp);
            builder.Property(_ => _.Properties).HasJsonConversion();
            builder.Property(_ => _.CustomerId).HasColumnName("CommonUserId");
            // migrate so that customer is the main property
            builder.Ignore(_ => _.TrackedUser);
            builder.HasOne(_ => _.Customer).WithMany().HasForeignKey("TrackedUserId");
            builder.HasOne(_ => _.Environment).WithMany().OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("TrackedUserEvents"); // db backwards compat
        }
    }
}