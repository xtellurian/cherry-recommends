using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TenantMembershipTypeConfiguration : EntityTypeConfigurationBase<TenantMembership>, IEntityTypeConfiguration<TenantMembership>
    {
        public override void Configure(EntityTypeBuilder<TenantMembership> builder)
        {
            base.Configure(builder);
            builder.HasIndex(_ => new { _.UserId, _.TenantId }).IsUnique();
        }
    }
}