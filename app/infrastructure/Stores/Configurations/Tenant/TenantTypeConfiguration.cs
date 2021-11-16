using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TenantTypeConfiguration : EntityTypeConfigurationBase<Tenant>, IEntityTypeConfiguration<Tenant>
    {
        public override void Configure(EntityTypeBuilder<Tenant> builder)
        {
            base.Configure(builder);
            builder.HasIndex(_ => _.Name).IsUnique();
            builder.Property(_ => _.Name).HasLowercaseConversion();
            builder.HasIndex(_ => _.DatabaseName).IsUnique();
        }
    }
}