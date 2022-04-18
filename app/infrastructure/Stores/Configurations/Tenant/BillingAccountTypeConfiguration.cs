using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Accounts;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class BillingAccountTypeConfiguration : EntityTypeConfigurationBase<BillingAccount>, IEntityTypeConfiguration<BillingAccount>
    {
        public override void Configure(EntityTypeBuilder<BillingAccount> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.PlanType).HasConversion<string>();

            builder
                .HasMany(_ => _.Tenants)
                .WithOne(_ => _.Account)
                .HasForeignKey(_ => _.AccountId)
                .OnDelete(DeleteBehavior.Restrict); // don't delete the billing account, if we have a tenant in there
        }
    }
}