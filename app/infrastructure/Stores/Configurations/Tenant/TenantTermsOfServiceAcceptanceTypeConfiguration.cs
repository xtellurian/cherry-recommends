using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TenantTermsOfServiceAcceptanceTypeConfiguration
    : EntityTypeConfigurationBase<TenantTermsOfServiceAcceptance>, IEntityTypeConfiguration<TenantTermsOfServiceAcceptance>
    {
        public override void Configure(EntityTypeBuilder<TenantTermsOfServiceAcceptance> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(_ => _.Tenant)
                .WithMany(_ => _.AcceptedTerms);

            builder
                .HasIndex(_ => new { Version = _.Version, AcceptedByUserId = _.AcceptedByUserId })
                .IsUnique();
        }
    }
}